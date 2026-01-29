import { useEffect, useRef, useState } from "react";
import { ChevronLeft, ChevronRight, Users } from "lucide-react";
import { ProductCard } from "./ProductCard";
import { RecommendationSkeleton } from "./common/RecommendationSkeleton";
import { useTracking } from "../hooks/useTracking";
import { useApp } from "../context/useApp";
import { getCfSimilarProducts } from "../api/recommendationService";
import type { ProductDto } from "../types/product/ProductDto";

interface AlsoBoughtProductsProps {
  productId: string;
  currentProductName: string;
}

export function AlsoBoughtProducts({
  productId,
  currentProductName,
}: AlsoBoughtProductsProps) {
  const { state } = useApp();
  const { recImpression, recClick } = useTracking(state.user?.IdUser);
  const [products, setProducts] = useState<ProductDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const scrollContainerRef = useRef<HTMLDivElement>(null);

  const scrollLeft = () => {
    if (scrollContainerRef.current) {
      const container = scrollContainerRef.current;
      const cardWidth = 320;
      const scrollAmount = Math.min(cardWidth * 2, container.scrollLeft);
      container.scrollBy({ left: -scrollAmount, behavior: "smooth" });
    }
  };

  const scrollRight = () => {
    if (scrollContainerRef.current) {
      const container = scrollContainerRef.current;
      const cardWidth = 320;
      const maxScroll = container.scrollWidth - container.clientWidth;
      const scrollAmount = Math.min(
        cardWidth * 2,
        maxScroll - container.scrollLeft
      );
      container.scrollBy({ left: scrollAmount, behavior: "smooth" });
    }
  };

  useEffect(() => {
    if (!productId) return;

    const fetchProducts = async () => {
      setLoading(true);
      setError(null);

      const result = await getCfSimilarProducts(productId, 8);

      if (result.status === 200 && result.data) {
        setProducts(result.data);
      } else if (result.status === 404) {
        setProducts([]);
      } else {
        setError(result.message || "Błąd pobierania rekomendacji");
      }

      setLoading(false);
    };

    void fetchProducts();
  }, [productId]);

  useEffect(() => {
    if (!products || products.length === 0) return;

    products.forEach((p, idx) => {
      void recImpression(
        "pdp_also_bought",
        "CollaborativeFiltering",
        productId,
        String(p.id),
        idx + 1
      );
    });
  }, [products, productId, recImpression]);

  const handleClick = (itemId: string, index: number) => {
    void recClick(
      "pdp_also_bought",
      "CollaborativeFiltering",
      productId,
      itemId,
      index + 1
    );
  };

  if (loading) {
    return <RecommendationSkeleton />;
  }

  if (error) {
    return null; // Ukryj sekcję w przypadku błędu
  }

  if (!products || products.length === 0) {
    return null; // Ukryj sekcję jeśli brak produktów (cold start)
  }

  return (
    <div className="py-8">
      <div className="mb-6">
        <div className="flex items-center gap-3 mb-2">
          <div className="p-2 bg-blue-100 dark:bg-blue-900 rounded-lg">
            <Users className="w-5 h-5 text-blue-600 dark:text-blue-400" />
          </div>
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white">
            Klienci kupili również
          </h2>
        </div>
        <p className="text-gray-600 dark:text-gray-400">
          Produkty wybierane przez klientów zainteresowanych:{" "}
          <span className="font-medium">{currentProductName}</span>
        </p>
      </div>

      <div className="relative">
        <button
          onClick={scrollLeft}
          className="absolute left-0 top-1/2 -translate-y-1/2 z-10 bg-white dark:bg-gray-800 rounded-full p-2 shadow-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
          aria-label="Przewiń w lewo"
        >
          <ChevronLeft className="w-5 h-5 text-gray-600 dark:text-gray-400" />
        </button>

        <button
          onClick={scrollRight}
          className="absolute right-0 top-1/2 -translate-y-1/2 z-10 bg-white dark:bg-gray-800 rounded-full p-2 shadow-lg border border-gray-200 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
          aria-label="Przewiń w prawo"
        >
          <ChevronRight className="w-5 h-5 text-gray-600 dark:text-gray-400" />
        </button>

        <div
          ref={scrollContainerRef}
          className="flex gap-6 overflow-x-auto scrollbar-hide pb-4"
          style={{
            scrollbarWidth: "none",
            msOverflowStyle: "none",
          }}
        >
          {products.map((product, index) => (
            <div key={product.id} className="flex-shrink-0 w-80">
              <ProductCard
                product={product}
                onCardClick={() => handleClick(String(product.id), index)}
              />
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}

