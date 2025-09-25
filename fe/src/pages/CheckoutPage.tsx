import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useApp } from "../context/useApp";
import { useToast } from "../hooks/useToast";
import {
  createOrder,
  payForOrder,
  getCustomer,
  addNewCustomer,
} from "../api/purchaseService";
import type { Customer } from "../types/purchase/Customer";
import type { OrderItem } from "../types/purchase/OrderItem";
import { CustomerCheckoutPage } from "./checkout/CustomerCheckoutPage";
import type { AddAddress } from "../types/purchase/AddAddress";
import { CreditCard } from "lucide-react";
import { clearCart } from "../api/cartService";

export function CheckoutPage() {
  const { state, dispatch } = useApp();
  const navigate = useNavigate();
  const { showSuccess, showError } = useToast();
  const [customer, setCustomer] = useState<Customer | null>(null);
  const [customerForm, setCustomerForm] = useState({
    firstName: state.user?.Name || "",
    lastName: state.user?.Surname || "",
    email: state.user?.Email || "",
    phoneNumber: "",
  });
  const [customerLoading, setCustomerLoading] = useState(true);

  const [newAddressForm, setNewAddressForm] = useState<AddAddress>({
    Street: "",
    City: "",
    PostalCode: "",
    Country: "",
  });

  const [selectedAddressId, setSelectedAddressId] = useState("");
  const [paymentMethod, setPaymentMethod] = useState<number>(1);

  const handleSaveCustomer = async () => {
    // Walidacja na froncie
    if (!customerForm.firstName.trim()) {
      showError("Imię jest wymagane!");
      return;
    }
    if (!customerForm.lastName.trim()) {
      showError("Nazwisko jest wymagane!");
      return;
    }
    if (!customerForm.email.trim()) {
      showError("Email jest wymagany!");
      return;
    }
    if (
      !customerForm.phoneNumber.trim() ||
      customerForm.phoneNumber.length < 7
    ) {
      showError("Numer telefonu musi mieć co najmniej 7 znaków!");
      return;
    }

    try {
      await addNewCustomer({
        FirstName: customerForm.firstName,
        LastName: customerForm.lastName,
        Email: customerForm.email,
        PhoneNumber: customerForm.phoneNumber,
        UserId: state.user?.IdUser || "",
        Addresses: [],
      });
      showSuccess("Dane klienta zapisane!");

      setCustomerLoading(true);
      await getCustomer()
        .then((res) => setCustomer(res.data || null))
        .catch(() => setCustomer(null))
        .finally(() => setCustomerLoading(false));
    } catch {
      showError("Nie udało się zapisać danych klienta!");
    }
  };

  const total = state.cart.reduce(
    (sum, item) => sum + item.product.price * item.quantity,
    0
  );

  const handleCreateOrder = async () => {
    if (!customer || !selectedAddressId) {
      showError("Wybierz adres dostawy!");
      return;
    }

    const orderItems: OrderItem[] = state.cart.map((item) => ({
      productId: item.product.id,
      productName: item.product.name,
      productPrice: item.product.price,
      quantity: item.quantity,
    }));

    let error = "Wystąpił błąd";

    try {
      const resp = await createOrder({
        CustomerId: customer.idCustomer,
        ShippingAddressId: selectedAddressId,
        Status: 0,
        CreatedAt: new Date(),
        PaidAt: new Date(),
        Items: orderItems,
        Payments: [],
      });
      if (resp.status !== 200) {
        error = "Błąd podczas tworzenia zamówienia.";
        throw new Error(error);
      }
      console.log(resp.data);
      const payResp = await payForOrder({
        OrderId: resp.data,
        Method: paymentMethod,
        PaymentDate: new Date(),
        Details: "Symulowana płatność edukacyjna",
      });

      if (payResp.status !== 200) {
        error = "Błąd podczas realizacji płatności.";
        throw new Error(error);
      }

      showSuccess("Zamówienie złożone i opłacone!");
      dispatch({ type: "CLEAR_CART" });
      clearCart();
      navigate("/orders");
    } catch {
      showError(error);
    }
  };

  if (state.cart.length === 0) {
    navigate("/cart");
    return null;
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white mb-8">
          Finalizacja zamówienia
        </h1>

        <form
          onSubmit={(e) => {
            e.preventDefault();
            handleCreateOrder();
          }}
          className="grid grid-cols-1 lg:grid-cols-2 gap-8"
        >
          <div>
            {/* Left Column - Forms */}
            <div className="space-y-6"></div>
            <CustomerCheckoutPage
              customer={customer}
              customerForm={customerForm}
              newAddressForm={newAddressForm}
              customerLoading={customerLoading}
              paymentMethod={paymentMethod}
              selectedAddressId={selectedAddressId}
              setCustomer={setCustomer}
              setCustomerForm={setCustomerForm}
              handleSaveCustomer={handleSaveCustomer}
              setNewAddressForm={setNewAddressForm}
              setPaymentMethod={setPaymentMethod}
              setCustomerLoading={setCustomerLoading}
              setSelectedAddressId={setSelectedAddressId}
            />
          </div>
          {/* Right Column - Order Summary */}
          <div className="p-0 sticky top-4">
            {" "}
            {/* Usunięte tło */}
            <h2 className="flex items-center gap-2 text-lg font-semibold dark:text-white mb-4">
              <CreditCard className="w-5 h-5 text-primary-600" />
              Podsumowanie zamówienia
            </h2>
            <div className="space-y-4 mb-6">
              {state.cart.map((item, index) => (
                <div
                  key={index}
                  className="flex items-center p-4 rounded-xl border-2 border-gray-200 dark:border-gray-700
          bg-white dark:bg-gray-800 gap-4"
                >
                  <img
                    src={item.product.image}
                    alt={item.product.name}
                    className="w-14 h-14 object-cover rounded-lg border border-gray-200 dark:border-gray-700 flex-shrink-0"
                  />
                  <div className="flex-1 min-w-0">
                    <h4 className="text-base font-semibold text-gray-900 dark:text-white break-words">
                      {item.product.name}
                    </h4>
                    <p className="text-xs text-gray-500 dark:text-gray-400 mt-1">
                      {item.size} | {item.color} |{" "}
                      <span className="font-semibold">x{item.quantity}</span>
                    </p>
                  </div>
                  <span className="text-base font-bold text-gray-900 dark:text-white whitespace-nowrap">
                    {(item.product.price * item.quantity).toFixed(2)} zł
                  </span>
                </div>
              ))}
            </div>
            <div className="border-t border-gray-200 dark:border-gray-700 pt-4 space-y-2">
              <div className="flex justify-between text-sm">
                <span className="text-gray-600 dark:text-gray-400">
                  Suma częściowa:
                </span>
                <span className="text-gray-900 dark:text-white">
                  {total.toFixed(2)} zł
                </span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-gray-600 dark:text-gray-400">
                  Dostawa:
                </span>
                <span className="text-green-600 font-medium">Darmowa</span>
              </div>
              <div className="flex justify-between text-lg font-bold pt-2">
                <span className="text-gray-900 dark:text-white">Razem:</span>
                <span className="text-gray-900 dark:text-white">
                  {total.toFixed(2)} zł
                </span>
              </div>
            </div>
            <button
              type="submit"
              className="w-full mt-6 py-3 rounded-2xl bg-primary-600 hover:bg-primary-700 text-white font-bold text-lg shadow-lg transition-colors"
            >
              Zapłać {total.toFixed(2)} zł
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
