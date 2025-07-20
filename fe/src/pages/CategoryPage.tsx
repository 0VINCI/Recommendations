import { useState, useEffect, useCallback } from "react";
import { useParams, useSearchParams } from "react-router-dom";
import { SlidersHorizontal } from "lucide-react";
import { ProductCard } from "../components/ProductCard";
import { useProducts } from "../hooks/useProducts";
import { Loader } from "../components/common/Loader";

export function CategoryPage() {
  const { category } = useParams<{ category: string }>();
  const [searchParams, setSearchParams] = useSearchParams();

  // Pobierz parametry z URL lub użyj domyślnych
  const urlPage = parseInt(searchParams.get("page") || "1");
  const urlPageSize = parseInt(searchParams.get("pageSize") || "20");

  const {
    products,
    loading,
    error,
    totalProductCount,
    productPage,
    productPageSize,
    totalProductPages,
    bestsellers,
    bestsellersLoading,
    bestsellersError,
    totalBestsellersCount,
    bestsellersPage,
    bestsellersPageSize,
    totalBestsellersPages,
    newProducts,
    newProductsLoading,
    newProductsError,
    totalNewProductsCount,
    newProductsPage,
    newProductsPageSize,
    totalNewProductsPages,
    masterCategories,
    getProducts,
    getProductsByCategory,
    getBestsellers,
    getNewProducts,
    loadCategories,
  } = useProducts();

  const [sortBy, setSortBy] = useState("name");
  const [priceRange, setPriceRange] = useState([0, 1000]);
  const [showFilters, setShowFilters] = useState(false);
  const [itemsPerPage, setItemsPerPage] = useState(urlPageSize);

  // Załaduj kategorie przy starcie
  useEffect(() => {
    if (!masterCategories || masterCategories.length === 0) {
      loadCategories();
    }
  }, [masterCategories, loadCategories]);

  const getCategoryIds = useCallback(
    (categoryName: string) => {
      if (!masterCategories)
        return { masterCategoryId: undefined, subCategoryId: undefined };

      // Szukaj w master categories
      const masterCategory = masterCategories.find(
        (cat) => cat.name.toLowerCase() === categoryName.toLowerCase()
      );
      if (masterCategory) {
        return {
          masterCategoryId: masterCategory.id,
          subCategoryId: undefined,
        };
      }

      // Szukaj w sub categories
      for (const masterCat of masterCategories) {
        const subCategory = masterCat.subCategories?.find(
          (sub) => sub.name.toLowerCase() === categoryName.toLowerCase()
        );
        if (subCategory) {
          return {
            masterCategoryId: masterCat.id,
            subCategoryId: subCategory.id,
          };
        }
      }

      return { masterCategoryId: undefined, subCategoryId: undefined };
    },
    [masterCategories]
  );

  useEffect(() => {
    if (category === "new") {
      getNewProducts(urlPage, itemsPerPage);
    } else if (category === "bestsellers") {
      getBestsellers(urlPage, itemsPerPage);
    } else if (category && category !== "wszystkie") {
      // Sprawdź czy kategorie są załadowane
      if (!masterCategories || masterCategories.length === 0) {
        return;
      }

      const { masterCategoryId, subCategoryId } = getCategoryIds(category);

      if (masterCategoryId || subCategoryId) {
        getProductsByCategory(
          masterCategoryId,
          subCategoryId,
          urlPage,
          itemsPerPage
        );
      } else {
        getProducts(urlPage, itemsPerPage);
      }
    } else {
      getProducts(urlPage, itemsPerPage);
    }
  }, [
    category,
    urlPage,
    itemsPerPage,
    getCategoryIds,
    getProducts,
    getProductsByCategory,
    getBestsellers,
    getNewProducts,
  ]);

  // Wybierz odpowiednie produkty na podstawie kategorii
  const getCategoryProducts = () => {
    if (category === "new") {
      return newProducts || [];
    } else if (category === "bestsellers") {
      return bestsellers || [];
    } else {
      return products;
    }
  };

  // Wybierz odpowiedni loading state
  const getLoadingState = () => {
    if (category === "new") {
      return newProductsLoading;
    } else if (category === "bestsellers") {
      return bestsellersLoading;
    } else {
      return loading;
    }
  };

  // Wybierz odpowiedni error state
  const getErrorState = () => {
    if (category === "new") {
      return newProductsError;
    } else if (category === "bestsellers") {
      return bestsellersError;
    } else {
      return error;
    }
  };

  // Wybierz odpowiednie stany paginacji
  const getPaginationState = () => {
    if (category === "new") {
      return {
        currentPage: newProductsPage,
        pageSize: newProductsPageSize,
        totalPages: totalNewProductsPages,
        totalCount: totalNewProductsCount,
        onPageChange: (page: number) => {
          updateURL(page, itemsPerPage);
          getNewProducts(page, itemsPerPage);
        },
      };
    } else if (category === "bestsellers") {
      return {
        currentPage: bestsellersPage,
        pageSize: bestsellersPageSize,
        totalPages: totalBestsellersPages,
        totalCount: totalBestsellersCount,
        onPageChange: (page: number) => {
          updateURL(page, itemsPerPage);
          getBestsellers(page, itemsPerPage);
        },
      };
    } else {
      return {
        currentPage: productPage,
        pageSize: productPageSize,
        totalPages: totalProductPages,
        totalCount: totalProductCount,
        onPageChange: (page: number) => {
          updateURL(page, itemsPerPage);
          if (category && category !== "wszystkie") {
            const { masterCategoryId, subCategoryId } =
              getCategoryIds(category);
            getProductsByCategory(
              masterCategoryId,
              subCategoryId,
              page,
              itemsPerPage
            );
          } else {
            getProducts(page, itemsPerPage);
          }
        },
      };
    }
  };

  // Funkcja do aktualizacji URL
  const updateURL = (page: number, pageSize: number) => {
    const newSearchParams = new URLSearchParams(searchParams);
    newSearchParams.set("page", page.toString());
    newSearchParams.set("pageSize", pageSize.toString());
    setSearchParams(newSearchParams);
  };

  const categoryProducts = getCategoryProducts();
  const isLoading = getLoadingState();
  const hasError = getErrorState();
  const pagination = getPaginationState();

  // Komponent paginacji
  const Pagination = () => {
    if (pagination.totalPages <= 1) return null;

    const pages = [];
    const startPage = Math.max(1, pagination.currentPage - 2);
    const endPage = Math.min(pagination.totalPages, pagination.currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return (
      <div className="flex justify-center items-center space-x-2 mt-8">
        <button
          onClick={() => pagination.onPageChange(pagination.currentPage - 1)}
          disabled={pagination.currentPage === 1}
          className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Poprzednia
        </button>

        {startPage > 1 && (
          <>
            <button
              onClick={() => pagination.onPageChange(1)}
              className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700"
            >
              1
            </button>
            {startPage > 2 && <span className="px-2">...</span>}
          </>
        )}

        {pages.map((page) => (
          <button
            key={page}
            onClick={() => pagination.onPageChange(page)}
            className={`px-3 py-2 border rounded-lg ${
              page === pagination.currentPage
                ? "bg-primary-600 text-white border-primary-600"
                : "border-gray-300 dark:border-gray-600 hover:bg-gray-50 dark:hover:bg-gray-700"
            }`}
          >
            {page}
          </button>
        ))}

        {endPage < pagination.totalPages && (
          <>
            {endPage < pagination.totalPages - 1 && (
              <span className="px-2">...</span>
            )}
            <button
              onClick={() => pagination.onPageChange(pagination.totalPages)}
              className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700"
            >
              {pagination.totalPages}
            </button>
          </>
        )}

        <button
          onClick={() => pagination.onPageChange(pagination.currentPage + 1)}
          disabled={pagination.currentPage === pagination.totalPages}
          className="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Następna
        </button>
      </div>
    );
  };

  const filteredProducts = categoryProducts.filter((product) => {
    if (
      category === "wszystkie" ||
      category === "new" ||
      category === "bestsellers"
    )
      return true;
    return product.subCategoryName.toLowerCase() === category?.toLowerCase();
  });

  const sortedProducts = [...filteredProducts].sort((a, b) => {
    switch (sortBy) {
      case "price-low":
        return a.price - b.price;
      case "price-high":
        return b.price - a.price;
      case "rating":
        return b.rating - a.rating;
      case "name":
      default:
        return a.productDisplayName.localeCompare(b.productDisplayName);
    }
  });

  const categoryNames: { [key: string]: string } = {
    wszystkie: "Wszystkie produkty",
    new: "Nowe produkty",
    bestsellers: "Bestsellery",
    koszule: "Koszule",
    spodnie: "Spodnie",
    sukienki: "Sukienki",
    bluzy: "Bluzy",
    marynarki: "Marynarki",
    koszulki: "Koszulki",
    spódnice: "Spódnice",
    swetry: "Swetry",
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center">
        <Loader />
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Header */}
        <div className="flex items-center justify-between mb-8">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 dark:text-white">
              {categoryNames[category || "wszystkie"]}
            </h1>
            <p className="text-gray-600 dark:text-gray-400 mt-1">
              {pagination.totalCount} produktów • {itemsPerPage} na stronę
            </p>
          </div>

          <div className="flex items-center space-x-4">
            {/* Items per page */}
            <select
              value={itemsPerPage}
              onChange={(e) => {
                const newPageSize = parseInt(e.target.value);
                setItemsPerPage(newPageSize);
                // Reset to first page when changing page size
                updateURL(1, newPageSize);
                if (category === "new") {
                  getNewProducts(1, newPageSize);
                } else if (category === "bestsellers") {
                  getBestsellers(1, newPageSize);
                } else if (category && category !== "wszystkie") {
                  const { masterCategoryId, subCategoryId } =
                    getCategoryIds(category);
                  getProductsByCategory(
                    masterCategoryId,
                    subCategoryId,
                    1,
                    newPageSize
                  );
                } else {
                  getProducts(1, newPageSize);
                }
              }}
              className="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-primary-500"
            >
              <option value={5}>Wyświetlaj 5 produktów</option>
              <option value={10}>Wyświetlaj 10 produktów</option>
              <option value={15}>Wyświetlaj 15 produktów</option>
              <option value={20}>Wyświetlaj 20 produktów</option>
              <option value={30}>Wyświetlaj 30 produktów</option>
            </select>

            {/* Sort */}
            <select
              value={sortBy}
              onChange={(e) => setSortBy(e.target.value)}
              className="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:ring-2 focus:ring-primary-500"
            >
              <option value="name">Sortuj: A-Z</option>
              <option value="price-low">Cena: od najniższej</option>
              <option value="price-high">Cena: od najwyższej</option>
              <option value="rating">Ocena: od najwyższej</option>
            </select>

            {/* Filters Toggle */}
            <button
              onClick={() => setShowFilters(!showFilters)}
              className="flex items-center space-x-2 px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors"
            >
              <SlidersHorizontal className="w-4 h-4" />
              <span className="text-gray-700 dark:text-gray-300">Filtry</span>
            </button>
          </div>
        </div>

        <div className="flex gap-8">
          {/* Filters Sidebar */}
          {showFilters && (
            <div className="w-64 bg-white dark:bg-gray-800 rounded-lg p-6 h-fit">
              <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
                Filtry
              </h3>

              {/* Price Range */}
              <div className="mb-6">
                <h4 className="text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                  Zakres cen
                </h4>
                <div className="space-y-2">
                  <input
                    type="range"
                    min="0"
                    max="1000"
                    value={priceRange[1]}
                    onChange={(e) =>
                      setPriceRange([priceRange[0], parseInt(e.target.value)])
                    }
                    className="w-full"
                  />
                  <div className="flex justify-between text-sm text-gray-600 dark:text-gray-400">
                    <span>{priceRange[0]} zł</span>
                    <span>{priceRange[1]} zł</span>
                  </div>
                </div>
              </div>

              {/* Size Filter */}
              <div className="mb-6">
                <h4 className="text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                  Rozmiary
                </h4>
                <div className="space-y-2">
                  {["XS", "S", "M", "L", "XL", "XXL"].map((size) => (
                    <label key={size} className="flex items-center">
                      <input
                        type="checkbox"
                        className="rounded border-gray-300 text-primary-600 focus:ring-primary-500"
                      />
                      <span className="ml-2 text-sm text-gray-700 dark:text-gray-300">
                        {size}
                      </span>
                    </label>
                  ))}
                </div>
              </div>

              {/* Color Filter */}
              <div>
                <h4 className="text-sm font-medium text-gray-700 dark:text-gray-300 mb-2">
                  Kolory
                </h4>
                <div className="grid grid-cols-4 gap-2">
                  {[
                    "Biały",
                    "Czarny",
                    "Szary",
                    "Niebieski",
                    "Czerwony",
                    "Zielony",
                    "Żółty",
                    "Różowy",
                  ].map((color) => (
                    <button
                      key={color}
                      className="w-8 h-8 rounded-full border-2 border-gray-300 dark:border-gray-600"
                      style={{
                        backgroundColor:
                          color === "Biały"
                            ? "#ffffff"
                            : color === "Czarny"
                            ? "#000000"
                            : color === "Szary"
                            ? "#6b7280"
                            : color === "Niebieski"
                            ? "#3b82f6"
                            : color === "Czerwony"
                            ? "#ef4444"
                            : color === "Zielony"
                            ? "#10b981"
                            : color === "Żółty"
                            ? "#f59e0b"
                            : color === "Różowy"
                            ? "#ec4899"
                            : "#6b7280",
                      }}
                      title={color}
                    />
                  ))}
                </div>
              </div>
            </div>
          )}

          {/* Products Grid */}
          <div className="flex-1">
            {hasError && (
              <div className="text-center py-12">
                <p className="text-red-500 dark:text-red-400 text-lg">
                  {hasError}
                </p>
              </div>
            )}

            {!isLoading && !hasError && (
              <>
                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                  {sortedProducts.map((product) => (
                    <ProductCard key={product.id} product={product} />
                  ))}
                </div>

                {sortedProducts.length === 0 && (
                  <div className="text-center py-12">
                    <p className="text-gray-500 dark:text-gray-400 text-lg">
                      Brak produktów w tej kategorii
                    </p>
                  </div>
                )}

                <Pagination />
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
