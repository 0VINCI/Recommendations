import React from "react";
import { Link } from "react-router-dom";
import { Star, Heart, ShoppingCart } from "lucide-react";
import type { ProductDto } from "../types/product/ProductDto";
import type { Product as CartProduct } from "../types/cart";
import { useApp } from "../context/useApp";

interface ProductCardProps {
  product: ProductDto;
}

export function ProductCard({ product }: ProductCardProps) {
  const { dispatch } = useApp();

  const addToCart = (e: React.MouseEvent) => {
    e.preventDefault();
    // Konwertuj ProductDto na Product dla koszyka z dostępnymi danymi z backendu
    const cartProduct: CartProduct = {
      id: product.id,
      name: product.productDisplayName,
      price: product.price,
      originalPrice: product.originalPrice,
      image: `https://via.placeholder.com/300x300/cccccc/666666?text=${encodeURIComponent(
        product.productDisplayName
      )}`,
      category: product.subCategoryName,
      description: `${product.brandName} - ${product.productDisplayName}`,
      sizes: ["S", "M", "L", "XL"], // Placeholder - można dodać później
      colors: [product.baseColourName || "Default"], // Placeholder - można dodać później
      rating: product.rating,
      reviews: product.reviews,
      isBestseller: product.isBestseller,
      isNew: product.isNew,
      // Dodatkowe pola z backendu - używamy dostępnych pól
      subCategory: product.subCategoryName,
      baseColour: product.baseColourName,
    };

    dispatch({
      type: "ADD_TO_CART",
      payload: {
        product: cartProduct,
        size: "",
        color: "",
      },
    });
  };

  return (
    <div className="group relative bg-white dark:bg-gray-800 rounded-lg shadow-sm hover:shadow-md transition-shadow duration-200 overflow-hidden">
      <Link to={`/product/${product.id}`}>
        <div className="relative aspect-square overflow-hidden">
          <img
            src={`https://via.placeholder.com/300x300/cccccc/666666?text=${encodeURIComponent(
              product.productDisplayName
            )}`}
            alt={product.productDisplayName}
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
            {product.productDisplayName}
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

        {/* Product Details */}
        <div className="mt-2 text-xs text-gray-500 dark:text-gray-400">
          {product.subCategoryName}
          {product.brandName && ` • ${product.brandName}`}
          {product.baseColourName && ` • ${product.baseColourName}`}
        </div>
      </div>
    </div>
  );
}
