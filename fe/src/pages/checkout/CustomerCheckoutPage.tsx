import { useEffect, useState } from "react";
import { addNewAddress, getCustomer } from "../../api/purchaseService";
import type { AddAddress } from "../../types/purchase/AddAddress";
import type { Customer } from "../../types/purchase/Customer";
import { Loader } from "../../components/common/Loader";
import { CreditCard, MapPin, User } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useToast } from "../../hooks/useToast";

interface CustomerCheckoutPageProps {
  customer: Customer | null;
  customerForm: {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
  };
  newAddressForm: AddAddress;
  customerLoading: boolean;
  paymentMethod: number;
  selectedAddressId: string;
  setCustomer: (customer: Customer | null) => void;
  setCustomerForm: (customer: {
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
  }) => void;
  handleSaveCustomer: () => void;
  setNewAddressForm: (newAddress: AddAddress) => void;
  setPaymentMethod: (paymentMethod: number) => void;
  setCustomerLoading: (isLoading: boolean) => void;
  setSelectedAddressId: (selectedAddressId: string) => void;
}
export function CustomerCheckoutPage({
  customer,
  customerForm,
  newAddressForm,
  customerLoading,
  paymentMethod,
  selectedAddressId,
  setCustomer,
  setCustomerForm,
  handleSaveCustomer,
  setNewAddressForm,
  setPaymentMethod,
  setCustomerLoading,
  setSelectedAddressId,
}: CustomerCheckoutPageProps) {
  const [newAddressFormVisible, setNewAddressFormVisible] =
    useState<boolean>(false);
  const navigate = useNavigate();
  const { showError } = useToast();

  useEffect(() => {
    setCustomerLoading(true);
    getCustomer()
      .then((res) => {
        if (res.status === 401) {
          navigate("/");
          showError("Twoja sesja wygasła. Zaloguj się ponownie");
        }
        setCustomer(res.data || null);
      })
      .catch(() => {
        setCustomer(null);
      })

      .finally(() => setCustomerLoading(false));
  }, []);

  const handleCustomerInputChange = (
    e: React.ChangeEvent<HTMLInputElement>
  ) => {
    setCustomerForm({
      ...customerForm,
      [e.target.name]: e.target.value,
    });
  };
  const paymentMethods = [
    { label: "Blik", icon: <CreditCard className="w-6 h-6" /> },
    { label: "Przelew", icon: <CreditCard className="w-6 h-6" /> },
    { label: "ApplePay", icon: <CreditCard className="w-6 h-6" /> },
    { label: "AndroidPay", icon: <CreditCard className="w-6 h-6" /> },
  ];

  if (!customer) {
    return (
      <div className="mb-8 p-4 sm:p-6 rounded-2xl border-2 border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800 shadow flex flex-col gap-4">
        <h2 className="text-lg font-semibold text-gray-900 dark:text-white flex items-center mb-1">
          <User className="inline-block mr-2" />
          Dane osobowe
        </h2>
        <div className="mb-2 text-gray-600 dark:text-gray-400">
          To Twoje pierwsze zakupy u nas! Wypełnij dane osobowe, aby utworzyć
          konto i przejść dalej.
        </div>
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Imię *
            </label>
            <input
              type="text"
              name="firstName"
              required
              value={customerForm.firstName}
              onChange={handleCustomerInputChange}
              className="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-xl text-gray-900 dark:text-gray-100 bg-white dark:bg-gray-800 focus:ring-2 focus:ring-primary-400 transition-all"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Nazwisko *
            </label>
            <input
              type="text"
              name="lastName"
              required
              value={customerForm.lastName}
              onChange={handleCustomerInputChange}
              className="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-xl text-gray-900 dark:text-gray-100 bg-white dark:bg-gray-800 focus:ring-2 focus:ring-primary-400 transition-all"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Email *
            </label>
            <input
              type="email"
              name="email"
              required
              value={customerForm.email}
              onChange={handleCustomerInputChange}
              className="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-xl text-gray-900 dark:text-gray-100 bg-white dark:bg-gray-800 focus:ring-2 focus:ring-primary-400 transition-all"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Telefon *
            </label>
            <input
              type="tel"
              name="phoneNumber"
              required
              value={customerForm.phoneNumber}
              onChange={handleCustomerInputChange}
              className="w-full px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-xl text-gray-900 dark:text-gray-100 bg-white dark:bg-gray-800 focus:ring-2 focus:ring-primary-400 transition-all"
            />
          </div>
        </div>
        <button
          type="button"
          onClick={handleSaveCustomer}
          className="mt-4 bg-primary-600 hover:bg-primary-700 text-white py-2 px-6 rounded-2xl font-semibold transition w-full sm:w-auto"
        >
          Załóż konto i przejdź dalej
        </button>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      {/* Sekcja wyboru/utworzenia customera */}
      <div>
        <h2 className="text-lg font-semibold dark:text-white mb-4 flex items-center">
          <User className="inline-block mr-2" />
          Dane osobowe
        </h2>
        {/* Box z danymi klienta */}
        <div className="mb-8 p-4 sm:p-6 rounded-2xl border-2 border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800 shadow flex flex-col gap-1">
          {customerLoading ? (
            <Loader />
          ) : (
            <>
              <div className="font-semibold text-lg text-gray-900 dark:text-white">
                {customer.firstName} {customer.lastName}
              </div>
              <div className="text-sm text-gray-500 dark:text-gray-300">
                {customer.email}
              </div>
              <div className="text-sm text-gray-500 dark:text-gray-300">
                {customer.phoneNumber}
              </div>
            </>
          )}
        </div>
      </div>

      <div className="mb-4">
        <h2 className="text-lg font-semibold dark:text-white mb-4 flex items-center">
          <MapPin className="inline-block mr-2" /> Adres dostawy
        </h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
          {customer?.addresses &&
            customer.addresses.length > 0 &&
            customer.addresses.map((addr) => (
              <button
                key={addr.idAddress}
                type="button"
                className={`flex flex-col items-start p-4 rounded-2xl border-2 shadow transition-all text-left
            ${
              selectedAddressId === addr.idAddress
                ? "border-primary-600 bg-primary-50 dark:bg-primary-900 text-primary-800 dark:text-white"
                : "border-gray-300 bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-200"
            }
            hover:scale-105 hover:shadow-lg`}
                onClick={() => setSelectedAddressId(addr.idAddress)}
              >
                <div className="font-semibold">{addr.street}</div>
                <div className="text-sm">
                  {addr.postalCode} {addr.city}
                </div>
                <div className="text-xs text-gray-500">{addr.country}</div>
              </button>
            ))}
          {/* Przycisk dodania nowego adresu */}
          <button
            type="button"
            onClick={() => setNewAddressFormVisible(true)}
            className="flex flex-col items-center justify-center p-4 rounded-2xl border-2 border-dashed border-primary-400 text-primary-600 bg-white dark:bg-gray-800 hover:bg-primary-50 hover:scale-105 hover:shadow-lg transition-all"
          >
            <span className="text-2xl font-bold">+</span>
            <span className="mt-2 font-medium">Dodaj nowy adres</span>
          </button>
        </div>
      </div>

      {/* Formularz dodawania nowego adresu */}
      {newAddressFormVisible && (
        <div className="mb-6">
          <h3 className="text-lg font-semibold dark:text-white mb-2 flex items-center">
            <MapPin className="inline-block mr-2" /> Nowy adres dostawy
          </h3>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4 mb-2">
            <input
              type="text"
              name="street"
              placeholder="Ulica i numer"
              value={newAddressForm.Street}
              onChange={(e) =>
                setNewAddressForm({ ...newAddressForm, Street: e.target.value })
              }
              className="col-span-2 px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-xl text-gray-900 dark:text-gray-100 bg-white dark:bg-gray-800 focus:ring-2 focus:ring-primary-400 transition-all"
              required
            />
            <input
              type="text"
              name="city"
              placeholder="Miasto"
              value={newAddressForm.City}
              onChange={(e) =>
                setNewAddressForm({ ...newAddressForm, City: e.target.value })
              }
              className="px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-xl text-gray-900 dark:text-gray-100 bg-white dark:bg-gray-800 focus:ring-2 focus:ring-primary-400 transition-all"
              required
            />
            <input
              type="text"
              name="postalCode"
              placeholder="Kod pocztowy"
              value={newAddressForm.PostalCode}
              onChange={(e) =>
                setNewAddressForm({
                  ...newAddressForm,
                  PostalCode: e.target.value,
                })
              }
              className="px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-xl text-gray-900 dark:text-gray-100 bg-white dark:bg-gray-800 focus:ring-2 focus:ring-primary-400 transition-all"
              required
            />
            <input
              type="text"
              name="country"
              placeholder="Kraj"
              value={newAddressForm.Country}
              onChange={(e) =>
                setNewAddressForm({
                  ...newAddressForm,
                  Country: e.target.value,
                })
              }
              className="px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-xl text-gray-900 dark:text-gray-100 bg-white dark:bg-gray-800 focus:ring-2 focus:ring-primary-400 transition-all"
              required
            />
          </div>
          <div className="flex gap-3 mt-4 justify-end">
            <button
              className="bg-primary-600 hover:bg-primary-700 text-white py-2 px-6 rounded-2xl font-semibold transition"
              type="button"
              onClick={async () => {
                await addNewAddress(newAddressForm);
                const res = await getCustomer();
                setCustomer(res.data || null);

                const newId = res.data?.addresses?.at(-1)?.idAddress;
                if (newId) setSelectedAddressId(newId);
                setNewAddressFormVisible(false);
              }}
            >
              Zapisz
            </button>
            <button
              className="bg-gray-300 hover:bg-gray-400 text-gray-900 py-2 px-6 rounded-2xl font-semibold transition"
              onClick={() => setNewAddressFormVisible(false)}
            >
              Anuluj
            </button>
          </div>
        </div>
      )}

      {/* Payment Information */}
      <div className="mb-6">
        <h2 className="text-lg font-semibold dark:text-white mb-4 flex items-center">
          <CreditCard className="inline-block mr-2" /> Metoda płatności
        </h2>
        <div className="grid grid-cols-2 gap-4">
          {paymentMethods.map((method, idx) => (
            <button
              key={method.label}
              onClick={() => setPaymentMethod(idx + 1)}
              type="button"
              className={`flex flex-col items-center justify-center p-4 rounded-2xl shadow border-2 transition-all
          ${
            paymentMethod === idx + 1
              ? "border-primary-600 bg-primary-50 dark:bg-primary-900 text-primary-800 dark:text-white"
              : "border-gray-300 bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-200"
          }
          hover:scale-105 hover:shadow-lg`}
            >
              {method.icon}
              <span className="mt-2 font-medium">{method.label}</span>
            </button>
          ))}
        </div>
      </div>
    </div>
  );
}
