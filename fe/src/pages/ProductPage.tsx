import { useState, useEffect } from "react";
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
import { useProducts } from "../hooks/useProducts";
import { useCart } from "../hooks/useCart";
import { Loader } from "../components/common/Loader";
import { SimilarProducts } from "../components/SimilarProducts";

// Funkcja do parsowania HTML opisu produktu
function parseProductDescription(description: string) {
  if (!description) return null;

  // Usuń tagi <p> i </p> z początku i końca
  const cleanDescription = description.replace(/^<p>|<\/p>$/g, "");

  // Podziel na sekcje na podstawie <strong> tagów
  const sections: { title: string; content: string }[] = [];

  // Znajdź wszystkie sekcje z <strong> tagami - prostsze podejście
  const strongMatches = cleanDescription.match(
    /<strong>([^<]+)(?:<br\s*\/?>\s*)?<\/strong>/g
  );

  if (strongMatches) {
    for (let i = 0; i < strongMatches.length; i++) {
      const currentMatch = strongMatches[i];
      const titleMatch = currentMatch.match(
        /<strong>([^<]+)(?:<br\s*\/?>\s*)?<\/strong>/
      );

      if (titleMatch) {
        const title = titleMatch[1].trim();

        // Znajdź treść między obecnym <strong> a następnym <strong> lub <em> lub końcem
        const currentIndex = cleanDescription.indexOf(currentMatch);
        const nextStrongIndex = cleanDescription.indexOf(
          "<strong>",
          currentIndex + currentMatch.length
        );
        const nextEmIndex = cleanDescription.indexOf(
          "<em>",
          currentIndex + currentMatch.length
        );

        let endIndex = cleanDescription.length;
        if (
          nextStrongIndex !== -1 &&
          (nextEmIndex === -1 || nextStrongIndex < nextEmIndex)
        ) {
          endIndex = nextStrongIndex;
        } else if (nextEmIndex !== -1) {
          endIndex = nextEmIndex;
        }

        let content = cleanDescription
          .substring(currentIndex + currentMatch.length, endIndex)
          .trim();

        // Usuń HTML tagi z content, ale zachowaj podziały linii
        content = content
          .replace(/<br\s*\/?>/gi, "\n") // Zamień <br> na nowe linie
          .replace(/<[^>]*>/g, "") // Usuń pozostałe HTML tagi
          .replace(/\n\s*\n/g, "\n") // Usuń podwójne nowe linie
          .trim();

        if (content) {
          sections.push({ title, content });
        }
      }
    }
  }

  // Znajdź sekcje z <em> tagami (jak "Model statistics")
  const emMatches = cleanDescription.match(/<em>([^<]+)<\/em>/g);

  if (emMatches) {
    for (let i = 0; i < emMatches.length; i++) {
      const currentMatch = emMatches[i];
      const titleMatch = currentMatch.match(/<em>([^<]+)<\/em>/);

      if (titleMatch) {
        const title = titleMatch[1].trim();

        // Znajdź treść między obecnym <em> a następnym <strong> lub <em> lub końcem
        const currentIndex = cleanDescription.indexOf(currentMatch);
        const nextStrongIndex = cleanDescription.indexOf(
          "<strong>",
          currentIndex + currentMatch.length
        );
        const nextEmIndex = cleanDescription.indexOf(
          "<em>",
          currentIndex + currentMatch.length
        );

        let endIndex = cleanDescription.length;
        if (
          nextStrongIndex !== -1 &&
          (nextEmIndex === -1 || nextStrongIndex < nextEmIndex)
        ) {
          endIndex = nextStrongIndex;
        } else if (nextEmIndex !== -1) {
          endIndex = nextEmIndex;
        }

        let content = cleanDescription
          .substring(currentIndex + currentMatch.length, endIndex)
          .trim();

        // Usuń HTML tagi z content, ale zachowaj podziały linii
        content = content
          .replace(/<br\s*\/?>/gi, "\n")
          .replace(/<[^>]*>/g, "")
          .replace(/\n\s*\n/g, "\n")
          .trim();

        if (content) {
          sections.push({ title, content });
        }
      }
    }
  }

  // Jeśli nie ma sekcji z <strong> lub <em>, traktuj całość jako zwykły opis
  if (sections.length === 0) {
    const cleanContent = cleanDescription
      .replace(/<br\s*\/?>/gi, "\n")
      .replace(/<[^>]*>/g, "")
      .replace(/\n\s*\n/g, "\n")
      .trim();
    return [{ title: "Opis", content: cleanContent }];
  }

  return sections;
}

export function ProductPage() {
  const { id } = useParams<{ id: string }>();
  const { currentProduct, loading, error, getProductById } = useProducts();
  const { addToCart } = useCart();
  const [quantity, setQuantity] = useState(1);
  const [currentImageIndex, setCurrentImageIndex] = useState(0);

  useEffect(() => {
    if (id) {
      getProductById(id);
    }
  }, [id, getProductById]);

  // Reset image index when product changes
  useEffect(() => {
    setCurrentImageIndex(0);
  }, [currentProduct?.id]);

  const product = currentProduct;

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center">
        <Loader />
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-gray-900 flex items-center justify-center">
        <div className="text-center">
          <h2 className="text-2xl font-bold text-gray-900 dark:text-white mb-4">
            Błąd: {error}
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

  const handleAddToCart = () => {
    if (!product) return;

    for (let i = 0; i < quantity; i++) {
      addToCart(product);
    }
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
          <span className="text-gray-900 dark:text-white">
            {product.productDisplayName}
          </span>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
          {/* Product Image */}
          <div className="space-y-4">
            <div className="aspect-square bg-white dark:bg-gray-800 rounded-lg overflow-hidden">
              <img
                src={(() => {
                  const currentImage =
                    product.images?.[currentImageIndex] ||
                    product.images?.find((img) => img.isPrimary) ||
                    product.images?.[0];
                  return (
                    currentImage?.imageUrl ||
                    `https://via.placeholder.com/600x600/cccccc/666666?text=${encodeURIComponent(
                      product.productDisplayName
                    )}`
                  );
                })()}
                alt={product.productDisplayName}
                className="w-full h-full object-contain"
                onError={(e) => {
                  const target = e.target as HTMLImageElement;
                  target.src = `https://via.placeholder.com/600x600/cccccc/666666?text=${encodeURIComponent(
                    product.productDisplayName
                  )}`;
                }}
              />
            </div>

            {/* Image Gallery */}
            {product.images && product.images.length > 1 && (
              <div className="flex space-x-2 overflow-x-auto">
                {product.images.map((image, index) => (
                  <button
                    key={index}
                    onClick={() => setCurrentImageIndex(index)}
                    className={`w-20 h-20 rounded border-2 overflow-hidden flex-shrink-0 transition-all duration-200 hover:scale-105 ${
                      currentImageIndex === index
                        ? "border-primary-500 ring-2 ring-primary-300"
                        : "border-gray-300 dark:border-gray-600 hover:border-primary-300"
                    }`}
                  >
                    <img
                      src={image.imageUrl}
                      alt={`${product.productDisplayName} - ${image.imageType}`}
                      className="w-full h-full object-contain bg-white dark:bg-gray-800"
                      onError={(e) => {
                        const target = e.target as HTMLImageElement;
                        target.src = `https://via.placeholder.com/80x80/cccccc/666666?text=${
                          index + 1
                        }`;
                      }}
                    />
                  </button>
                ))}
              </div>
            )}
          </div>

          {/* Product Info */}
          <div className="space-y-6">
            <div>
              <h1 className="text-3xl font-bold text-gray-900 dark:text-white mb-2">
                {product.productDisplayName}
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
                {product.brandName} - {product.productDisplayName}
              </p>
            </div>

            {/* Product Details */}
            <div className="space-y-3">
              <div className="flex justify-between">
                <span className="text-gray-600 dark:text-gray-400">
                  Kategoria:
                </span>
                <span className="text-gray-900 dark:text-white">
                  {product.subCategoryName}
                </span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600 dark:text-gray-400">Marka:</span>
                <span className="text-gray-900 dark:text-white">
                  {product.brandName}
                </span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600 dark:text-gray-400">
                  Typ artykułu:
                </span>
                <span className="text-gray-900 dark:text-white">
                  {product.articleTypeName}
                </span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600 dark:text-gray-400">Kolor:</span>
                <span className="text-gray-900 dark:text-white">
                  {product.baseColourName}
                </span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600 dark:text-gray-400">Ocena:</span>
                <span className="text-gray-900 dark:text-white">
                  {product.rating}/5 ({product.reviews} opinii)
                </span>
              </div>
              {product.isBestseller && (
                <div className="flex justify-between">
                  <span className="text-gray-600 dark:text-gray-400">
                    Status:
                  </span>
                  <span className="text-gray-900 dark:text-white">
                    Bestseller
                  </span>
                </div>
              )}
              {product.isNew && (
                <div className="flex justify-between">
                  <span className="text-gray-600 dark:text-gray-400">
                    Status:
                  </span>
                  <span className="text-gray-900 dark:text-white">Nowość</span>
                </div>
              )}
            </div>

            {/* Additional Product Details */}
            {product.details && (
              <div className="border-t border-gray-200 dark:border-gray-700 pt-6">
                <h3 className="text-lg font-medium text-gray-900 dark:text-white mb-4">
                  Szczegóły produktu
                </h3>
                <div className="space-y-3">
                  {product.details.gender && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Płeć:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.gender}
                      </span>
                    </div>
                  )}
                  {product.details.season && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Sezon:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.season}
                      </span>
                    </div>
                  )}
                  {product.details.usage && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Użycie:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.usage}
                      </span>
                    </div>
                  )}
                  {product.details.year && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Rok:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.year}
                      </span>
                    </div>
                  )}
                  {product.details.sleeveLength && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Długość rękawa:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.sleeveLength}
                      </span>
                    </div>
                  )}
                  {product.details.fit && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Fason:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.fit}
                      </span>
                    </div>
                  )}
                  {product.details.fabric && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Materiał:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.fabric}
                      </span>
                    </div>
                  )}
                  {product.details.collar && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Kołnierz:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.collar}
                      </span>
                    </div>
                  )}
                  {product.details.pattern && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Wzór:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.pattern}
                      </span>
                    </div>
                  )}
                  {product.details.ageGroup && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Grupa wiekowa:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.ageGroup}
                      </span>
                    </div>
                  )}
                  {product.details.bodyOrGarmentSize && (
                    <div className="flex justify-between">
                      <span className="text-gray-600 dark:text-gray-400">
                        Rozmiar:
                      </span>
                      <span className="text-gray-900 dark:text-white">
                        {product.details.bodyOrGarmentSize}
                      </span>
                    </div>
                  )}
                  {product.details.description && (
                    <div className="border-t border-gray-200 dark:border-gray-700 pt-4 mt-4">
                      <h4 className="text-md font-medium text-gray-900 dark:text-white mb-3">
                        Szczegółowy opis
                      </h4>
                      {(() => {
                        const descriptionSections = parseProductDescription(
                          product.details.description
                        );

                        // Debug - sprawdź co się dzieje
                        console.log(
                          "Original description:",
                          product.details.description
                        );
                        console.log("Parsed sections:", descriptionSections);

                        if (!descriptionSections) return null;

                        return (
                          <div className="space-y-4">
                            {descriptionSections.map((section, index) => (
                              <div
                                key={index}
                                className="bg-gray-50 dark:bg-gray-800 rounded-lg p-4"
                              >
                                <h5 className="font-semibold text-gray-900 dark:text-white mb-2 capitalize">
                                  {section.title}
                                </h5>
                                <p className="text-gray-600 dark:text-gray-300 leading-relaxed text-sm whitespace-pre-line">
                                  {section.content}
                                </p>
                              </div>
                            ))}
                          </div>
                        );
                      })()}

                      {/* Debug - pokaż surowy HTML */}
                      <details className="mt-4">
                        <summary className="cursor-pointer text-sm text-gray-500 hover:text-gray-700">
                          Debug - Pokaż surowy HTML
                        </summary>
                        <div className="mt-2 p-3 bg-gray-100 dark:bg-gray-800 rounded text-xs">
                          <pre className="whitespace-pre-wrap overflow-x-auto">
                            {product.details.description}
                          </pre>
                        </div>
                      </details>
                    </div>
                  )}
                </div>
              </div>
            )}

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
                onClick={handleAddToCart}
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

      {/* Similar Products */}
      <SimilarProducts
        productId={product.id}
        currentProductName={product.productDisplayName}
      />
    </div>
  );
}
