import { useEffect, useState } from "react";
import { ShoppingBag } from "lucide-react";
import { ProductCard } from "./ProductCard";
import { getCartRecommendations } from "../api/recommendationService";
import { useApp } from "../context/useApp";
import { useTracking } from "../hooks/useTracking";
import type { ProductDto } from "../types/product/ProductDto";

export function CartRecommendations() {
  const { state } = useApp();
  const { recImpression, recClick } = useTracking(state.user?.IdUser);
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(false);

  const cartProductIds = state.cart.map((item) => String(item.product.id));

  useEffect(() => {
    if (cartProductIds.length === 0) {
      setProducts([]);
      return;
    }

    const fetchRecommendations = async () => {
      setLoading(true);

      const result = await getCartRecommendations(cartProductIds, 4);

      if (result.status === 200 && result.data) {
        setProducts(result.data);
      } else {
        setProducts([]);
      }

      setLoading(false);
    };

    void fetchRecommendations();
  }, [cartProductIds.join(",")]); // Reaguj na zmiany w koszyku

  useEffect(() => {
    if (!products || products.length === 0) return;

    products.forEach((p, idx) => {
      void recImpression(
        "cart_cross_sell",
        "CollaborativeFiltering",
        cartProductIds[0] || "cart",
        String(p.id),
        idx + 1
      );
    });
  }, [products, cartProductIds, recImpression]);

  const handleClick = (itemId: string, index: number) => {
    void recClick(
      "cart_cross_sell",
      "CollaborativeFiltering",
      cartProductIds[0] || "cart",
      itemId,
      index + 1
    );
  };

  if (loading) {
    return (
      <div className="mt-8 bg-white dark:bg-gray-800 rounded-lg shadow-sm p-6">
        <div className="flex items-center gap-3 mb-6">
          <div className="p-2 bg-blue-100 dark:bg-blue-900 rounded-lg">
            <ShoppingBag className="w-5 h-5 text-blue-600 dark:text-blue-400" />
          </div>
          <h3 className="text-lg font-bold text-gray-900 dark:text-white">
            Klienci kupili również
          </h3>
        </div>
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
          {[...Array(4)].map((_, i) => (
            <div
              key={i}
              className="bg-gray-100 dark:bg-gray-700 rounded-xl h-48 animate-pulse"
            />
          ))}
        </div>
      </div>
    );
  }

  if (products.length === 0) {
    return null;
  }

  return (
    <div className="mt-8 bg-white dark:bg-gray-800 rounded-lg shadow-sm p-6">
      <div className="flex items-center gap-3 mb-6">
        <div className="p-2 bg-blue-100 dark:bg-blue-900 rounded-lg">
          <ShoppingBag className="w-5 h-5 text-blue-600 dark:text-blue-400" />
        </div>
        <div>
          <h3 className="text-lg font-bold text-gray-900 dark:text-white">
            Klienci kupili również
          </h3>
          <p className="text-sm text-gray-500 dark:text-gray-400">
            Produkty często kupowane razem z artykułami z Twojego koszyka
          </p>
        </div>
      </div>

      <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
        {products.map((product, index) => (
          <ProductCard
            key={product.id}
            product={product}
            onCardClick={() => handleClick(String(product.id), index)}
          />
        ))}
      </div>
    </div>
  );
}

