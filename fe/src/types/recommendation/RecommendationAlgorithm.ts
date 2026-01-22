export enum RecommendationAlgorithm {
  ContentBasedFull = "ContentBasedFull",
  ContentBasedNoBrand = "ContentBasedNoBrand",
  ContentBasedNoBrandAndAttributes = "ContentBasedNoBrandAndAttributes",
  ContentBasedOnlyDescription = "ContentBasedOnlyDescription",
  CollaborativeFiltering = "CollaborativeFiltering",
  VisualBased = "VisualBased",
}

export interface RecommendationAlgorithmInfo {
  value: RecommendationAlgorithm;
  label: string;
  description: string;
  isAvailable: boolean;
}

export const RECOMMENDATION_ALGORITHMS: RecommendationAlgorithmInfo[] = [
  {
    value: RecommendationAlgorithm.ContentBasedFull,
    label: "Content-Based (Pełny)",
    description: "Rekomendacje bazujące na wszystkich cechach produktu",
    isAvailable: true,
  },
  {
    value: RecommendationAlgorithm.ContentBasedNoBrand,
    label: "Content-Based (Bez marki)",
    description: "Rekomendacje bez uwzględniania marki",
    isAvailable: true,
  },
  {
    value: RecommendationAlgorithm.ContentBasedNoBrandAndAttributes,
    label: "Content-Based (Bez marki i atrybutów)",
    description: "Rekomendacje bazujące tylko na opisie",
    isAvailable: true,
  },
  {
    value: RecommendationAlgorithm.ContentBasedOnlyDescription,
    label: "Content-Based (Tylko opis)",
    description: "Rekomendacje bazujące wyłącznie na opisie tekstowym",
    isAvailable: true,
  },
  {
    value: RecommendationAlgorithm.CollaborativeFiltering,
    label: "Collaborative Filtering",
    description: "Rekomendacje bazujące na preferencjach innych użytkowników",
    isAvailable: true,
  },
  {
    value: RecommendationAlgorithm.VisualBased,
    label: "Visual-Based",
    description: "Rekomendacje bazujące na podobieństwie wizualnym produktów",
    isAvailable: true,
  },
];
