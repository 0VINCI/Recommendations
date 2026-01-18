import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { ArrowRight, Sparkles } from "lucide-react";
import { ProductCard } from "./ProductCard";
import { getRecommendationsForUser } from "../api/recommendationService";
import { useApp } from "../context/useApp";
import type { ProductDto } from "../types/product/ProductDto";

export function ForYouSection() {
  const { state } = useApp();
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const userId = state.user?.IdUser;

  useEffect(() => {
    if (!userId) {
      setProducts([]);
      return;
    }

    const fetchRecommendations = async () => {
      setLoading(true);
      setError(null);

      const result = await getRecommendationsForUser(userId, 8);

      if (result.status === 200 && result.data) {
        setProducts(result.data);
      } else if (result.status === 404) {
        // Cold start - no recommendations yet
        setProducts([]);
      } else {
        setError(result.message || "Błąd pobierania rekomendacji");
      }

      setLoading(false);
    };

    fetchRecommendations();
  }, [userId]);

  if (!userId) {
    return null;
  }


  if (!loading && products.length === 0) {
    return null;
  }

  return (
    <section className="py-20 bg-gradient-to-br from-brand-50 to-purple-50 dark:from-gray-800 dark:to-purple-900/20">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between mb-12">
          <div className="flex items-center space-x-3">
            <div className="p-2 bg-brand-100 dark:bg-brand-900 rounded-xl">
              <Sparkles className="w-6 h-6 text-brand-600 dark:text-brand-400" />
            </div>
            <div>
              <h2 className="text-3xl font-bold text-gray-900 dark:text-white">
                Dla Ciebie
              </h2>
              <p className="text-sm text-gray-500 dark:text-gray-400 mt-1">
                Spersonalizowane rekomendacje na podstawie Twojej aktywności
              </p>
            </div>
          </div>
          <Link
            to="/category/wszystkie?page=1&pageSize=20"
            className="text-brand-600 dark:text-brand-400 hover:text-brand-700 dark:hover:text-brand-300 font-semibold transition-colors flex items-center space-x-1"
          >
            <span>Zobacz więcej</span>
            <ArrowRight className="w-4 h-4" />
          </Link>
        </div>

        {loading && (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
            {[...Array(4)].map((_, i) => (
              <div
                key={i}
                className="bg-white dark:bg-gray-800 rounded-2xl h-80 animate-pulse"
              />
            ))}
          </div>
        )}

        {error && (
          <div className="text-center py-12">
            <p className="text-red-500 dark:text-red-400 text-lg">{error}</p>
          </div>
        )}

        {!loading && !error && products.length > 0 && (
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
            {products.slice(0, 4).map((product) => (
              <ProductCard key={product.id} product={product} />
            ))}
          </div>
        )}
      </div>
    </section>
  );
}

