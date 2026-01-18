import { Link } from "react-router-dom";
import { ArrowRight, Star, TrendingUp } from "lucide-react";
import { ProductCard } from "../components/ProductCard";
import { ForYouSection } from "../components/ForYouSection";
import { useProducts } from "../hooks/useProducts";
import { useEffect } from "react";
import { Loader } from "../components/common/Loader";
import { RotatingBanners } from "../components/common/RotatingBanners";

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
    getProducts(1, 20);
    getBestsellers(1, 20);
    getNewProducts(1, 20);
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
      {/* Rotating Banners */}
      <RotatingBanners />

      {/* Personalized Recommendations (CF user-to-item) - only for logged in users */}
      <ForYouSection />

      {/* Bestsellers Section */}
      <section className="py-20 bg-white dark:bg-gray-800">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between mb-12">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-accent-100 dark:bg-accent-900 rounded-xl">
                <TrendingUp className="w-6 h-6 text-accent-600 dark:text-accent-400" />
              </div>
              <h2 className="text-3xl font-bold text-gray-900 dark:text-white">
                Bestsellery
              </h2>
            </div>
            <Link
              to="/category/bestsellers?page=1&pageSize=20"
              className="text-brand-600 dark:text-brand-400 hover:text-brand-700 dark:hover:text-brand-300 font-semibold transition-colors flex items-center space-x-1"
            >
              <span>Zobacz wszystkie</span>
              <ArrowRight className="w-4 h-4" />
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
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
              {(bestsellers || []).slice(0, 4).map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>
          )}
        </div>
      </section>
      {/* New Products Section */}
      <section className="py-20 bg-gray-50 dark:bg-gray-900">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex items-center justify-between mb-12">
            <div className="flex items-center space-x-3">
              <div className="p-2 bg-green-100 dark:bg-green-900 rounded-xl">
                <Star className="w-6 h-6 text-green-600 dark:text-green-400" />
              </div>
              <h2 className="text-3xl font-bold text-gray-900 dark:text-white">
                Nowości
              </h2>
            </div>
            <Link
              to="/category/new?page=1&pageSize=20"
              className="text-brand-600 dark:text-brand-400 hover:text-brand-700 dark:hover:text-brand-300 font-semibold transition-colors flex items-center space-x-1"
            >
              <span>Zobacz wszystkie</span>
              <ArrowRight className="w-4 h-4" />
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
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
              {(newProducts || []).slice(0, 4).map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>
          )}
        </div>
      </section>
      {/* Featured Products */}
      <section className="py-20 bg-white dark:bg-gray-800">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-12">
            <h2 className="text-3xl font-bold text-gray-900 dark:text-white mb-4">
              Polecane Produkty
            </h2>
            <p className="text-lg text-gray-600 dark:text-gray-400 max-w-2xl mx-auto">
              Wyselekcjonowane przez nas produkty, które pokochasz
            </p>
          </div>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
            {featuredProducts.map((product) => (
              <ProductCard key={product.id} product={product} />
            ))}
          </div>
        </div>
      </section>
      {/* Special Offers */}
      <section className="py-20 bg-gray-50 dark:bg-gray-900">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="text-center mb-12">
            <h2 className="text-3xl font-bold text-gray-900 dark:text-white mb-4">
              Oferty Specjalne
            </h2>
            <p className="text-lg text-gray-600 dark:text-gray-400">
              Nie przegap okazji - ograniczone czasowo promocje
            </p>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {/* Free Shipping */}
            <div className="bg-white dark:bg-gray-800 rounded-2xl p-8 shadow-soft hover:shadow-strong transition-all duration-300 text-center">
              <div className="w-16 h-16 bg-green-100 dark:bg-green-900 rounded-2xl flex items-center justify-center mx-auto mb-4">
                <span className="text-2xl">🚚</span>
              </div>
              <h3 className="text-xl font-bold text-gray-900 dark:text-white mb-2">
                Darmowa Dostawa
              </h3>
              <p className="text-gray-600 dark:text-gray-400 mb-4">
                Przy zamówieniach powyżej 200 zł
              </p>
              <Link
                to="/category/wszystkie?page=1&pageSize=20"
                className="inline-flex items-center text-brand-600 dark:text-brand-400 hover:text-brand-700 dark:hover:text-brand-300 font-semibold"
              >
                Kup teraz
                <ArrowRight className="ml-1 w-4 h-4" />
              </Link>
            </div>

            {/* New Customer Discount */}
            <div className="bg-white dark:bg-gray-800 rounded-2xl p-8 shadow-soft hover:shadow-strong transition-all duration-300 text-center">
              <div className="w-16 h-16 bg-orange-100 dark:bg-orange-900 rounded-2xl flex items-center justify-center mx-auto mb-4">
                <span className="text-2xl">🎁</span>
              </div>
              <h3 className="text-xl font-bold text-gray-900 dark:text-white mb-2">
                -15% dla Nowych Klientów
              </h3>
              <p className="text-gray-600 dark:text-gray-400 mb-4">
                Kod: WITAJ15 przy pierwszym zamówieniu
              </p>
              <Link
                to="/category/wszystkie?page=1&pageSize=20"
                className="inline-flex items-center text-brand-600 dark:text-brand-400 hover:text-brand-700 dark:hover:text-brand-300 font-semibold"
              >
                Skorzystaj z kodu
                <ArrowRight className="ml-1 w-4 h-4" />
              </Link>
            </div>

            {/* Return Policy */}
            <div className="bg-white dark:bg-gray-800 rounded-2xl p-8 shadow-soft hover:shadow-strong transition-all duration-300 text-center">
              <div className="w-16 h-16 bg-blue-100 dark:bg-blue-900 rounded-2xl flex items-center justify-center mx-auto mb-4">
                <span className="text-2xl">↩️</span>
              </div>
              <h3 className="text-xl font-bold text-gray-900 dark:text-white mb-2">
                30 Dni na Zwrot
              </h3>
              <p className="text-gray-600 dark:text-gray-400 mb-4">
                Bezpieczne zakupy z możliwością zwrotu
              </p>
              <Link
                to="/category/wszystkie?page=1&pageSize=20"
                className="inline-flex items-center text-brand-600 dark:text-brand-400 hover:text-brand-700 dark:hover:text-brand-300 font-semibold"
              >
                Dowiedz się więcej
                <ArrowRight className="ml-1 w-4 h-4" />
              </Link>
            </div>
          </div>
        </div>
      </section>

      {/* Newsletter Section */}
      <section className="py-20 bg-gradient-to-br from-brand-50 to-brand-100 dark:from-gray-800 dark:to-gray-900">
        <div className="max-w-4xl mx-auto px-4 sm:px-6 lg:px-8 text-center">
          <h2 className="text-4xl font-bold text-gray-900 dark:text-white mb-4">
            Bądź na bieżąco z nowościami
          </h2>
          <p className="text-xl text-gray-600 dark:text-gray-300 mb-10 max-w-2xl mx-auto">
            Zapisz się do naszego newslettera i otrzymuj informacje o
            najnowszych produktach i ekskluzywnych promocjach
          </p>
          <div className="flex flex-col sm:flex-row gap-4 max-w-lg mx-auto">
            <input
              type="email"
              placeholder="Twój adres email"
              className="flex-1 px-6 py-4 border border-gray-200 dark:border-gray-600 rounded-2xl focus:ring-2 focus:ring-brand-500 focus:border-transparent bg-white dark:bg-gray-700 text-gray-900 dark:text-white placeholder-gray-400 dark:placeholder-gray-500 transition-all duration-200"
            />
            <button className="bg-brand-600 hover:bg-brand-700 text-white px-8 py-4 rounded-2xl font-semibold transition-all duration-200 shadow-medium hover:shadow-strong hover:scale-105">
              Zapisz się
            </button>
          </div>
        </div>
      </section>
    </div>
  );
}
