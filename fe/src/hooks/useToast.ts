import { useCallback } from "react";
import { useApp } from "../context/useApp";
import type { ToastType } from "../components/Toast";

export function useToast() {
  const { dispatch } = useApp();

  const showToast = useCallback(
    (type: ToastType, message: string, duration?: number) => {
      const id = Math.random().toString(36).substr(2, 9);
      dispatch({
        type: "ADD_TOAST",
        payload: {
          id,
          type,
          message,
          duration,
        },
      });
    },
    [dispatch]
  );

  const showSuccess = useCallback(
    (message: string, duration?: number) => {
      showToast("success", message, duration);
    },
    [showToast]
  );

  const showError = useCallback(
    (message: string, duration?: number) => {
      showToast("error", message, duration);
    },
    [showToast]
  );

  const showInfo = useCallback(
    (message: string, duration?: number) => {
      showToast("info", message, duration);
    },
    [showToast]
  );

  return {
    showToast,
    showSuccess,
    showError,
    showInfo,
  };
}
