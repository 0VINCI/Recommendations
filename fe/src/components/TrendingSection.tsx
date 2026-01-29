import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { ArrowRight, TrendingUp } from "lucide-react";
import { ProductCard } from "./ProductCard";
import { getTrendingProducts } from "../api/productService";
import { useApp } from "../context/useApp";
import type { ProductDto } from "../types/product/ProductDto";

export function TrendingSection() {
  const { state } = useApp();
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Dla zalogowanych użytkowników pokazujemy "Dla Ciebie", nie trending
  const userId = state.user?.IdUser;

  useEffect(() => {
    // Jeśli user jest zalogowany, nie pokazuj tej sekcji (pokazuje się Dla CIebie)
    if (userId) {
      setLoading(false);
      setProducts([]);
      return;
    }

    const fetchProducts = async () => {
      setLoading(true);
      setError(null);

      const result = await getTrendingProducts({ page: 1, pageSize: 8 });

      if (result.status === 200 && result.data) {
        setProducts(result.data.products || []);
      } else {
        setError(result.message || "Błąd pobierania produktów");
        setProducts([]);
      }

      setLoading(false);
    };

    void fetchProducts();
  }, [userId]);

  if (userId) {
    return null;
  }

  if (!loading && products.length === 0) {
    return null;
  }

  return (
    <section className="py-20 bg-gradient-to-br from-orange-50 to-red-50 dark:from-gray-800 dark:to-orange-900/20">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between mb-12">
          <div className="flex items-center space-x-3">
            <div className="p-2 bg-orange-100 dark:bg-orange-900 rounded-xl">
              <TrendingUp className="w-6 h-6 text-orange-600 dark:text-orange-400" />
            </div>
            <div>
              <h2 className="text-3xl font-bold text-gray-900 dark:text-white">
                Popularne teraz
              </h2>
              <p className="text-sm text-gray-500 dark:text-gray-400 mt-1">
                Produkty cieszące się największym zainteresowaniem
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

