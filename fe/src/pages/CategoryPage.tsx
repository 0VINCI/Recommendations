import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { SlidersHorizontal } from "lucide-react";
import { ProductCard } from "../components/ProductCard";
import { useProducts } from "../hooks/useProducts";

export function CategoryPage() {
  const { category } = useParams<{ category: string }>();
  const { products, loading, error, getProducts, getProductsByCategory } =
    useProducts();
  const [sortBy, setSortBy] = useState("name");
  const [priceRange, setPriceRange] = useState([0, 1000]);
  const [showFilters, setShowFilters] = useState(false);

  useEffect(() => {
    if (category && category !== "wszystkie") {
      getProductsByCategory(category);
    } else {
      getProducts();
    }
  }, [category, getProducts, getProductsByCategory]);

  const filteredProducts = products.filter((product) => {
    if (category === "wszystkie") return true;
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
    koszule: "Koszule",
    spodnie: "Spodnie",
    sukienki: "Sukienki",
    bluzy: "Bluzy",
    marynarki: "Marynarki",
    koszulki: "Koszulki",
    spódnice: "Spódnice",
    swetry: "Swetry",
  };

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
              {sortedProducts.length} produktów
            </p>
          </div>

          <div className="flex items-center space-x-4">
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
            {loading && (
              <div className="text-center py-12">
                <p className="text-gray-500 dark:text-gray-400 text-lg">
                  Ładowanie produktów...
                </p>
              </div>
            )}

            {error && (
              <div className="text-center py-12">
                <p className="text-red-500 dark:text-red-400 text-lg">
                  {error}
                </p>
              </div>
            )}

            {!loading && !error && (
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
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
