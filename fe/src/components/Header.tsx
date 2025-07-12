import { Link } from "react-router-dom";
import { ShoppingCart, Sun, Moon, Search, Menu, LogOut } from "lucide-react";
import { useApp } from "../context/useApp";
import { signOut } from "../api/authorizationService";

export function Header() {
  const { state, dispatch } = useApp();

  const toggleTheme = () => {
    dispatch({
      type: "SET_THEME",
      payload: state.theme === "light" ? "dark" : "light",
    });
  };

  const openAuthModal = (mode: "login" | "register") => {
    dispatch({ type: "TOGGLE_AUTH_MODAL", payload: mode });
  };

  const handleSignOut = async () => {
    try {
      await signOut();
      dispatch({ type: "SET_USER", payload: null });
    } catch (error) {
      console.error("Błąd podczas wylogowania:", error);
    }
  };

  const cartItemsCount = state.cart.reduce(
    (sum, item) => sum + item.quantity,
    0
  );

  return (
    <header className="bg-white dark:bg-gray-900 shadow-sm border-b border-gray-200 dark:border-gray-700">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          {/* Logo */}
          <Link to="/" className="flex items-center space-x-2">
            <div className="w-8 h-8 bg-primary-600 rounded-lg flex items-center justify-center">
              <span className="text-white font-bold text-lg">S</span>
            </div>
            <span className="text-xl font-bold text-gray-900 dark:text-white">
              StyleShop
            </span>
          </Link>

          {/* Search Bar */}
          <div className="hidden md:flex flex-1 max-w-lg mx-8">
            <div className="relative w-full">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
              <input
                type="text"
                placeholder="Szukaj produktów..."
                className="w-full pl-10 pr-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-800 text-gray-900 dark:text-white"
              />
            </div>
          </div>

          {/* Actions */}
          <div className="flex items-center space-x-4">
            {/* Theme Toggle */}
            <button
              onClick={toggleTheme}
              className="p-2 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors"
            >
              {state.theme === "light" ? (
                <Moon className="w-5 h-5" />
              ) : (
                <Sun className="w-5 h-5" />
              )}
            </button>

            {/* User Menu */}
            {state.user ? (
              <div className="flex items-center space-x-2">
                <img
                  src={`https://ui-avatars.com/api/?name=${state.user.Name} ${state.user.Surname}&background=0ea5e9&color=fff`}
                  alt={`${state.user.Name} ${state.user.Surname}`}
                  className="w-8 h-8 rounded-full"
                />
                <span className="hidden md:block text-sm text-gray-700 dark:text-gray-300">
                  {state.user.Name} {state.user.Surname}
                </span>
                <button
                  onClick={handleSignOut}
                  className="p-1 text-gray-600 dark:text-gray-300 hover:text-red-600 dark:hover:text-red-400 transition-colors"
                  title="Wyloguj"
                >
                  <LogOut className="w-4 h-4" />
                </button>
              </div>
            ) : (
              <div className="flex items-center space-x-2">
                <button
                  onClick={() => openAuthModal("login")}
                  className="text-sm text-gray-700 dark:text-gray-300 hover:text-primary-600 dark:hover:text-primary-400"
                >
                  Zaloguj
                </button>
                <span className="text-gray-300 dark:text-gray-600">|</span>
                <button
                  onClick={() => openAuthModal("register")}
                  className="text-sm text-gray-700 dark:text-gray-300 hover:text-primary-600 dark:hover:text-primary-400"
                >
                  Zarejestruj
                </button>
              </div>
            )}

            {/* Cart */}
            <Link
              to="/cart"
              className="relative p-2 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors"
            >
              <ShoppingCart className="w-6 h-6" />
              {cartItemsCount > 0 && (
                <span className="absolute -top-1 -right-1 bg-primary-600 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center">
                  {cartItemsCount}
                </span>
              )}
            </Link>

            {/* Mobile Menu */}
            <button className="md:hidden p-2 text-gray-600 dark:text-gray-300">
              <Menu className="w-6 h-6" />
            </button>
          </div>
        </div>
      </div>
    </header>
  );
}
