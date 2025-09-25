import { Link, useNavigate } from "react-router-dom";
import {
  ShoppingCart,
  Sun,
  Moon,
  Search,
  Menu,
  LogOut,
  Settings,
  ChevronDown,
  ClipboardList,
} from "lucide-react";
import { useApp } from "../../context/useApp";
import { signOut } from "../../api/authorizationService";
import { useState, useEffect, useRef } from "react";
import { RecommendationAlgorithmSelector } from "./RecommendationAlgorithmSelector";

export function Header() {
  const { state, dispatch } = useApp();
  const [isUserMenuOpen, setIsUserMenuOpen] = useState(false);
  const userMenuRef = useRef<HTMLDivElement>(null);
  const navigate = useNavigate();

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        userMenuRef.current &&
        !userMenuRef.current.contains(event.target as Node)
      ) {
        setIsUserMenuOpen(false);
      }
    }

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

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

  const openChangePasswordModal = () => {
    setIsUserMenuOpen(false);
    dispatch({ type: "OPEN_CHANGE_PASSWORD_MODAL" });
  };

  const handleGoToOrders = () => {
    setIsUserMenuOpen(false);
    navigate("/orders");
  };

  const cartItemsCount = state.cart.reduce(
    (sum, item) => sum + item.quantity,
    0
  );

  return (
    <header className="bg-white dark:bg-gray-900 shadow-soft border-b border-gray-100 dark:border-gray-800 sticky top-0 z-50 backdrop-blur-sm bg-white/95 dark:bg-gray-900/95">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-20">
          {/* Logo */}
          <Link to="/" className="flex items-center space-x-3 group">
            <div className="w-10 h-10 bg-gradient-to-br from-brand-500 to-brand-700 rounded-xl flex items-center justify-center shadow-medium group-hover:shadow-strong transition-all duration-300 group-hover:scale-105">
              <span className="text-white font-bold text-xl">S</span>
            </div>
            <span className="text-2xl font-bold text-gray-900 dark:text-white tracking-tight">
              StyleHub
            </span>
          </Link>

          {/* Search Bar */}
          <div className="hidden md:flex flex-1 max-w-2xl mx-8">
            <div className="relative w-full group">
              <Search className="absolute left-4 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5 group-focus-within:text-brand-500 transition-colors" />
              <input
                type="text"
                placeholder="Szukaj produktów, marek, kategorii..."
                className="w-full pl-12 pr-4 py-3 border border-gray-200 dark:border-gray-700 rounded-2xl focus:ring-2 focus:ring-brand-500 focus:border-transparent bg-gray-50 dark:bg-gray-800 text-gray-900 dark:text-white placeholder-gray-400 dark:placeholder-gray-500 transition-all duration-200 hover:bg-white dark:hover:bg-gray-700"
              />
            </div>
          </div>

          {/* Actions */}
          <div className="flex items-center space-x-2">
            {/* Recommendation Algorithm Selector */}
            <RecommendationAlgorithmSelector />

            {/* Theme Toggle */}
            <button
              onClick={toggleTheme}
              className="p-3 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white hover:bg-gray-100 dark:hover:bg-gray-800 rounded-xl transition-all duration-200"
            >
              {state.theme === "light" ? (
                <Moon className="w-5 h-5" />
              ) : (
                <Sun className="w-5 h-5" />
              )}
            </button>

            {/* User Menu */}
            {state.user ? (
              <div className="relative" ref={userMenuRef}>
                <button
                  onClick={() => setIsUserMenuOpen(!isUserMenuOpen)}
                  className="flex items-center space-x-3 p-2 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white hover:bg-gray-100 dark:hover:bg-gray-800 transition-all duration-200 rounded-xl"
                >
                  <img
                    src={`https://ui-avatars.com/api/?name=${state.user.Name} ${state.user.Surname}&background=0ea5e9&color=fff`}
                    alt={`${state.user.Name} ${state.user.Surname}`}
                    className="w-9 h-9 rounded-full ring-2 ring-gray-200 dark:ring-gray-700"
                  />
                  <span className="hidden md:block text-sm font-medium">
                    {state.user.Name} {state.user.Surname}
                  </span>
                  <ChevronDown className="w-4 h-4" />
                </button>

                {isUserMenuOpen && (
                  <div className="absolute right-0 mt-3 w-56 bg-white dark:bg-gray-800 rounded-2xl shadow-strong py-2 z-50 border border-gray-100 dark:border-gray-700 animate-scale-in">
                    <button
                      onClick={handleGoToOrders}
                      className="flex items-center w-full px-4 py-3 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
                    >
                      <ClipboardList className="w-4 h-4 mr-3" />
                      Moje zamówienia
                    </button>
                    <button
                      onClick={openChangePasswordModal}
                      className="flex items-center w-full px-4 py-3 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
                    >
                      <Settings className="w-4 h-4 mr-3" />
                      Zmień hasło
                    </button>
                    <div className="border-t border-gray-100 dark:border-gray-700 my-1"></div>
                    <button
                      onClick={handleSignOut}
                      className="flex items-center w-full px-4 py-3 text-sm text-accent-600 dark:text-accent-400 hover:bg-accent-50 dark:hover:bg-accent-900/20 transition-colors"
                    >
                      <LogOut className="w-4 h-4 mr-3" />
                      Wyloguj
                    </button>
                  </div>
                )}
              </div>
            ) : (
              <div className="flex items-center space-x-1">
                <button
                  onClick={() => openAuthModal("login")}
                  className="px-4 py-2 text-sm font-medium text-gray-700 dark:text-gray-300 hover:text-brand-600 dark:hover:text-brand-400 hover:bg-gray-100 dark:hover:bg-gray-800 rounded-xl transition-all duration-200"
                >
                  Zaloguj
                </button>
                <button
                  onClick={() => openAuthModal("register")}
                  className="px-4 py-2 text-sm font-medium text-white bg-brand-600 hover:bg-brand-700 rounded-xl transition-all duration-200 shadow-medium hover:shadow-strong"
                >
                  Zarejestruj
                </button>
              </div>
            )}

            {/* Cart */}
            <Link
              to="/cart"
              className="relative p-3 text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white hover:bg-gray-100 dark:hover:bg-gray-800 transition-all duration-200 rounded-xl"
            >
              <ShoppingCart className="w-6 h-6" />
              {cartItemsCount > 0 && (
                <span className="absolute -top-1 -right-1 bg-accent-500 text-white text-xs rounded-full w-6 h-6 flex items-center justify-center font-semibold shadow-medium">
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
