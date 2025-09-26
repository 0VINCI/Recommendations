import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import {
  ChevronLeft,
  ChevronRight,
  ArrowRight,
  Star,
  TrendingUp,
  Sparkles,
} from "lucide-react";

interface BannerData {
  id: number;
  title: string;
  subtitle: string;
  description: string;
  ctaText: string;
  ctaLink: string;
  backgroundImage: string;
  badge?: {
    text: string;
    icon: React.ReactNode;
    color: string;
  };
  overlay: string;
}

const banners: BannerData[] = [
  {
    id: 1,
    title: "Sprzęt Sportowy",
    subtitle: "Aktywny Styl Życia",
    description:
      "Odkryj najlepszy sprzęt sportowy dla każdej dyscypliny. Jakość, która wspiera Twoje cele fitness.",
    ctaText: "Zobacz Sprzęt Sportowy",
    ctaLink: "/category/sports%20equipment?page=1&pageSize=20",
    backgroundImage:
      "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?auto=format&fit=crop&w=1920&q=80",
    badge: {
      text: "Sport",
      icon: <TrendingUp className="w-4 h-4" />,
      color: "from-green-500 to-green-600",
    },
    overlay: "from-green-900/70 via-green-800/60 to-green-900/70",
  },
  {
    id: 2,
    title: "Odzież Domowa",
    subtitle: "Komfort w Domu",
    description:
      "Najwygodniejsze ubrania do domu i na noc. Luksusowy komfort dla każdego dnia.",
    ctaText: "Odzież Domowa",
    ctaLink: "/category/loungewear%20and%20nightwear?page=1&pageSize=20",
    backgroundImage:
      "https://images.unsplash.com/photo-1556905055-8f358a7a47b2?auto=format&fit=crop&w=1920&q=80",
    badge: {
      text: "Komfort",
      icon: <Star className="w-4 h-4" />,
      color: "from-purple-500 to-purple-600",
    },
    overlay: "from-purple-900/70 via-purple-800/60 to-purple-900/70",
  },
  {
    id: 3,
    title: "Akcesoria do Paznokci",
    subtitle: "Piękne Dłonie",
    description:
      "Wszystko czego potrzebujesz do perfekcyjnego manicure. Profesjonalne akcesoria i kosmetyki.",
    ctaText: "Zobacz Akcesoria",
    ctaLink: "/category/nails?page=1&pageSize=20",
    backgroundImage:
      "https://images.unsplash.com/photo-1604654894610-df63bc536371?auto=format&fit=crop&w=1920&q=80",
    badge: {
      text: "Beauty",
      icon: <Sparkles className="w-4 h-4" />,
      color: "from-pink-500 to-pink-600",
    },
    overlay: "from-pink-900/70 via-pink-800/60 to-pink-900/70",
  },
  {
    id: 4,
    title: "Okulary i Soczewki",
    subtitle: "Widzę Styl",
    description:
      "Nowoczesne okulary i soczewki kontaktowe. Połącz funkcjonalność z modnym designem.",
    ctaText: "Zobacz Okulary",
    ctaLink: "/category/eyewear?page=1&pageSize=20",
    backgroundImage:
      "https://images.unsplash.com/photo-1506629905607-1b2a0a0b0b0b?auto=format&fit=crop&w=1920&q=80",
    badge: {
      text: "Wizja",
      icon: <Star className="w-4 h-4" />,
      color: "from-blue-500 to-blue-600",
    },
    overlay: "from-blue-900/70 via-blue-800/60 to-blue-900/70",
  },
  {
    id: 5,
    title: "Sandały",
    subtitle: "Lato w Stylu",
    description:
      "Eleganckie i wygodne sandały na każdą okazję. Od plaży po miasto - znajdź swój idealny styl.",
    ctaText: "Zobacz Sandały",
    ctaLink: "/category/sandal?page=1&pageSize=20",
    backgroundImage:
      "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?auto=format&fit=crop&w=1920&q=80",
    badge: {
      text: "Lato",
      icon: <Star className="w-4 h-4" />,
      color: "from-orange-500 to-orange-600",
    },
    overlay: "from-orange-900/70 via-orange-800/60 to-orange-900/70",
  },
];

export function RotatingBanners() {
  const [currentBanner, setCurrentBanner] = useState(0);
  const [isAutoPlaying, setIsAutoPlaying] = useState(true);

  useEffect(() => {
    if (!isAutoPlaying) return;

    const interval = setInterval(() => {
      setCurrentBanner((prev) => (prev + 1) % banners.length);
    }, 5000);

    return () => clearInterval(interval);
  }, [isAutoPlaying]);

  const goToNext = () => {
    setCurrentBanner((prev) => (prev + 1) % banners.length);
    setIsAutoPlaying(false);
  };

  const goToPrevious = () => {
    setCurrentBanner((prev) => (prev - 1 + banners.length) % banners.length);
    setIsAutoPlaying(false);
  };

  const goToSlide = (index: number) => {
    setCurrentBanner(index);
    setIsAutoPlaying(false);
  };

  const currentBannerData = banners[currentBanner];

  return (
    <section className="relative h-[500px] overflow-hidden rounded-2xl mx-4 sm:mx-6 lg:mx-8 mt-4 shadow-strong">
      {/* Background Image */}
      <div
        className="absolute inset-0 bg-cover bg-center bg-no-repeat transition-all duration-1000 ease-in-out"
        style={{
          backgroundImage: `url(${currentBannerData.backgroundImage})`,
        }}
      />

      {/* Overlay */}
      <div
        className={`absolute inset-0 bg-gradient-to-br ${currentBannerData.overlay}`}
      />

      {/* Content */}
      <div className="relative z-10 h-full flex items-center">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 w-full">
          <div className="max-w-2xl">
            {/* Badge */}
            {currentBannerData.badge && (
              <div className="mb-4">
                <span
                  className={`inline-flex items-center space-x-2 bg-gradient-to-r ${currentBannerData.badge.color} text-white text-sm px-4 py-2 rounded-full font-semibold shadow-medium`}
                >
                  {currentBannerData.badge.icon}
                  <span>{currentBannerData.badge.text}</span>
                </span>
              </div>
            )}

            {/* Title */}
            <h1 className="text-4xl md:text-6xl font-bold text-white mb-2 leading-tight">
              {currentBannerData.title}
            </h1>

            {/* Subtitle */}
            <h2 className="text-xl md:text-2xl text-gray-200 mb-4 font-medium">
              {currentBannerData.subtitle}
            </h2>

            {/* Description */}
            <p className="text-lg text-gray-200 mb-8 max-w-xl leading-relaxed">
              {currentBannerData.description}
            </p>

            {/* CTA Button */}
            <Link
              to={currentBannerData.ctaLink}
              className="inline-flex items-center bg-white text-gray-900 px-8 py-4 rounded-2xl font-semibold hover:bg-gray-100 transition-all duration-300 shadow-medium hover:shadow-strong hover:scale-105"
            >
              {currentBannerData.ctaText}
              <ArrowRight className="ml-2 w-5 h-5" />
            </Link>
          </div>
        </div>
      </div>

      {/* Navigation Arrows */}
      <button
        onClick={goToPrevious}
        className="absolute left-4 top-1/2 transform -translate-y-1/2 z-20 p-3 bg-white/20 hover:bg-white/30 backdrop-blur-sm rounded-full text-white transition-all duration-200 hover:scale-110"
        aria-label="Poprzedni baner"
      >
        <ChevronLeft className="w-6 h-6" />
      </button>

      <button
        onClick={goToNext}
        className="absolute right-4 top-1/2 transform -translate-y-1/2 z-20 p-3 bg-white/20 hover:bg-white/30 backdrop-blur-sm rounded-full text-white transition-all duration-200 hover:scale-110"
        aria-label="Następny baner"
      >
        <ChevronRight className="w-6 h-6" />
      </button>

      {/* Dots Indicator */}
      <div className="absolute bottom-6 left-1/2 transform -translate-x-1/2 z-20 flex space-x-2">
        {banners.map((_, index) => (
          <button
            key={index}
            onClick={() => goToSlide(index)}
            className={`w-3 h-3 rounded-full transition-all duration-200 ${
              index === currentBanner
                ? "bg-white scale-125"
                : "bg-white/50 hover:bg-white/75"
            }`}
            aria-label={`Przejdź do banera ${index + 1}`}
          />
        ))}
      </div>

      {/* Progress Bar */}
      <div className="absolute bottom-0 left-0 h-1 bg-white/20 w-full">
        <div
          className="h-full bg-white transition-all duration-100 ease-linear"
          style={{
            width: isAutoPlaying
              ? `${((Date.now() % 5000) / 5000) * 100}%`
              : "0%",
          }}
        />
      </div>
    </section>
  );
}
