import React, { useState } from "react";
import { useParams, Link } from "react-router-dom";
import {
  ArrowLeft,
  Star,
  Heart,
  ShoppingCart,
  Truck,
  Shield,
  RotateCcw,
} from "lucide-react";
import { useApp } from "../context/useApp";

export function ProductPage() {
  const { id } = useParams<{ id: string }>();
  const { state, dispatch } = useApp();
  const [quantity, setQuantity] = useState(1);

  const product = state.products.find((p) => p.id === id);

  if (!product) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center">
        <div className="text-center">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-4">
            Produkt nie został znaleziony
          </h2>
          <Link
            to="/"
            className="text-primary-600 dark:text-primary-400 hover:underline"
          >
            Wróć do strony głównej
          </Link>
        </div>
      </div>
    );
  }

  const addToCart = () => {
    for (let i = 0; i < quantity; i++) {
      dispatch({
        type: "ADD_TO_CART",
        payload: {
          product,
          size: "",
          color: "",
        },
      });
    }

    alert("Produkt został dodany do koszyka!");
  };

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Breadcrumb */}
        <div className="flex items-center space-x-2 mb-8">
          <Link
            to="/"
            className="flex items-center text-gray-600 dark:text-gray-400 hover:text-gray-900 dark:hover:text-white"
          >
            <ArrowLeft className="w-4 h-4 mr-1" />
            Powrót
          </Link>
          <span className="text-gray-400 dark:text-gray-600">/</span>
          <span className="text-gray-900 dark:text-white">{product.name}</span>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
          {/* Product Image */}
          <div className="space-y-4">
            <div className="aspect-square bg-white dark:bg-gray-800 rounded-lg overflow-hidden">
              <img
                src={product.image}
                alt={product.name}
                className="w-full h-full object-cover"
              />
            </div>
          </div>

          {/* Product Info */}
          <div className="space-y-6">
            <div>
              <h1 className="text-3xl font-bold text-gray-900 dark:text-white mb-2">
                {product.name}
              </h1>

              <div className="flex items-center space-x-4 mb-4">
                <div className="flex items-center space-x-1">
                  {[...Array(5)].map((_, i) => (
                    <Star
                      key={i}
                      className={`w-5 h-5 ${
                        i < Math.floor(product.rating)
                          ? "text-yellow-400 fill-current"
                          : "text-gray-300 dark:text-gray-600"
                      }`}
                    />
                  ))}
                </div>
                <span className="text-sm text-gray-600 dark:text-gray-400">
                  {product.rating} ({product.reviews} opinii)
                </span>
              </div>

              <div className="flex items-center space-x-4 mb-6">
                <span className="text-3xl font-bold text-gray-900 dark:text-white">
                  {product.price.toFixed(2)} zł
                </span>
                {product.originalPrice && (
                  <span className="text-xl text-gray-500 dark:text-gray-400 line-through">
                    {product.originalPrice.toFixed(2)} zł
                  </span>
                )}
              </div>
            </div>

            <div>
              <p className="text-gray-600 dark:text-gray-300 leading-relaxed">
                {product.description}
              </p>
            </div>

            {/* Product Details */}
            <div className="space-y-3">
              <div className="flex justify-between">
                <span className="text-gray-600 dark:text-gray-400">
                  Kategoria:
                </span>
                <span className="text-gray-900 dark:text-white">
                  {product.category}
                </span>
              </div>
              {product.gender && (
                <div className="flex justify-between">
                  <span className="text-gray-600 dark:text-gray-400">
                    Płeć:
                  </span>
                  <span className="text-gray-900 dark:text-white">
                    {product.gender}
                  </span>
                </div>
              )}
              {product.subCategory && (
                <div className="flex justify-between">
                  <span className="text-gray-600 dark:text-gray-400">
                    Podkategoria:
                  </span>
                  <span className="text-gray-900 dark:text-white">
                    {product.subCategory}
                  </span>
                </div>
              )}
              {product.articleType && (
                <div className="flex justify-between">
                  <span className="text-gray-600 dark:text-gray-400">Typ:</span>
                  <span className="text-gray-900 dark:text-white">
                    {product.articleType}
                  </span>
                </div>
              )}
              {product.baseColour && (
                <div className="flex justify-between">
                  <span className="text-gray-600 dark:text-gray-400">
                    Kolor:
                  </span>
                  <span className="text-gray-900 dark:text-white">
                    {product.baseColour}
                  </span>
                </div>
              )}
              {product.season && (
                <div className="flex justify-between">
                  <span className="text-gray-600 dark:text-gray-400">
                    Sezon:
                  </span>
                  <span className="text-gray-900 dark:text-white">
                    {product.season}
                  </span>
                </div>
              )}
              {product.year && (
                <div className="flex justify-between">
                  <span className="text-gray-600 dark:text-gray-400">Rok:</span>
                  <span className="text-gray-900 dark:text-white">
                    {product.year}
                  </span>
                </div>
              )}
            </div>

            {/* Quantity */}
            <div>
              <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-3">
                Ilość
              </h3>
              <div className="flex items-center space-x-3">
                <button
                  onClick={() => setQuantity(Math.max(1, quantity - 1))}
                  className="w-10 h-10 border border-gray-300 dark:border-gray-600 rounded-md flex items-center justify-center hover:bg-gray-50 dark:hover:bg-gray-700"
                >
                  -
                </button>
                <span className="text-lg font-medium text-gray-900 dark:text-white w-8 text-center">
                  {quantity}
                </span>
                <button
                  onClick={() => setQuantity(quantity + 1)}
                  className="w-10 h-10 border border-gray-300 dark:border-gray-600 rounded-md flex items-center justify-center hover:bg-gray-50 dark:hover:bg-gray-700"
                >
                  +
                </button>
              </div>
            </div>

            {/* Actions */}
            <div className="flex space-x-4">
              <button
                onClick={addToCart}
                className="flex-1 bg-primary-600 hover:bg-primary-700 text-white py-3 px-6 rounded-lg font-medium flex items-center justify-center space-x-2 transition-colors"
              >
                <ShoppingCart className="w-5 h-5" />
                <span>Dodaj do koszyka</span>
              </button>
              <button className="p-3 border border-gray-300 dark:border-gray-600 rounded-lg hover:bg-gray-50 dark:hover:bg-gray-700 transition-colors">
                <Heart className="w-5 h-5 text-gray-600 dark:text-gray-300" />
              </button>
            </div>

            {/* Features */}
            <div className="border-t border-gray-200 dark:border-gray-700 pt-6">
              <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                <div className="flex items-center space-x-2">
                  <Truck className="w-5 h-5 text-primary-600" />
                  <span className="text-sm text-gray-600 dark:text-gray-300">
                    Darmowa dostawa
                  </span>
                </div>
                <div className="flex items-center space-x-2">
                  <RotateCcw className="w-5 h-5 text-primary-600" />
                  <span className="text-sm text-gray-600 dark:text-gray-300">
                    30 dni na zwrot
                  </span>
                </div>
                <div className="flex items-center space-x-2">
                  <Shield className="w-5 h-5 text-primary-600" />
                  <span className="text-sm text-gray-600 dark:text-gray-300">
                    Gwarancja jakości
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
