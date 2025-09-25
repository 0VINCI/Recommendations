import { Link, useParams } from "react-router-dom";
import { ChevronRight, Home } from "lucide-react";
import type { MasterCategoryDto } from "../../types/product/ProductDto";

interface BreadcrumbItem {
  label: string;
  href?: string;
}

interface BreadcrumbsProps {
  items?: BreadcrumbItem[];
  className?: string;
  masterCategories?: MasterCategoryDto[];
}

export function Breadcrumbs({
  items,
  className = "",
  masterCategories = [],
}: BreadcrumbsProps) {
  const { category } = useParams<{ category: string }>();

  // Generowanie breadcrumbs na podstawie kategorii z API
  const generateBreadcrumbs = (): BreadcrumbItem[] => {
    const breadcrumbs: BreadcrumbItem[] = [
      { label: "Strona główna", href: "/" },
    ];

    if (!category) {
      return breadcrumbs;
    }

    // Dekoduj nazwę kategorii z URL
    const decodedCategory = decodeURIComponent(category);

    // Mapowanie specjalnych kategorii
    const specialCategories: { [key: string]: string } = {
      wszystkie: "Wszystkie Produkty",
      new: "Nowe Produkty",
      bestsellers: "Bestsellery",
      cart: "Koszyk",
      checkout: "Zamówienie",
      orders: "Moje Zamówienia",
      product: "Produkt",
    };

    if (specialCategories[decodedCategory]) {
      breadcrumbs.push({ label: specialCategories[decodedCategory] });
      return breadcrumbs;
    }

    // Szukaj w master categories
    let foundCategory: {
      masterName: string;
      subName?: string;
      isMasterCategory?: boolean;
    } | null = null;

    for (const masterCategory of masterCategories) {
      // Sprawdź sub categories (tylko sub categories są klikalne)
      for (const subCategory of masterCategory.subCategories || []) {
        if (subCategory.name.toLowerCase() === decodedCategory.toLowerCase()) {
          foundCategory = {
            masterName: masterCategory.name,
            subName: subCategory.name,
            isMasterCategory: false,
          };
          break;
        }
      }

      if (foundCategory) break;
    }

    if (foundCategory) {
      // Dodaj master category (tylko jako tekst, nie link)
      breadcrumbs.push({
        label: foundCategory.masterName,
      });

      // Dodaj sub category (jako link)
      breadcrumbs.push({
        label: foundCategory.subName!,
        href: `/category/${encodeURIComponent(
          foundCategory.subName!.toLowerCase()
        )}?page=1&pageSize=20`,
      });
    } else {
      // Fallback - użyj nazwy z URL
      breadcrumbs.push({
        label:
          decodedCategory.charAt(0).toUpperCase() + decodedCategory.slice(1),
      });
    }

    return breadcrumbs;
  };

  const breadcrumbItems = items || generateBreadcrumbs();

  return (
    <nav
      className={`flex items-center space-x-1 text-sm ${className}`}
      aria-label="Breadcrumb"
    >
      <ol className="flex items-center space-x-1">
        {breadcrumbItems.map((item, index) => (
          <li key={index} className="flex items-center">
            {index > 0 && (
              <ChevronRight className="w-4 h-4 text-gray-400 mx-1" />
            )}

            {index === 0 && <Home className="w-4 h-4 text-gray-500 mr-1" />}

            {item.href ? (
              <Link
                to={item.href}
                className="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200 transition-colors duration-200"
              >
                {item.label}
              </Link>
            ) : (
              <span className="text-gray-900 dark:text-white font-medium">
                {item.label}
              </span>
            )}
          </li>
        ))}
      </ol>
    </nav>
  );
}
