import { useState, useRef, useEffect } from "react";
import { ChevronDown, Check } from "lucide-react";
import { useApp } from "../../context/useApp";
import {
  RECOMMENDATION_ALGORITHMS,
  RecommendationAlgorithm,
} from "../../types/recommendation/RecommendationAlgorithm";

export function RecommendationAlgorithmSelector() {
  const { state, dispatch } = useApp();
  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(event.target as Node)
      ) {
        setIsOpen(false);
      }
    }

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const currentAlgorithm = RECOMMENDATION_ALGORITHMS.find(
    (alg) => alg.value === state.selectedRecommendationAlgorithm
  );

  const availableAlgorithms = RECOMMENDATION_ALGORITHMS.filter(
    (alg) => alg.isAvailable
  );

  const handleAlgorithmSelect = (algorithm: RecommendationAlgorithm) => {
    dispatch({ type: "SET_RECOMMENDATION_ALGORITHM", payload: algorithm });
    setIsOpen(false);
  };

  return (
    <div className="relative" ref={dropdownRef}>
      <button
        onClick={() => setIsOpen(!isOpen)}
        className="flex items-center space-x-2 px-3 py-2 text-sm text-gray-700 dark:text-gray-300 hover:text-gray-900 dark:hover:text-white transition-colors rounded-md hover:bg-gray-100 dark:hover:bg-gray-700"
        title="Wybierz algorytm rekomendacji"
      >
        <span className="hidden sm:block">Algorytm:</span>
        <span className="font-medium">{currentAlgorithm?.label}</span>
        <ChevronDown
          className={`w-4 h-4 transition-transform ${
            isOpen ? "rotate-180" : ""
          }`}
        />
      </button>

      {isOpen && (
        <div className="absolute right-0 mt-2 w-80 bg-white dark:bg-gray-800 rounded-md shadow-lg py-1 z-50 border border-gray-200 dark:border-gray-700">
          <div className="px-3 py-2 text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wide">
            Dostępne algorytmy
          </div>

          {availableAlgorithms.map((algorithm) => (
            <button
              key={algorithm.value}
              onClick={() => handleAlgorithmSelect(algorithm.value)}
              className="flex items-center justify-between w-full px-3 py-2 text-sm text-gray-700 dark:text-gray-300 hover:bg-gray-100 dark:hover:bg-gray-700 transition-colors"
            >
              <div className="flex-1 text-left">
                <div className="font-medium">{algorithm.label}</div>
                <div className="text-xs text-gray-500 dark:text-gray-400 mt-1">
                  {algorithm.description}
                </div>
              </div>
              {state.selectedRecommendationAlgorithm === algorithm.value && (
                <Check className="w-4 h-4 text-primary-600" />
              )}
            </button>
          ))}

          <div className="border-t border-gray-200 dark:border-gray-700 mt-2 pt-2">
            <div className="px-3 py-2 text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wide">
              Niedostępne algorytmy
            </div>

            {RECOMMENDATION_ALGORITHMS.filter((alg) => !alg.isAvailable).map(
              (algorithm) => (
                <div
                  key={algorithm.value}
                  className="flex items-center justify-between w-full px-3 py-2 text-sm text-gray-400 dark:text-gray-500 cursor-not-allowed"
                >
                  <div className="flex-1">
                    <div className="font-medium">{algorithm.label}</div>
                    <div className="text-xs text-gray-400 dark:text-gray-500 mt-1">
                      {algorithm.description}
                    </div>
                  </div>
                  <span className="text-xs bg-gray-200 dark:bg-gray-700 text-gray-500 dark:text-gray-400 px-2 py-1 rounded">
                    Wkrótce
                  </span>
                </div>
              )
            )}
          </div>
        </div>
      )}
    </div>
  );
}
