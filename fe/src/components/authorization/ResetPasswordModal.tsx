import React, { useState } from "react";
import { X } from "lucide-react";
import { changePassword } from "../../api/authorizationService";
import { useToast } from "../../hooks/useToast";

interface ResetPasswordModalProps {
  isOpen: boolean;
  onClose: () => void;
  email: string;
}

export function ResetPasswordModal({
  isOpen,
  onClose,
  email,
}: ResetPasswordModalProps) {
  const [isLoading, setIsLoading] = useState(false);
  const [formData, setFormData] = useState({
    verificationCode: "",
    newPassword: "",
    confirmNewPassword: "",
  });
  const { showSuccess, showError } = useToast();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (formData.newPassword !== formData.confirmNewPassword) {
      showError("Nowe hasła nie są identyczne!");
      return;
    }

    if (formData.newPassword.length < 6) {
      showError("Nowe hasło musi mieć co najmniej 6 znaków!");
      return;
    }

    if (!formData.verificationCode) {
      showError("Wprowadź kod weryfikacyjny!");
      return;
    }

    setIsLoading(true);

    try {
      // Używamy changePassword endpoint z kodem jako starym hasłem
      const result = await changePassword({
        Email: email,
        OldPassword: formData.verificationCode, // Kod weryfikacyjny jako "stare" hasło
        NewPassword: formData.newPassword,
      });

      if (result.status === 200) {
        showSuccess(
          "Hasło zostało zmienione pomyślnie! Możesz się teraz zalogować."
        );
        setFormData({
          verificationCode: "",
          newPassword: "",
          confirmNewPassword: "",
        });
        setTimeout(() => {
          onClose();
        }, 3000);
      } else {
        showError(
          "Nieprawidłowy kod weryfikacyjny lub błąd podczas zmiany hasła."
        );
      }
    } catch {
      showError("Wystąpił błąd podczas resetowania hasła.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleClose = () => {
    setFormData({
      verificationCode: "",
      newPassword: "",
      confirmNewPassword: "",
    });
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white dark:bg-gray-800 rounded-lg max-w-md w-full p-6">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-xl font-bold text-gray-900 dark:text-white">
            Resetuj hasło
          </h2>
          <button
            onClick={handleClose}
            className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300"
          >
            <X className="w-6 h-6" />
          </button>
        </div>

        <div className="mb-4 p-3 bg-blue-100 dark:bg-blue-900 text-blue-700 dark:text-blue-300 rounded-md">
          <p className="text-sm">
            Kod weryfikacyjny został wysłany na adres: <strong>{email}</strong>
          </p>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Kod weryfikacyjny
            </label>
            <input
              type="text"
              required
              value={formData.verificationCode}
              onChange={(e) =>
                setFormData({ ...formData, verificationCode: e.target.value })
              }
              placeholder="Wprowadź kod z emaila"
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
              disabled={isLoading}
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Nowe hasło
            </label>
            <input
              type="password"
              required
              value={formData.newPassword}
              onChange={(e) =>
                setFormData({ ...formData, newPassword: e.target.value })
              }
              placeholder="Minimum 6 znaków"
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
              disabled={isLoading}
            />
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Potwierdź nowe hasło
            </label>
            <input
              type="password"
              required
              value={formData.confirmNewPassword}
              onChange={(e) =>
                setFormData({ ...formData, confirmNewPassword: e.target.value })
              }
              placeholder="Powtórz nowe hasło"
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
              disabled={isLoading}
            />
          </div>

          <button
            type="submit"
            disabled={isLoading}
            className="w-full bg-primary-600 hover:bg-primary-700 disabled:bg-gray-400 text-white py-2 px-4 rounded-md font-medium transition-colors"
          >
            {isLoading ? "Resetowanie hasła..." : "Resetuj hasło"}
          </button>
        </form>

        <div className="mt-4 text-center">
          <p className="text-sm text-gray-600 dark:text-gray-400">
            Wprowadź kod weryfikacyjny z emaila i ustaw nowe hasło.
          </p>
        </div>
      </div>
    </div>
  );
}
