import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { ArrowRight, Star } from "lucide-react";
import { ProductCard } from "./ProductCard";
import { getProducts } from "../api/productService";
import type { ProductDto } from "../types/product/ProductDto";

export function TopRatedSection() {
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProducts = async () => {
      setLoading(true);

      // Pobierz produkty i posortuj po ocenie, potem liczbuie ocen - od najwyzszych
      const result = await getProducts({ page: 1, pageSize: 100 });

      if (result.status === 200 && result.data?.products) {
        // Filtruj produkty z min. 5 recenzjami i sortuj po ocenie
        const topRated = result.data.products
          .filter((p) => p.reviews >= 5) // minimum recenzji dla wiarygodności
          .sort((a, b) => {
            if (b.rating !== a.rating) return b.rating - a.rating;
            return b.reviews - a.reviews;
          })
          .slice(0, 8);

        setProducts(topRated);
      }

      setLoading(false);
    };

    void fetchProducts();
  }, []);

  if (loading) {
    return (
      <section className="py-20 bg-white dark:bg-gray-800">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between mb-12">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-yellow-100 dark:bg-yellow-900 rounded-xl">
                <Star className="w-6 h-6 text-yellow-600 dark:text-yellow-400" />
              </div>
              <h2 className="text-3xl font-bold text-gray-900 dark:text-white">
                Najlepiej oceniane
              </h2>
            </div>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
            {[...Array(4)].map((_, i) => (
              <div
                key={i}
                className="bg-gray-100 dark:bg-gray-700 rounded-2xl h-80 animate-pulse"
              />
            ))}
          </div>
        </div>
      </section>
    );
  }

  if (products.length === 0) {
    return null;
  }

  return (
    <section className="py-20 bg-white dark:bg-gray-800">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between mb-12">
          <div className="flex items-center space-x-3">
            <div className="p-2 bg-yellow-100 dark:bg-yellow-900 rounded-xl">
              <Star className="w-6 h-6 text-yellow-600 dark:text-yellow-400 fill-current" />
            </div>
            <div>
              <h2 className="text-3xl font-bold text-gray-900 dark:text-white">
                Najlepiej oceniane
              </h2>
              <p className="text-sm text-gray-500 dark:text-gray-400 mt-1">
                Produkty z najwyższymi ocenami od użytkowników
              </p>
            </div>
          </div>
          <Link
            to="/category/wszystkie?page=1&pageSize=20"
            className="text-brand-600 dark:text-brand-400 hover:text-brand-700 dark:hover:text-brand-300 font-semibold transition-colors flex items-center space-x-1"
          >
            <span>Zobacz wszystkie</span>
            <ArrowRight className="w-4 h-4" />
          </Link>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
          {products.slice(0, 4).map((product) => (
            <ProductCard key={product.id} product={product} />
          ))}
        </div>
      </div>
    </section>
  );
}

