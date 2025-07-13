import React from "react";
import { useApp } from "../context/useApp";
import { ToastContainer } from "./ToastContainer";

export function ToastManager() {
  const { state, dispatch } = useApp();

  const handleCloseToast = (id: string) => {
    dispatch({ type: "REMOVE_TOAST", payload: id });
  };

  return <ToastContainer toasts={state.toasts} onClose={handleCloseToast} />;
}
