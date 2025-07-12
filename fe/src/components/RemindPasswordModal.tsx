import React, { useState } from "react";
import { X } from "lucide-react";
import { remindPassword } from "../api/authorizationService";

interface RemindPasswordModalProps {
  isOpen: boolean;
  onClose: () => void;
}

export function RemindPasswordModal({
  isOpen,
  onClose,
}: RemindPasswordModalProps) {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);
  const [email, setEmail] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);

    if (!email || !email.includes("@")) {
      setError("Podaj poprawny adres email!");
      return;
    }

    setIsLoading(true);

    try {
      const result = await remindPassword(email);

      if (result.status === 200) {
        setSuccess(true);
        setEmail("");
        setTimeout(() => {
          onClose();
          setSuccess(false);
        }, 3000);
      } else {
        setError("Błąd podczas wysyłania przypomnienia hasła.");
      }
    } catch {
      setError("Wystąpił błąd podczas wysyłania przypomnienia hasła.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleClose = () => {
    setEmail("");
    setError(null);
    setSuccess(false);
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50 p-4">
      <div className="bg-white dark:bg-gray-800 rounded-lg max-w-md w-full p-6">
        <div className="flex items-center justify-between mb-6">
          <h2 className="text-xl font-bold text-gray-900 dark:text-white">
            Przypomnij hasło
          </h2>
          <button
            onClick={handleClose}
            className="text-gray-400 hover:text-gray-600 dark:hover:text-gray-300"
          >
            <X className="w-6 h-6" />
          </button>
        </div>

        {success && (
          <div className="mb-4 p-3 bg-green-100 dark:bg-green-900 text-green-700 dark:text-green-300 rounded-md">
            <p className="font-medium">Email wysłany!</p>
            <p className="text-sm mt-1">
              Jeśli podany adres email istnieje w naszej bazie, otrzymasz
              instrukcje do resetowania hasła.
            </p>
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
              Adres email
            </label>
            <input
              type="email"
              required
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="twoj@email.com"
              className="w-full px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
              disabled={isLoading}
            />
          </div>

          {error && (
            <div className="text-red-600 dark:text-red-400 text-sm">
              {error}
            </div>
          )}

          <button
            type="submit"
            disabled={isLoading}
            className="w-full bg-primary-600 hover:bg-primary-700 disabled:bg-gray-400 text-white py-2 px-4 rounded-md font-medium transition-colors"
          >
            {isLoading ? "Wysyłanie..." : "Wyślij przypomnienie"}
          </button>
        </form>

        <div className="mt-4 text-center">
          <p className="text-sm text-gray-600 dark:text-gray-400">
            Wprowadź swój adres email, a my wyślemy Ci instrukcje do resetowania
            hasła.
          </p>
        </div>
      </div>
    </div>
  );
}
