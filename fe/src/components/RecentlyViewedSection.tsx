import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { ArrowRight, Clock } from "lucide-react";
import { ProductCard } from "./ProductCard";
import { getRecentlyViewedProducts } from "../api/recommendationService";
import { useApp } from "../context/useApp";
import type { ProductDto } from "../types/product/ProductDto";

export function RecentlyViewedSection() {
  const { state } = useApp();
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(false);

  const userId = state.user?.IdUser;

  useEffect(() => {
    if (!userId) {
      setProducts([]);
      return;
    }

    const fetchProducts = async () => {
      setLoading(true);

      const result = await getRecentlyViewedProducts(userId, 8);

      if (result.status === 200 && result.data) {
        setProducts(result.data);
      } else {
        setProducts([]);
      }

      setLoading(false);
    };

    void fetchProducts();
  }, [userId]);

  // Nie pokazuj sekcji dla niezalogowanych lub gdy brak produktów
  if (!userId) {
    return null;
  }

  if (!loading && products.length === 0) {
    return null;
  }

  return (
    <section className="py-20 bg-white dark:bg-gray-800">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between mb-12">
          <div className="flex items-center space-x-3">
            <div className="p-2 bg-gray-100 dark:bg-gray-700 rounded-xl">
              <Clock className="w-6 h-6 text-gray-600 dark:text-gray-400" />
            </div>
            <div>
              <h2 className="text-3xl font-bold text-gray-900 dark:text-white">
                Ostatnio oglądane
              </h2>
              <p className="text-sm text-gray-500 dark:text-gray-400 mt-1">
                Produkty, które ostatnio przeglądałeś
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
                className="bg-gray-100 dark:bg-gray-700 rounded-2xl h-80 animate-pulse"
              />
            ))}
          </div>
        )}

        {!loading && products.length > 0 && (
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

