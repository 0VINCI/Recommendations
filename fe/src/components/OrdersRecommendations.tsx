import { useEffect, useState } from "react";
import { Sparkles } from "lucide-react";
import { ProductCard } from "./ProductCard";
import { getRecommendationsForUser } from "../api/recommendationService";
import { useApp } from "../context/useApp";
import { useTracking } from "../hooks/useTracking";
import type { ProductDto } from "../types/product/ProductDto";

export function OrdersRecommendations() {
  const { state } = useApp();
  const { recImpression, recClick } = useTracking(state.user?.IdUser);
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(true);

  const userId = state.user?.IdUser;

  useEffect(() => {
    if (!userId) {
      setProducts([]);
      setLoading(false);
      return;
    }

    const fetchRecommendations = async () => {
      setLoading(true);

      const result = await getRecommendationsForUser(userId, 4);

      if (result.status === 200 && result.data) {
        setProducts(result.data);
      } else {
        setProducts([]);
      }

      setLoading(false);
    };

    void fetchRecommendations();
  }, [userId]);

  useEffect(() => {
    if (!products || products.length === 0) return;

    products.forEach((p, idx) => {
      void recImpression(
        "orders_next_purchase",
        "CollaborativeFiltering",
        userId || "user",
        String(p.id),
        idx + 1
      );
    });
  }, [products, userId, recImpression]);

  const handleClick = (itemId: string, index: number) => {
    void recClick(
      "orders_next_purchase",
      "CollaborativeFiltering",
      userId || "user",
      itemId,
      index + 1
    );
  };

  if (loading) {
    return (
      <div className="bg-gradient-to-r from-brand-50 to-purple-50 dark:from-gray-800 dark:to-purple-900/20 rounded-lg p-6 mb-8">
        <div className="flex items-center gap-3 mb-6">
          <div className="p-2 bg-brand-100 dark:bg-brand-900 rounded-lg">
            <Sparkles className="w-5 h-5 text-brand-600 dark:text-brand-400" />
          </div>
          <h3 className="text-lg font-bold text-gray-900 dark:text-white">
            Do następnych zakupów
          </h3>
        </div>
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
          {[...Array(4)].map((_, i) => (
            <div
              key={i}
              className="bg-white dark:bg-gray-800 rounded-xl h-48 animate-pulse"
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
    <div className="bg-gradient-to-r from-brand-50 to-purple-50 dark:from-gray-800 dark:to-purple-900/20 rounded-lg p-6 mb-8">
      <div className="flex items-center gap-3 mb-6">
        <div className="p-2 bg-brand-100 dark:bg-brand-900 rounded-lg">
          <Sparkles className="w-5 h-5 text-brand-600 dark:text-brand-400" />
        </div>
        <div>
          <h3 className="text-lg font-bold text-gray-900 dark:text-white">
            Do następnych zakupów
          </h3>
          <p className="text-sm text-gray-500 dark:text-gray-400">
            Produkty dopasowane do Twoich preferencji
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

