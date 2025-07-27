import { Link } from "react-router-dom";
import { ArrowRight, Star, TrendingUp } from "lucide-react";
import { ProductCard } from "../components/ProductCard";
import { useProducts } from "../hooks/useProducts";
import { useEffect } from "react";
import { Loader } from "../components/common/Loader";

export function HomePage() {
  const {
    products,
    bestsellers,
    bestsellersLoading,
    bestsellersError,
    newProducts,
    newProductsLoading,
    newProductsError,
    getProducts,
    getBestsellers,
    getNewProducts,
  } = useProducts();

  useEffect(() => {
    getProducts(1, 20); // Dla featured products zawsze 20
    getBestsellers(1, 20); // Dla bestsellers zawsze 20
    getNewProducts(1, 20); // Dla new products zawsze 20
  }, []);

  const featuredProducts = products?.slice(0, 8) || [];

  if (bestsellersLoading || newProductsLoading) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center">
        <Loader />
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      {/* Hero Section */}
      <section
        className="relative bg-gradient-to-r from-primary-600 to-primary-400 flex flex-col items-center justify-center h-[388px]"
        style={{
          backgroundImage:
            "url('https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=1200&q=80')",
          backgroundSize: "cover",
          backgroundPosition: "center",
          backgroundRepeat: "no-repeat",
        }}
      >
        <div className="absolute inset-0 bg-blue-600/70 z-0" />{" "}
        {/* Opcjonalny overlay dla lepszej czytelności */}
        <div className="relative z-10 flex flex-col items-center justify-center h-full">
          <h1 className="text-6xl font-bold text-white mb-6 text-center drop-shadow-lg">
            Odkryj Swój Styl
          </h1>
          <p className="text-2xl text-blue-100 mb-8 text-center drop-shadow-lg">
            Najnowsze trendy w modzie damskiej i męskiej
          </p>
          <button className="bg-white/70 text-primary-600 px-8 py-3 rounded-xl font-semibold hover:bg-white transition">
            Przeglądaj Kolekcję
          </button>
        </div>
      </section>
      {/* Bestsellers Section */}
      <section className="py-16">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between mb-8">
            <div className="flex items-center space-x-2">
              <TrendingUp className="w-6 h-6 text-primary-600" />
              <h2 className="text-2xl font-bold text-gray-900 dark:text-white">
                Bestsellery
              </h2>
            </div>
            <Link
              to="/category/bestsellers?page=1&pageSize=20"
              className="text-primary-600 dark:text-primary-400 hover:underline font-medium"
            >
              Zobacz wszystkie
            </Link>
          </div>

          {bestsellersError && (
            <div className="text-center py-12">
              <p className="text-red-500 dark:text-red-400 text-lg">
                {bestsellersError}
              </p>
            </div>
          )}

          {!bestsellersLoading && !bestsellersError && (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
              {(bestsellers || []).slice(0, 4).map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>
          )}
        </div>
      </section>
      {/* New Products Section */}
      <section className="py-16 bg-white dark:bg-gray-800">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between mb-8">
            <div className="flex items-center space-x-2">
              <Star className="w-6 h-6 text-green-600" />
              <h2 className="text-2xl font-bold text-gray-900 dark:text-white">
                Nowości
              </h2>
            </div>
            <Link
              to="/category/new?page=1&pageSize=20"
              className="text-primary-600 dark:text-primary-400 hover:underline font-medium"
            >
              Zobacz wszystkie
            </Link>
          </div>

          {newProductsError && (
            <div className="text-center py-12">
              <p className="text-red-500 dark:text-red-400 text-lg">
                {newProductsError}
              </p>
            </div>
          )}

          {!newProductsLoading && !newProductsError && (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
              {(newProducts || []).slice(0, 4).map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>
          )}
        </div>
      </section>
      {/* Featured Products */}
      <section className="py-16">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-8 text-center">
            Polecane Produkty
          </h2>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
            {featuredProducts.map((product) => (
              <ProductCard key={product.id} product={product} />
            ))}
          </div>
        </div>
      </section>
      {/* Newsletter Section */}
      <section className="py-16 bg-primary-50 dark:bg-gray-800">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          <h2 className="text-3xl font-bold text-gray-900 dark:text-white mb-4">
            Bądź na bieżąco z nowościami
          </h2>
          <p className="text-lg text-gray-600 dark:text-gray-300 mb-8">
            Zapisz się do naszego newslettera i otrzymuj informacje o
            najnowszych produktach i promocjach
          </p>
          <div className="flex flex-col sm:flex-row gap-4 max-w-md mx-auto">
            <input
              type="email"
              placeholder="Twój adres email"
              className="flex-1 px-4 py-3 border border-gray-300 dark:border-gray-600 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
            />
            <button className="bg-primary-600 hover:bg-primary-700 text-white px-6 py-3 rounded-lg font-medium transition-colors">
              Zapisz się
            </button>
          </div>
        </div>
      </section>
    </div>
  );
}
