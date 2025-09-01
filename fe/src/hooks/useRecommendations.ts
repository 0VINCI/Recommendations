import { useState } from "react";
import { getSimilarProducts } from "../api/recommendationService";
import { RecommendationAlgorithm } from "../types/recommendation/RecommendationAlgorithm";
import type { ProductDto } from "../types/product/ProductDto";

export function useRecommendations() {
  const [similarProducts, setSimilarProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const getSimilarProductsHandler = async (
    productId: string,
    algorithm: RecommendationAlgorithm,
    topCount: number = 10
  ) => {
    setLoading(true);
    setError(null);

    try {
      const result = await getSimilarProducts({
        productId,
        algorithm,
        topCount,
      });

      if (result.status === 200 && result.data) {
        setSimilarProducts(result.data.products);
      } else {
        setError(result.message || "Nie udało się pobrać podobnych produktów");
        setSimilarProducts([]);
      }
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Błąd podczas pobierania rekomendacji"
      );
      setSimilarProducts([]);
    } finally {
      setLoading(false);
    }
  };

  const clearSimilarProducts = () => {
    setSimilarProducts([]);
    setError(null);
  };

  return {
    similarProducts,
    loading,
    error,
    getSimilarProducts: getSimilarProductsHandler,
    clearSimilarProducts,
  };
}
