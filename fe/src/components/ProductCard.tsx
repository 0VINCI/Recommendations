import React from "react";
import { Link } from "react-router-dom";
import { Star, Heart, ShoppingCart, TrendingUp, Sparkles } from "lucide-react";
import type { ProductDto } from "../types/product/ProductDto";
import { useCart } from "../hooks/useCart";

interface ProductCardProps {
  product: ProductDto;
}

export function ProductCard({ product }: ProductCardProps) {
  const { addToCart } = useCart();
  const [currentImageIndex, setCurrentImageIndex] = React.useState(0);
  const [imageError, setImageError] = React.useState(false);

  React.useEffect(() => {
    setCurrentImageIndex(0);
    setImageError(false);
  }, [product.id]);

  const primaryImage =
    product.images?.find((img) => img.isPrimary) || product.images?.[0];
  const defaultImageUrl =
    primaryImage?.imageUrl ||
    `https://via.placeholder.com/300x300/cccccc/666666?text=${encodeURIComponent(
      product.productDisplayName
    )}`;

  // Aktualnie wyświetlany obraz
  const currentImage = product.images?.[currentImageIndex] || primaryImage;
  const imageUrl = imageError
    ? `https://via.placeholder.com/300x300/cccccc/666666?text=${encodeURIComponent(
        product.productDisplayName
      )}`
    : currentImage?.imageUrl || defaultImageUrl;

  // Funkcja do zmiany obrazu
  const changeImage = (e: React.MouseEvent, index: number) => {
    e.preventDefault();
    e.stopPropagation();
    setCurrentImageIndex(index);
  };

  const handleAddToCart = (e: React.MouseEvent) => {
    e.preventDefault();
    addToCart(product);
  };

  return (
    <div className="group relative bg-white dark:bg-gray-800 rounded-lg shadow-sm hover:shadow-md transition-shadow duration-200 overflow-hidden">
      <Link to={`/product/${product.id}`}>
        <div className="relative aspect-square overflow-hidden">
          <img
            src={imageUrl}
            alt={product.productDisplayName}
            className="w-full h-full object-contain group-hover:scale-105 transition-transform duration-300 bg-white dark:bg-gray-800"
            onError={() => {
              // Ustaw flagę błędu żeby zapobiec pętli
              setImageError(true);
            }}
          />

          {/* Badges */}
          <div className="absolute top-2 left-2 flex flex-col space-y-1 z-10">
            {/* Bestseller badge */}
            {product.isBestseller && (
              <span className="bg-gradient-to-r from-red-500 to-red-600 text-white text-xs px-3 py-1.5 rounded-full font-semibold flex items-center space-x-1 shadow-lg border border-red-400">
                <TrendingUp className="w-3 h-3" />
                <span>Bestseller</span>
              </span>
            )}

            {/* New product badge */}
            {product.isNew && (
              <span className="bg-gradient-to-r from-green-500 to-green-600 text-white text-xs px-3 py-1.5 rounded-full font-semibold flex items-center space-x-1 shadow-lg border border-green-400">
                <Sparkles className="w-3 h-3" />
                <span>Nowość</span>
              </span>
            )}

            {/* Discount badge */}
            {product.originalPrice && (
              <span className="bg-gradient-to-r from-orange-500 to-orange-600 text-white text-xs px-3 py-1.5 rounded-full font-semibold shadow-lg border border-orange-400">
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
          <div className="absolute bottom-2 right-2 opacity-0 group-hover:opacity-100 transition-opacity duration-200 z-20">
            <button
              onClick={handleAddToCart}
              className="bg-primary-600 hover:bg-primary-700 text-white py-2 px-2 rounded-md flex items-center justify-center transition-colors shadow-lg min-w-[40px]"
              aria-label="Dodaj do koszyka"
              title="Dodaj do koszyka"
            >
              <ShoppingCart className="w-5 h-5" />
            </button>
          </div>

          {/* Image Gallery Thumbnails */}
          {product.images && product.images.length > 1 && (
            <div className="absolute bottom-2 left-2 opacity-0 group-hover:opacity-100 transition-opacity duration-200 z-10">
              <div className="flex space-x-1 bg-white/80 dark:bg-gray-900/80 p-1 rounded-md shadow-md">
                {product.images.slice(0, 3).map((image, index) => (
                  <button
                    key={index}
                    onClick={(e) => changeImage(e, index)}
                    className={`w-8 h-8 rounded border-2 overflow-hidden transition-all duration-200 hover:scale-110 ${
                      currentImageIndex === index
                        ? "border-primary-500 ring-2 ring-primary-300"
                        : "border-white hover:border-primary-300"
                    }`}
                  >
                    <img
                      src={image.imageUrl}
                      alt={`${product.productDisplayName} - ${image.imageType}`}
                      className="w-full h-full object-contain bg-white dark:bg-gray-800"
                      onError={(e) => {
                        const target = e.target as HTMLImageElement;
                        target.src = `https://via.placeholder.com/32x32/cccccc/666666?text=${
                          index + 1
                        }`;
                      }}
                    />
                  </button>
                ))}
                {product.images.length > 3 && (
                  <div className="w-8 h-8 rounded border-2 border-white shadow-md bg-gray-800 text-white text-xs flex items-center justify-center">
                    +{product.images.length - 3}
                  </div>
                )}
              </div>
            </div>
          )}

          {/* USUNIĘTO DOT INDICATORS */}
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
