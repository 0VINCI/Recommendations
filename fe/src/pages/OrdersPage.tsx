import React, { useEffect } from "react";
import { Link } from "react-router-dom";
import { Package, Calendar, MapPin } from "lucide-react";
import { useApp } from "../context/useApp";
import { Loader } from "../components/common/Loader";
import { getMyOrders } from "../api/purchaseService";
import type { OrderItem } from "../types/purchase/OrderItem";
import { getProductById } from "../api/productService";

export function OrdersPage() {
  const { state, dispatch } = useApp();
  const [loading, setLoading] = React.useState(false);

  useEffect(() => {
    if (!state.user) return;
    setLoading(true);

    getMyOrders()
      .then(async (res) => {
        const orders = res.data ?? [];
        const productIds = [
          ...new Set(
            orders.flatMap((order) => order.items.map((item) => item.productId))
          ),
        ];
        const productsWithImages: Record<string, string> = {};
        await Promise.all(
          productIds.map(async (productId) => {
            try {
              const res = await getProductById({ productId });
              if (
                res.status === 200 &&
                res.data &&
                res.data.product &&
                res.data.product.images &&
                res.data.product.images.length > 0
              ) {
                productsWithImages[productId] =
                  res.data.product.images.find((img) => img.isPrimary)
                    ?.imageUrl || res.data.product.images[0].imageUrl;
              } else {
                productsWithImages[productId] = "/placeholder.png";
              }
            } catch {
              productsWithImages[productId] = "/placeholder.png";
            }
          })
        );
        const mappedOrders = orders.map((order) => ({
          ...order,
          createdAt: new Date(order.createdAt),
          paidAt: order.paidAt ? new Date(order.paidAt) : null,
          items: order.items.map((item) => ({
            ...item,
            image: productsWithImages[item.productId] || "/placeholder.png",
          })),
        }));
        dispatch({ type: "SET_ORDERS", payload: mappedOrders });
      })
      .finally(() => setLoading(false));
  }, [state.user, dispatch]);

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center">
        <Loader />
      </div>
    );
  }

  if (!state.user) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center">
        <div className="text-center">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-4">
            Musisz być zalogowany
          </h2>
          <p className="text-gray-600 dark:text-gray-400 mb-8">
            Zaloguj się, aby zobaczyć swoje zamówienia
          </p>
          <button
            onClick={() =>
              dispatch({ type: "TOGGLE_AUTH_MODAL", payload: "login" })
            }
            className="bg-primary-600 hover:bg-primary-700 text-white px-6 py-3 rounded-lg font-medium transition-colors"
          >
            Zaloguj się
          </button>
        </div>
      </div>
    );
  }

  const userOrders = state.orders;

  const getStatusInfo = (status: number) => {
    switch (status) {
      case 0:
        return {
          text: "Oczekuje",
          color:
            "bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-300",
        };
      case 1:
        return {
          text: "W realizacji",
          color:
            "bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-300",
        };
      case 2:
        return {
          text: "Wysłane",
          color:
            "bg-purple-100 text-purple-800 dark:bg-purple-900 dark:text-purple-300",
        };
      case 3:
        return {
          text: "Dostarczone",
          color:
            "bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300",
        };
      default:
        return {
          text: String(status),
          color:
            "bg-gray-100 text-gray-800 dark:bg-gray-900 dark:text-gray-300",
        };
    }
  };

  if (userOrders.length === 0) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white mb-8">
            Moje zamówienia
          </h1>
          <div className="text-center py-12">
            <Package className="w-16 h-16 text-gray-400 mx-auto mb-4" />
            <h2 className="text-xl font-medium text-gray-900 dark:text-white mb-4">
              Nie masz jeszcze żadnych zamówień
            </h2>
            <p className="text-gray-600 dark:text-gray-400 mb-8">
              Rozpocznij zakupy, aby zobaczyć swoje zamówienia tutaj
            </p>
            <Link
              to="/"
              className="bg-primary-600 hover:bg-primary-700 text-white px-6 py-3 rounded-lg font-medium transition-colors"
            >
              Rozpocznij zakupy
            </Link>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white mb-8">
          Moje zamówienia ({userOrders.length})
        </h1>
        <div className="space-y-6">
          {userOrders.map((order) => {
            const total = order.items.reduce(
              (sum: number, item: OrderItem) =>
                sum + item.productPrice * item.quantity,
              0
            );
            const status = getStatusInfo(order.status);
            return (
              <>
                <div
                  key={order.id}
                  className="bg-white dark:bg-gray-800 rounded-lg shadow-sm p-6"
                >
                  {/* Order Header */}
                  <div className="flex items-center justify-between mb-4">
                    <div>
                      <h3 className="text-lg font-medium text-gray-900 dark:text-white">
                        Zamówienie #{order.id}
                      </h3>
                      <div className="flex items-center space-x-2 text-sm text-gray-600 dark:text-gray-400">
                        <Calendar className="w-4 h-4" />
                        <span>
                          {order.createdAt.toLocaleDateString("pl-PL")}
                        </span>
                      </div>
                    </div>
                    <div className="text-right">
                      <span
                        className={`inline-flex px-2 py-1 text-xs font-medium rounded-full ${status.color}`}
                      >
                        {status.text}
                      </span>
                      <p className="text-lg font-bold text-gray-900 dark:text-white mt-1">
                        {total.toFixed(2)} zł
                      </p>
                    </div>
                  </div>

                  {/* Order Items */}
                  <div className="border-t border-gray-200 dark:border-gray-700 pt-4 mb-4">
                    <div className="space-y-3">
                      {order.items.map((item: OrderItem, idx: number) => (
                        <div key={idx} className="flex items-center space-x-3">
                          <img
                            src={item.image}
                            alt={item.productName}
                            className="w-12 h-12 object-cover rounded"
                          />
                          <div className="flex-1">
                            <h4 className="text-sm font-medium text-gray-900 dark:text-white">
                              {item.productName}
                            </h4>
                            <p className="text-xs text-gray-600 dark:text-gray-400">
                              Ilość: {item.quantity}
                            </p>
                          </div>
                          <span className="text-sm font-medium text-gray-900 dark:text-white">
                            {(item.productPrice * item.quantity).toFixed(2)} zł
                          </span>
                        </div>
                      ))}
                    </div>
                  </div>

                  {/* Shipping Address Placeholder */}
                  <div className="border-t border-gray-200 dark:border-gray-700 pt-4">
                    <div className="flex items-start space-x-2">
                      <MapPin className="w-4 h-4 text-gray-400 mt-0.5" />
                      <div>
                        <p className="text-sm font-medium text-gray-900 dark:text-white">
                          Adres dostawy:
                        </p>
                        {order.address && (
                          <p className="text-sm text-gray-600 dark:text-gray-400">
                            {order.address.street}
                            <br />
                            {order.address.postalCode} {order.address.city}
                            <br />
                            {order.address.country}
                          </p>
                        )}
                      </div>
                    </div>
                  </div>
                </div>
              </>
            );
          })}
        </div>
      </div>
    </div>
  );
}
