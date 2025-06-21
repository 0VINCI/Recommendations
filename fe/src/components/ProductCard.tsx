import React from "react";
import { Link } from "react-router-dom";
import { Star, Heart, ShoppingCart } from "lucide-react";
import type { Product } from "../types";
import { useApp } from "../context/useApp";

interface ProductCardProps {
  product: Product;
}

export function ProductCard({ product }: ProductCardProps) {
  const { dispatch } = useApp();

  const addToCart = (e: React.MouseEvent) => {
    e.preventDefault();
    dispatch({
      type: "ADD_TO_CART",
      payload: {
        product,
        size: product.sizes[0],
        color: product.colors[0],
      },
    });
  };

  return (
    <div className="group relative bg-white dark:bg-gray-800 rounded-lg shadow-sm hover:shadow-md transition-shadow duration-200 overflow-hidden">
      <Link to={`/product/${product.id}`}>
        <div className="relative aspect-square overflow-hidden">
          <img
            src={product.image}
            alt={product.name}
            className="w-full h-full object-cover group-hover:scale-105 transition-transform duration-300"
          />

          {/* Badges */}
          <div className="absolute top-2 left-2 flex flex-col space-y-1">
            {product.isBestseller && (
              <span className="bg-red-500 text-white text-xs px-2 py-1 rounded-full font-medium">
                Bestseller
              </span>
            )}
            {product.isNew && (
              <span className="bg-green-500 text-white text-xs px-2 py-1 rounded-full font-medium">
                Nowość
              </span>
            )}
            {product.originalPrice && (
              <span className="bg-orange-500 text-white text-xs px-2 py-1 rounded-full font-medium">
                -
                {Math.round(
                  ((product.originalPrice - product.price) /
                    product.originalPrice) *
                    100
                )}
                %
              </span>
            )}
          </div>

          {/* Quick Actions */}
          <div className="absolute top-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity duration-200">
            <button className="p-2 bg-white dark:bg-gray-800 rounded-full shadow-md hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors">
              <Heart className="w-4 h-4 text-gray-600 dark:text-gray-300" />
            </button>
          </div>

          {/* Quick Add to Cart */}
          <div className="absolute bottom-2 left-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity duration-200">
            <button
              onClick={addToCart}
              className="w-full bg-primary-600 hover:bg-primary-700 text-white py-2 px-4 rounded-md flex items-center justify-center space-x-2 transition-colors"
            >
              <ShoppingCart className="w-4 h-4" />
              <span className="text-sm font-medium">Dodaj do koszyka</span>
            </button>
          </div>
        </div>
      </Link>

      <div className="p-4">
        <Link to={`/product/${product.id}`}>
          <h3 className="text-sm font-medium text-gray-900 dark:text-white mb-1 hover:text-primary-600 dark:hover:text-primary-400 transition-colors">
            {product.name}
          </h3>
        </Link>

        <div className="flex items-center space-x-1 mb-2">
          <div className="flex items-center">
            {[...Array(5)].map((_, i) => (
              <Star
                key={i}
                className={`w-3 h-3 ${
                  i < Math.floor(product.rating)
                    ? "text-yellow-400 fill-current"
                    : "text-gray-300 dark:text-gray-600"
                }`}
              />
            ))}
          </div>
          <span className="text-xs text-gray-500 dark:text-gray-400">
            ({product.reviews})
          </span>
        </div>

        <div className="flex items-center space-x-2">
          <span className="text-lg font-bold text-gray-900 dark:text-white">
            {product.price.toFixed(2)} zł
          </span>
          {product.originalPrice && (
            <span className="text-sm text-gray-500 dark:text-gray-400 line-through">
              {product.originalPrice.toFixed(2)} zł
            </span>
          )}
        </div>

        <div className="mt-2 flex flex-wrap gap-1">
          {product.colors.slice(0, 4).map((color, index) => (
            <div
              key={index}
              className="w-4 h-4 rounded-full border border-gray-300 dark:border-gray-600"
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
                    : color === "Bordowy"
                    ? "#991b1b"
                    : color === "Granatowy"
                    ? "#1e40af"
                    : color === "Beżowy"
                    ? "#d2b48c"
                    : "#6b7280",
              }}
              title={color}
            />
          ))}
          {product.colors.length > 4 && (
            <span className="text-xs text-gray-500 dark:text-gray-400">
              +{product.colors.length - 4}
            </span>
          )}
        </div>
      </div>
    </div>
  );
}
