import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { ProductCard } from "../components/ProductCard";
import { useProducts } from "../hooks/useProducts";
import type { ProductDto } from "../types/product/ProductDto";

export function ProductsPage() {
  const { getProducts } = useProducts();
  const [searchParams, setSearchParams] = useSearchParams();
  const currentPage = Number(searchParams.get("page") || 1);

  const [products, setProducts] = useState<ProductDto[]>([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    getProducts(currentPage, 20)
      .then((response) => {
        setProducts(response.products);
        setPage(response.page);
        setTotalPages(response.totalPages);
        setError(null);
      })
      .catch((err) => {
        setError(err.message || "Błąd podczas pobierania produktów");
      })
      .finally(() => setLoading(false));
  }, [currentPage]);

  // Numerki do paginacji: poprzednia, aktualna, następna (jeśli istnieje)
  const pageNumbers = [];
  if (page > 1) pageNumbers.push(page - 1);
  pageNumbers.push(page);
  if (page < totalPages) pageNumbers.push(page + 1);

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900 py-16">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-8 text-center">
          Przeglądaj Kolekcję
        </h2>
        {loading && <div>Ładowanie...</div>}
        {error && <div className="text-red-500">{error}</div>}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
          {products.map((product) => (
            <ProductCard key={product.id} product={product} />
          ))}
        </div>
        {/* Pagination Controls with numbers */}
        <div className="flex justify-center mt-8 items-center space-x-2">
          <button
            disabled={page === 1}
            onClick={() => setSearchParams({ page: String(page - 1) })}
            className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
          >
            Poprzednia
          </button>
          {pageNumbers.map((num) => (
            <button
              key={num}
              onClick={() => setSearchParams({ page: String(num) })}
              className={`px-3 py-2 rounded ${
                num === page
                  ? "bg-primary-600 text-white"
                  : "bg-gray-200 text-gray-900"
              }`}
              disabled={num === page}
            >
              {num}
            </button>
          ))}
          <button
            disabled={page === totalPages}
            onClick={() => setSearchParams({ page: String(page + 1) })}
            className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
          >
            Następna
          </button>
        </div>
      </div>
    </div>
  );
}
