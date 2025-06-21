import React from "react";
import { Link } from "react-router-dom";
import { Minus, Plus, Trash2, ShoppingBag } from "lucide-react";
import { useApp } from "../context/useApp";

export function CartPage() {
  const { state, dispatch } = useApp();

  const updateQuantity = (productId: string, newQuantity: number) => {
    if (newQuantity === 0) {
      dispatch({ type: "REMOVE_FROM_CART", payload: productId });
    } else {
      dispatch({
        type: "UPDATE_CART_QUANTITY",
        payload: { productId, quantity: newQuantity },
      });
    }
  };

  const removeItem = (index: number) => {
    dispatch({ type: "REMOVE_FROM_CART", payload: index.toString() });
  };

  const total = state.cart.reduce(
    (sum, item) => sum + item.product.price * item.quantity,
    0
  );

  const proceedToCheckout = () => {
    if (!state.user) {
      dispatch({ type: "TOGGLE_AUTH_MODAL", payload: "login" });
      return;
    }

    // Redirect to checkout
    window.location.href = "/checkout";
  };

  if (state.cart.length === 0) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center">
        <div className="text-center">
          <ShoppingBag className="w-16 h-16 text-gray-400 mx-auto mb-4" />
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-4">
            Twój koszyk jest pusty
          </h2>
          <p className="text-gray-600 dark:text-gray-400 mb-8">
            Dodaj produkty do koszyka, aby kontynuować zakupy
          </p>
          <Link
            to="/"
            className="bg-primary-600 hover:bg-primary-700 text-white px-6 py-3 rounded-lg font-medium transition-colors"
          >
            Kontynuuj zakupy
          </Link>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white mb-8">
          Koszyk ({state.cart.length})
        </h1>

        <div className="bg-white dark:bg-gray-800 rounded-lg shadow-sm">
          {state.cart.map((item, index) => (
            <div
              key={index}
              className="flex items-center p-6 border-b border-gray-200 dark:border-gray-700 last:border-b-0"
            >
              <img
                src={item.product.image}
                alt={item.product.name}
                className="w-20 h-20 object-cover rounded-lg"
              />

              <div className="flex-1 ml-4">
                <h3 className="text-lg font-medium text-gray-900 dark:text-white">
                  {item.product.name}
                </h3>
                <p className="text-sm text-gray-600 dark:text-gray-400">
                  Rozmiar: {item.size} | Kolor: {item.color}
                </p>
                <p className="text-lg font-bold text-gray-900 dark:text-white mt-1">
                  {item.product.price.toFixed(2)} zł
                </p>
              </div>

              <div className="flex items-center space-x-3">
                <button
                  onClick={() =>
                    updateQuantity(item.product.id, item.quantity - 1)
                  }
                  className="w-8 h-8 border border-gray-300 dark:border-gray-600 rounded-md flex items-center justify-center hover:bg-gray-50 dark:hover:bg-gray-700"
                >
                  <Minus className="w-4 h-4" />
                </button>

                <span className="text-lg font-medium text-gray-900 dark:text-white w-8 text-center">
                  {item.quantity}
                </span>

                <button
                  onClick={() =>
                    updateQuantity(item.product.id, item.quantity + 1)
                  }
                  className="w-8 h-8 border border-gray-300 dark:border-gray-600 rounded-md flex items-center justify-center hover:bg-gray-50 dark:hover:bg-gray-700"
                >
                  <Plus className="w-4 h-4" />
                </button>
              </div>

              <div className="ml-6">
                <p className="text-lg font-bold text-gray-900 dark:text-white">
                  {(item.product.price * item.quantity).toFixed(2)} zł
                </p>
              </div>

              <button
                onClick={() => removeItem(index)}
                className="ml-4 p-2 text-red-600 hover:text-red-800 dark:text-red-400 dark:hover:text-red-300"
              >
                <Trash2 className="w-5 h-5" />
              </button>
            </div>
          ))}
        </div>

        {/* Summary */}
        <div className="mt-8 bg-white dark:bg-gray-800 rounded-lg shadow-sm p-6">
          <div className="flex justify-between items-center mb-4">
            <span className="text-lg font-medium text-gray-900 dark:text-white">
              Suma częściowa:
            </span>
            <span className="text-lg font-medium text-gray-900 dark:text-white">
              {total.toFixed(2)} zł
            </span>
          </div>

          <div className="flex justify-between items-center mb-4">
            <span className="text-lg font-medium text-gray-900 dark:text-white">
              Dostawa:
            </span>
            <span className="text-lg font-medium text-green-600">Darmowa</span>
          </div>

          <div className="border-t border-gray-200 dark:border-gray-700 pt-4">
            <div className="flex justify-between items-center mb-6">
              <span className="text-xl font-bold text-gray-900 dark:text-white">
                Razem:
              </span>
              <span className="text-xl font-bold text-gray-900 dark:text-white">
                {total.toFixed(2)} zł
              </span>
            </div>

            <div className="flex space-x-4">
              <Link
                to="/"
                className="flex-1 border border-gray-300 dark:border-gray-600 text-gray-700 dark:text-gray-300 py-3 px-6 rounded-lg font-medium text-center hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
              >
                Kontynuuj zakupy
              </Link>
              <button
                onClick={proceedToCheckout}
                className="flex-1 bg-primary-600 hover:bg-primary-700 text-white py-3 px-6 rounded-lg font-medium transition-colors"
              >
                Przejdź do płatności
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
