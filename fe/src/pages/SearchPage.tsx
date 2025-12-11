import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { searchProducts } from "../api/productService";
import { ProductCard } from "../components/ProductCard";
import { Loader } from "../components/common/Loader";
import { Search, X } from "lucide-react";
import { useTracking } from "../hooks/useTracking";
import { useApp } from "../context/useApp";

export function SearchPage() {
  const [searchParams] = useSearchParams();
  const query = searchParams.get("q") || "";
  const [products, setProducts] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const { state } = useApp();
  const { searchPerformed } = useTracking(state.user?.IdUser);

  useEffect(() => {
    const fetchSearchResults = async () => {
      if (!query.trim()) {
        setProducts([]);
        setLoading(false);
        return;
      }

      setLoading(true);
      try {
        const result = await searchProducts(query);

        if (result.status === 200 && result.data) {
          const productsArray = Array.isArray(result.data) ? result.data : [];
          setProducts(productsArray);

          // Track search event
          void searchPerformed(query, productsArray.length);
        } else {
          setProducts([]);
        }
      } catch (error) {
        console.error("Search error:", error);
        setProducts([]);
      } finally {
        setLoading(false);
      }
    };

    fetchSearchResults();
  }, [query, searchPerformed]);

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <Loader />
      </div>
    );
  }

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      {/* Search Header */}
      <div className="mb-8">
        <div className="flex items-center space-x-3 mb-4">
          <Search className="w-8 h-8 text-brand-500" />
          <h1 className="text-3xl font-bold text-gray-900 dark:text-white">
            Wyniki wyszukiwania
          </h1>
        </div>

        <div className="flex items-center space-x-4">
          <p className="text-lg text-gray-600 dark:text-gray-400">
            Szukano:{" "}
            <span className="font-semibold text-gray-900 dark:text-white">
              "{query}"
            </span>
          </p>
          <p className="text-sm text-gray-500 dark:text-gray-400">
            Znaleziono: <span className="font-semibold">{products.length}</span>{" "}
            {products.length === 1 ? "produkt" : "produktów"}
          </p>
        </div>
      </div>

      {/* Results */}
      {products.length === 0 ? (
        <div className="text-center py-16">
          <div className="w-20 h-20 mx-auto mb-4 bg-gray-100 dark:bg-gray-800 rounded-full flex items-center justify-center">
            <X className="w-10 h-10 text-gray-400" />
          </div>
          <h2 className="text-2xl font-semibold text-gray-900 dark:text-white mb-2">
            Brak wyników
          </h2>
          <p className="text-gray-600 dark:text-gray-400">
            Nie znaleziono produktów dla zapytania "{query}"
          </p>
          <p className="text-sm text-gray-500 dark:text-gray-500 mt-2">
            Spróbuj użyć innych słów kluczowych
          </p>
        </div>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
          {products.map((product, index) => (
            <ProductCard
              key={product.id}
              product={product}
              viewMode="grid"
              listId={`search:${query}`}
              position={index + 1}
            />
          ))}
        </div>
      )}
    </div>
  );
}
