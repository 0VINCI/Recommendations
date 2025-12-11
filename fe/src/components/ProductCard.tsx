import React from "react";
import { Link } from "react-router-dom";
import { Star, Heart, ShoppingCart, TrendingUp, Sparkles } from "lucide-react";
import type { ProductDto } from "../types/product/ProductDto";
import { useCart } from "../hooks/useCart";
import { useTracking } from "../hooks/useTracking";
import { useApp } from "../context/useApp";

interface ProductCardProps {
  product: ProductDto;
  viewMode?: "grid" | "list";
  listId?: string;
  position?: number;
  onCardClick?: () => void; // Custom click handler (e.g., for rec_click)
}

export function ProductCard({
  product,
  viewMode = "grid",
  listId,
  position,
  onCardClick,
}: ProductCardProps) {
  const { addToCart } = useCart();
  const { state } = useApp();
  const { productClicked, itemImpression } = useTracking(state.user?.IdUser);
  const [currentImageIndex, setCurrentImageIndex] = React.useState(0);
  const [imageError, setImageError] = React.useState(false);

  React.useEffect(() => {
    setCurrentImageIndex(0);
    setImageError(false);
  }, [product.id]);

  React.useEffect(() => {
    // fire lightweight impression when rendered in a list with known position
    if (listId && typeof position === "number") {
      void itemImpression(String(product.id), listId, position);
    }
  }, [itemImpression, listId, position, product.id]);

  const primaryImage =
    product.images?.find((img) => img.isPrimary) || product.images?.[0];
  const defaultImageUrl =
    primaryImage?.imageUrl ||
    `https://picsum.photos/300/300?random=${product.id}`;

  const currentImage = product.images?.[currentImageIndex] || primaryImage;
  const imageUrl = imageError
    ? `https://picsum.photos/300/300?random=${product.id}`
    : currentImage?.imageUrl || defaultImageUrl;

  const changeImage = (e: React.MouseEvent, index: number) => {
    e.preventDefault();
    e.stopPropagation();
    setCurrentImageIndex(index);
  };

  const handleAddToCart = (e: React.MouseEvent) => {
    e.preventDefault();
    addToCart(product);
  };

  const handleCardClick = () => {
    // If custom click handler is provided (e.g., for rec_click), use it
    if (onCardClick) {
      onCardClick();
    } else {
      // Otherwise use standard product_clicked tracking
      void productClicked(String(product.id), listId, position);
    }
  };

  if (viewMode === "list") {
    return (
      <div className="group bg-white dark:bg-gray-800 rounded-2xl shadow-soft hover:shadow-strong transition-all duration-300 border border-gray-100 dark:border-gray-700 hover:border-gray-200 dark:hover:border-gray-600">
        <Link
          to={`/product/${product.id}`}
          className="flex"
          onClick={handleCardClick}
        >
          <div className="relative w-32 h-32 flex-shrink-0 overflow-hidden bg-gray-50 dark:bg-gray-900">
            <img
              src={imageUrl}
              alt={product.productDisplayName}
              className="w-full h-full object-contain group-hover:scale-110 transition-transform duration-500 ease-out bg-white dark:bg-gray-800"
              onError={() => {
                setImageError(true);
              }}
            />

            {/* Badges */}
            <div className="absolute top-2 left-2 flex flex-col space-y-1 z-10">
              {product.isBestseller && (
                <span className="bg-gradient-to-r from-accent-500 to-accent-600 text-white text-xs px-2 py-1 rounded-full font-semibold flex items-center space-x-1 shadow-medium border border-accent-400">
                  <TrendingUp className="w-3 h-3" />
                  <span>Bestseller</span>
                </span>
              )}

              {product.isNew && (
                <span className="bg-gradient-to-r from-green-500 to-green-600 text-white text-xs px-2 py-1 rounded-full font-semibold flex items-center space-x-1 shadow-medium border border-green-400">
                  <Sparkles className="w-3 h-3" />
                  <span>Nowość</span>
                </span>
              )}

              {product.originalPrice && (
                <span className="bg-gradient-to-r from-orange-500 to-orange-600 text-white text-xs px-2 py-1 rounded-full font-semibold shadow-medium border border-orange-400">
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
          </div>

          <div className="flex-1 p-4 flex flex-col justify-between">
            <div>
              <h3 className="text-lg font-medium text-gray-900 dark:text-white line-clamp-2 group-hover:text-brand-600 dark:group-hover:text-brand-400 transition-colors mb-2">
                {product.productDisplayName}
              </h3>
              <p className="text-sm text-gray-500 dark:text-gray-400 mb-3">
                {product.subCategoryName}
              </p>
            </div>

            <div className="flex items-center justify-between">
              <div className="flex items-center space-x-2">
                <span className="text-xl font-bold text-gray-900 dark:text-white">
                  {product.price.toFixed(2)} zł
                </span>
                {product.originalPrice && (
                  <span className="text-sm text-gray-500 dark:text-gray-400 line-through">
                    {product.originalPrice.toFixed(2)} zł
                  </span>
                )}
              </div>

              <div className="flex items-center space-x-2">
                <button
                  onClick={(e) => {
                    e.preventDefault();
                    e.stopPropagation();
                  }}
                  className="p-2 bg-gray-100 dark:bg-gray-700 rounded-full hover:bg-gray-200 dark:hover:bg-gray-600 transition-colors"
                >
                  <Heart className="w-4 h-4 text-gray-600 dark:text-gray-400" />
                </button>
                <button
                  onClick={handleAddToCart}
                  className="p-2 bg-brand-100 dark:bg-brand-900 text-brand-600 dark:text-brand-400 rounded-full hover:bg-brand-200 dark:hover:bg-brand-800 transition-colors"
                >
                  <ShoppingCart className="w-4 h-4" />
                </button>
              </div>
            </div>
          </div>
        </Link>
      </div>
    );
  }

  return (
    <div className="group relative bg-white dark:bg-gray-800 rounded-2xl shadow-soft hover:shadow-strong transition-all duration-300 overflow-hidden border border-gray-100 dark:border-gray-700 hover:border-gray-200 dark:hover:border-gray-600">
      <Link to={`/product/${product.id}`} onClick={handleCardClick}>
        <div className="relative aspect-square overflow-hidden bg-gray-50 dark:bg-gray-900">
          <img
            src={imageUrl}
            alt={product.productDisplayName}
            className="w-full h-full object-contain group-hover:scale-110 transition-transform duration-500 ease-out bg-white dark:bg-gray-800"
            onError={() => {
              setImageError(true);
            }}
          />

          {/* Badges */}
          <div className="absolute top-3 left-3 flex flex-col space-y-2 z-10">
            {/* Bestseller badge */}
            {product.isBestseller && (
              <span className="bg-gradient-to-r from-accent-500 to-accent-600 text-white text-xs px-3 py-1.5 rounded-full font-semibold flex items-center space-x-1 shadow-medium border border-accent-400">
                <TrendingUp className="w-3 h-3" />
                <span>Bestseller</span>
              </span>
            )}

            {/* New product badge */}
            {product.isNew && (
              <span className="bg-gradient-to-r from-green-500 to-green-600 text-white text-xs px-3 py-1.5 rounded-full font-semibold flex items-center space-x-1 shadow-medium border border-green-400">
                <Sparkles className="w-3 h-3" />
                <span>Nowość</span>
              </span>
            )}

            {/* Discount badge */}
            {product.originalPrice && (
              <span className="bg-gradient-to-r from-orange-500 to-orange-600 text-white text-xs px-3 py-1.5 rounded-full font-semibold shadow-medium border border-orange-400">
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
          <div className="absolute top-3 right-3 opacity-0 group-hover:opacity-100 transition-all duration-300 transform translate-y-2 group-hover:translate-y-0">
            <button className="p-2.5 bg-white/90 dark:bg-gray-800/90 backdrop-blur-sm rounded-full shadow-medium hover:bg-white dark:hover:bg-gray-800 transition-all duration-200 hover:scale-110">
              <Heart className="w-4 h-4 text-gray-600 dark:text-gray-300" />
            </button>
          </div>

          {/* Quick Add to Cart */}
          <div className="absolute bottom-3 right-3 opacity-0 group-hover:opacity-100 transition-all duration-300 transform translate-y-2 group-hover:translate-y-0 z-20">
            <button
              onClick={handleAddToCart}
              className="bg-brand-600 hover:bg-brand-700 text-white py-2.5 px-3 rounded-xl flex items-center justify-center transition-all duration-200 shadow-medium hover:shadow-strong min-w-[44px] hover:scale-105"
              aria-label="Dodaj do koszyka"
              title="Dodaj do koszyka"
            >
              <ShoppingCart className="w-5 h-5" />
            </button>
          </div>

          {/* Image Gallery Thumbnails */}
          {product.images && product.images.length > 1 && (
            <div className="absolute bottom-3 left-3 opacity-0 group-hover:opacity-100 transition-all duration-300 transform translate-y-2 group-hover:translate-y-0 z-10">
              <div className="flex space-x-1.5 bg-white/90 dark:bg-gray-900/90 backdrop-blur-sm p-1.5 rounded-xl shadow-medium">
                {product.images.slice(0, 3).map((image, index) => (
                  <button
                    key={index}
                    onClick={(e) => changeImage(e, index)}
                    className={`w-8 h-8 rounded-lg border-2 overflow-hidden transition-all duration-200 hover:scale-110 ${
                      currentImageIndex === index
                        ? "border-brand-500 ring-2 ring-brand-300"
                        : "border-white hover:border-brand-300"
                    }`}
                  >
                    <img
                      src={image.imageUrl}
                      alt={`${product.productDisplayName} - ${image.imageType}`}
                      className="w-full h-full object-contain bg-white dark:bg-gray-800"
                      onError={(e) => {
                        const target = e.target as HTMLImageElement;
                        target.src = `https://picsum.photos/32/32?random=${product.id}-${index}`;
                      }}
                    />
                  </button>
                ))}
                {product.images.length > 3 && (
                  <div className="w-8 h-8 rounded-lg border-2 border-white shadow-medium bg-gray-800 text-white text-xs flex items-center justify-center font-semibold">
                    +{product.images.length - 3}
                  </div>
                )}
              </div>
            </div>
          )}
        </div>
      </Link>

      <div className="p-5">
        <Link to={`/product/${product.id}`} onClick={handleCardClick}>
          <h3 className="text-sm font-semibold text-gray-900 dark:text-white mb-2 hover:text-brand-600 dark:hover:text-brand-400 transition-colors line-clamp-2 leading-tight">
            {product.productDisplayName}
          </h3>
        </Link>

        <div className="flex items-center space-x-1 mb-3">
          <div className="flex items-center">
            {[...Array(5)].map((_, i) => (
              <Star
                key={i}
                className={`w-3.5 h-3.5 ${
                  i < Math.floor(product.rating)
                    ? "text-yellow-400 fill-current"
                    : "text-gray-300 dark:text-gray-600"
                }`}
              />
            ))}
          </div>
          <span className="text-xs text-gray-500 dark:text-gray-400 font-medium">
            ({product.reviews})
          </span>
        </div>

        <div className="flex items-center space-x-2 mb-3">
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
        <div className="space-y-1">
          <div className="text-xs text-gray-500 dark:text-gray-400 font-medium">
            {product.subCategoryName}
            {product.brandName && ` • ${product.brandName}`}
            {product.baseColourName && ` • ${product.baseColourName}`}
            {product.articleTypeName && ` • ${product.articleTypeName}`}
          </div>
        </div>
      </div>
    </div>
  );
}
