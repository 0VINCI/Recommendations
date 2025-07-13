import React, { useState } from "react";
import { X } from "lucide-react";
import { useApp } from "../context/useApp";
import { signIn, signUp } from "../api/authorizationService.tsx";
import type { User } from "../types/authorization/User.tsx";
import { useToast } from "../hooks/useToast";

export function AuthModal() {
  const { state, dispatch } = useApp();
  const [authError, setAuthError] = useState<string | null>(null);
  const { showSuccess, showError } = useToast();

  const [formData, setFormData] = useState({
    email: "",
    password: "",
    name: "",
    surname: "",
    confirmPassword: "",
  });

  const closeModal = () => {
    dispatch({ type: "CLOSE_AUTH_MODAL" });
    setFormData({
      email: "",
      password: "",
      name: "",
      surname: "",
      confirmPassword: "",
    });
    setAuthError(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (state.authMode === "register") {
      if (formData.password !== formData.confirmPassword) {
        showError("Hasła nie są identyczne!");
        return;
      }

      const result = await signUp({
        Name: formData.name,
        Surname: formData.surname,
        Email: formData.email,
        Password: formData.password,
      });

      if (result.status === 200) {
        showSuccess("Pomyślnie zarejestrowano, zaloguj się.");
        closeModal();
      } else {
        setAuthError("Błąd rejestracji.");
      }
    } else {
      // LOGIN
      const result = await signIn({
        Email: formData.email,
        Password: formData.password,
      });

      if (result.status === 200 && result.data) {
        console.log("Login successful, result.data:", result.data); // Debug
        // Zapisz dane użytkownika w kontekście
        const userData: User = {
          IdUser: result.data.idUser,
          Name: result.data.name,
          Surname: result.data.surname,
          Email: result.data.email,
        };
        console.log("User data to save:", userData); // Debug
        dispatch({ type: "SET_USER", payload: userData });
        closeModal();
        showSuccess("Zalogowano pomyślnie!");
      } else {
        setAuthError("Błąd logowania.");
      }
    }
  };

  const switchMode = () => {
    dispatch({
      type: "TOGGLE_AUTH_MODAL",
      payload: state.authMode === "login" ? "register" : "login",
    });
  };

  if (!state.isAuthModalOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white dark:bg-gray-800 rounded-lg max-w-md w-full p-6">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-xl font-bold text-gray-900 dark:text-white">
            {state.authMode === "login" ? "Zaloguj się" : "Zarejestruj się"}
          </h2>
          <button
            onClick={closeModal}
            className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300"
          >
            <X className="w-6 h-6" />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          {state.authMode === "register" && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Imię
              </label>
              <input
                type="text"
                required
                value={formData.name}
                onChange={(e) =>
                  setFormData({ ...formData, name: e.target.value })
                }
                className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
              />
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Nazwisko
              </label>
              <input
                type="text"
                required
                value={formData.surname}
                onChange={(e) =>
                  setFormData({ ...formData, surname: e.target.value })
                }
                className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
              />
            </div>
          )}

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Email
            </label>
            <input
              type="email"
              required
              value={formData.email}
              onChange={(e) =>
                setFormData({ ...formData, email: e.target.value })
              }
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Hasło
            </label>
            <input
              type="password"
              required
              value={formData.password}
              onChange={(e) =>
                setFormData({ ...formData, password: e.target.value })
              }
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
            />
            {state.authMode === "login" && (
              <button
                type="button"
                onClick={() => {
                  dispatch({ type: "CLOSE_AUTH_MODAL" });
                  dispatch({ type: "OPEN_REMIND_PASSWORD_MODAL" });
                }}
                className="text-sm text-primary-600 dark:text-primary-400 hover:underline mt-1"
              >
                Zapomniałeś hasła?
              </button>
            )}
          </div>

          {state.authMode === "register" && (
            <div>
              <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Potwierdź hasło
              </label>
              <input
                type="password"
                required
                value={formData.confirmPassword}
                onChange={(e) =>
                  setFormData({ ...formData, confirmPassword: e.target.value })
                }
                className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
              />
            </div>
          )}
          {authError && (
            <div className="text-red-600 dark:text-red-400 text-sm">
              {authError}
            </div>
          )}

          <button
            type="submit"
            className="w-full bg-primary-600 hover:bg-primary-700 text-white py-2 px-4 rounded-md font-medium transition-colors"
          >
            {state.authMode === "login" ? "Zaloguj się" : "Zarejestruj się"}
          </button>
        </form>

        <div className="mt-4 text-center">
          <button
            onClick={switchMode}
            className="text-sm text-primary-600 dark:text-primary-400 hover:underline"
          >
            {state.authMode === "login"
              ? "Nie masz konta? Zarejestruj się"
              : "Masz już konto? Zaloguj się"}
          </button>
        </div>
      </div>
    </div>
  );
}
