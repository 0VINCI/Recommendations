import { useState, useEffect, useRef } from "react";
import { Link, useLocation } from "react-router-dom";
import { ChevronDown } from "lucide-react";
import { useProducts } from "../../hooks/useProducts";
import type { MasterCategoryDto } from "../../types/product/ProductDto";

export function Navigation() {
  const location = useLocation();
  const { masterCategories, loadCategories } = useProducts();
  const [hoveredCategory, setHoveredCategory] = useState<string | null>(null);
  const [dropdownPosition, setDropdownPosition] = useState({ top: 0, left: 0 });
  const [isDropdownHovered, setIsDropdownHovered] = useState(false);
  const hoverTimeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  useEffect(() => {
    loadCategories();
  }, [loadCategories]);

  // Cleanup timeout on unmount
  useEffect(() => {
    return () => {
      if (hoverTimeoutRef.current) {
        clearTimeout(hoverTimeoutRef.current);
      }
    };
  }, []);

  return (
    <nav className="bg-gray-50 dark:bg-gray-800 border-b border-gray-200 dark:border-gray-700">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex space-x-8 overflow-x-auto py-4 relative">
          {/* Wszystkie produkty */}
          <Link
            to="/category/wszystkie?page=1&pageSize=20"
            className={`whitespace-nowrap px-3 py-2 text-sm font-medium rounded-md transition-colors ${
              location.pathname === "/category/wszystkie"
                ? "bg-primary-100 dark:bg-primary-900 text-primary-700 dark:text-primary-300"
                : "text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white hover:bg-gray-100 dark:hover:bg-gray-700"
            }`}
          >
            Wszystkie
          </Link>

          {/* Dynamiczne kategorie */}
          {masterCategories &&
            masterCategories.map((category: MasterCategoryDto) => (
              <div
                key={category.id}
                className="relative"
                onMouseEnter={(e) => {
                  // Clear any existing timeout
                  if (hoverTimeoutRef.current) {
                    clearTimeout(hoverTimeoutRef.current);
                  }

                  const rect = e.currentTarget.getBoundingClientRect();
                  setDropdownPosition({
                    top: rect.bottom + 4,
                    left: rect.left,
                  });
                  setHoveredCategory(category.id);
                }}
                onMouseLeave={() => {
                  // Add delay before hiding dropdown
                  hoverTimeoutRef.current = setTimeout(() => {
                    if (!isDropdownHovered) {
                      setHoveredCategory(null);
                    }
                  }, 150);
                }}
              >
                <div className="flex items-center space-x-1 px-3 py-2 text-sm font-medium rounded-md transition-colors cursor-pointer text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white hover:bg-gray-100 dark:hover:bg-gray-700">
                  <span>{category.name}</span>
                  <ChevronDown className="w-4 h-4" />
                </div>
              </div>
            ))}

          {/* Globalny dropdown z podkategoriami */}
          {hoveredCategory && masterCategories && (
            <div
              className="fixed bg-white dark:bg-gray-800 rounded-lg shadow-lg border border-gray-200 dark:border-gray-700 z-[9999]"
              style={{
                top: `${dropdownPosition.top}px`,
                left: `${dropdownPosition.left}px`,
                width: "16rem",
                maxHeight: "20rem",
                overflowY: "auto",
              }}
              onMouseEnter={() => {
                setIsDropdownHovered(true);
                if (hoverTimeoutRef.current) {
                  clearTimeout(hoverTimeoutRef.current);
                }
              }}
              onMouseLeave={() => {
                setIsDropdownHovered(false);
                setHoveredCategory(null);
              }}
            >
              <div className="py-2">
                {masterCategories
                  .find((cat) => cat.id === hoveredCategory)
                  ?.subCategories?.map((subCategory) => (
                    <Link
                      key={subCategory.id}
                      to={`/category/${subCategory.name.toLowerCase()}?page=1&pageSize=20`}
                      className="block px-4 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
                      onClick={() => setHoveredCategory(null)}
                    >
                      {subCategory.name}
                    </Link>
                  ))}
              </div>
            </div>
          )}
        </div>
      </div>
    </nav>
  );
}
