import { useEffect } from "react";
import { useNavigate, Routes, Route } from "react-router-dom";
import { useApp } from "../context/useApp";
import { CartPage } from "../pages/CartPage";
import { CategoryPage } from "../pages/CategoryPage";
import { CheckoutPage } from "../pages/CheckoutPage";
import { HomePage } from "../pages/HomePage";
import { OrdersPage } from "../pages/OrdersPage";
import { ProductPage } from "../pages/ProductPage";
import { SearchPage } from "../pages/SearchPage";
import { AuthModal } from "./authorization/AuthModal";
import { ModalManager } from "./authorization/ModalManager";
import { Header } from "./common/Header";
import { ToastManager } from "./common/ToastManager";
import { Navigation } from "./common/Navigation";

function AppContent() {
  const navigate = useNavigate();
  const { state, userLoading } = useApp();

  useEffect(() => {
    if (userLoading) return;
    if (!state.user) {
      navigate("/");
    }
  }, [state.user]);

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <Header />
      <Navigation />
      <main className="relative">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/category/:category" element={<CategoryPage />} />
          <Route path="/product/:id" element={<ProductPage />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/cart" element={<CartPage />} />
          <Route path="/checkout" element={<CheckoutPage />} />
          <Route path="/orders" element={<OrdersPage />} />
        </Routes>
      </main>
      <AuthModal />
      <ModalManager />
      <ToastManager />
    </div>
  );
}

export default AppContent;
