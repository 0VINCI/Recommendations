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
    <nav className="bg-white dark:bg-gray-900 border-b border-gray-100 dark:border-gray-800 shadow-soft">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex space-x-1 overflow-x-auto py-3 relative">
          {/* Wszystkie produkty */}
          <Link
            to="/category/wszystkie?page=1&pageSize=20"
            className={`whitespace-nowrap px-4 py-2.5 text-sm font-medium rounded-xl transition-all duration-200 ${
              location.pathname === "/category/wszystkie"
                ? "bg-brand-100 dark:bg-brand-900 text-brand-700 dark:text-brand-300 shadow-medium"
                : "text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white hover:bg-gray-100 dark:hover:bg-gray-800"
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
                <div className="flex items-center space-x-2 px-4 py-2.5 text-sm font-medium rounded-xl transition-all duration-200 cursor-pointer text-gray-600 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white hover:bg-gray-100 dark:hover:bg-gray-800">
                  <span>{category.name}</span>
                  <ChevronDown className="w-4 h-4" />
                </div>
              </div>
            ))}

          {/* Globalny dropdown z podkategoriami */}
          {hoveredCategory && masterCategories && (
            <div
              className="fixed bg-white dark:bg-gray-800 rounded-2xl shadow-strong border border-gray-100 dark:border-gray-700 z-[9999] animate-scale-in"
              style={{
                top: `${dropdownPosition.top}px`,
                left: `${dropdownPosition.left}px`,
                width: "18rem",
                maxHeight: "24rem",
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
              <div className="py-3">
                {masterCategories
                  .find((cat) => cat.id === hoveredCategory)
                  ?.subCategories?.map((subCategory) => (
                    <Link
                      key={subCategory.id}
                      to={`/category/${subCategory.name.toLowerCase()}?page=1&pageSize=20`}
                      className="block px-4 py-3 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors hover:text-brand-600 dark:hover:text-brand-400"
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
