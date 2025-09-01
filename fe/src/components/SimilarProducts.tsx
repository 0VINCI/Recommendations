import { useEffect } from "react";
import { Link } from "react-router-dom";
import { useRecommendations } from "../hooks/useRecommendations";
import { useApp } from "../context/useApp";
import { ProductCard } from "./ProductCard";
import { Loader } from "./common/Loader";
import { RECOMMENDATION_ALGORITHMS } from "../types/recommendation/RecommendationAlgorithm";

interface SimilarProductsProps {
  productId: string;
  currentProductName: string;
}

export function SimilarProducts({
  productId,
  currentProductName,
}: SimilarProductsProps) {
  const { state } = useApp();
  const {
    similarProducts,
    loading,
    error,
    getSimilarProducts,
    clearSimilarProducts,
  } = useRecommendations();

  const currentAlgorithm = RECOMMENDATION_ALGORITHMS.find(
    (alg) => alg.value === state.selectedRecommendationAlgorithm
  );

  useEffect(() => {
    if (productId && state.selectedRecommendationAlgorithm) {
      getSimilarProducts(productId, state.selectedRecommendationAlgorithm, 8);
    }

    return () => {
      clearSimilarProducts();
    };
  }, [
    productId,
    state.selectedRecommendationAlgorithm,
    getSimilarProducts,
    clearSimilarProducts,
  ]);

  if (loading) {
    return (
      <div className="py-8">
        <div className="flex items-center justify-center">
          <Loader />
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="py-8">
        <div className="text-center">
          <p className="text-gray-500 dark:text-gray-400">
            Nie udało się załadować podobnych produktów: {error}
          </p>
        </div>
      </div>
    );
  }

  if (!similarProducts || similarProducts.length === 0) {
    return (
      <div className="py-8">
        <div className="text-center">
          <p className="text-gray-500 dark:text-gray-400">
            Brak podobnych produktów dla wybranego algorytmu
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="py-8">
      <div className="mb-6">
        <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-2">
          Podobne produkty
        </h2>
        <p className="text-gray-600 dark:text-gray-400">
          Rekomendacje na podstawie algorytmu:{" "}
          <span className="font-medium">{currentAlgorithm?.label}</span>
        </p>
        <p className="text-sm text-gray-500 dark:text-gray-500 mt-1">
          Podobne do: {currentProductName}
        </p>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        {similarProducts.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </div>

      {similarProducts.length > 0 && (
        <div className="mt-8 text-center">
          <Link
            to="/"
            className="inline-flex items-center px-4 py-2 border border-transparent text-sm font-medium rounded-md text-primary-700 bg-primary-100 hover:bg-primary-200 dark:text-primary-300 dark:bg-primary-900 dark:hover:bg-primary-800 transition-colors"
          >
            Zobacz więcej produktów
          </Link>
        </div>
      )}
    </div>
  );
}
