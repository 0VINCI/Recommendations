import { useEffect, useRef } from "react";
import { ChevronLeft, ChevronRight } from "lucide-react";
import { useRecommendations } from "../hooks/useRecommendations";
import { useApp } from "../context/useApp";
import { ProductCard } from "./ProductCard";
import { RecommendationSkeleton } from "./common/RecommendationSkeleton";
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

  const scrollContainerRef = useRef<HTMLDivElement>(null);

  const currentAlgorithm = RECOMMENDATION_ALGORITHMS.find(
    (alg) => alg.value === state.selectedRecommendationAlgorithm
  );

  // Funkcje do przewijania
  const scrollLeft = () => {
    if (scrollContainerRef.current) {
      const container = scrollContainerRef.current;
      const cardWidth = 320; // Szerokość karty + gap
      const scrollAmount = Math.min(cardWidth * 2, container.scrollLeft);
      container.scrollBy({ left: -scrollAmount, behavior: "smooth" });
    }
  };

  const scrollRight = () => {
    if (scrollContainerRef.current) {
      const container = scrollContainerRef.current;
      const cardWidth = 320; // Szerokość karty + gap
      const maxScroll = container.scrollWidth - container.clientWidth;
      const scrollAmount = Math.min(
        cardWidth * 2,
        maxScroll - container.scrollLeft
      );
      container.scrollBy({ left: scrollAmount, behavior: "smooth" });
    }
  };

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
    return <RecommendationSkeleton />;
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

      {/* Carousel Container */}
      <div className="relative">
        {/* Left Arrow */}
        <button
          onClick={scrollLeft}
          className="absolute left-0 top-1/2 -translate-y-1/2 z-10 bg-white dark:bg-gray-800 rounded-full p-2 shadow-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
          aria-label="Przewiń w lewo"
        >
          <ChevronLeft className="w-5 h-5 text-gray-600 dark:text-gray-400" />
        </button>

        {/* Right Arrow */}
        <button
          onClick={scrollRight}
          className="absolute right-0 top-1/2 -translate-y-1/2 z-10 bg-white dark:bg-gray-800 rounded-full p-2 shadow-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
          aria-label="Przewiń w prawo"
        >
          <ChevronRight className="w-5 h-5 text-gray-600 dark:text-gray-400" />
        </button>

        {/* Scrollable Products Container */}
        <div
          ref={scrollContainerRef}
          className="flex gap-6 overflow-x-auto scrollbar-hide pb-4"
          style={{
            scrollbarWidth: "none",
            msOverflowStyle: "none",
          }}
        >
          {similarProducts.map((product) => (
            <div key={product.id} className="flex-shrink-0 w-80">
              <ProductCard product={product} />
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
