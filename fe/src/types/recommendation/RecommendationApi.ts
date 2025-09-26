import type { ProductDto } from "../product/ProductDto";
import type { RecommendationAlgorithm } from "./RecommendationAlgorithm";

export interface GetSimilarProductsRequest {
  productId: string;
  algorithm: RecommendationAlgorithm;
  topCount?: number;
}

export interface GetSimilarProductsResponse {
  products: ProductDto[];
  algorithm: RecommendationAlgorithm;
  productId: string;
}

export interface GetProductEmbeddingRequest {
  productId: string;
  algorithm: RecommendationAlgorithm;
}

export interface GetProductEmbeddingResponse {
  productId: string;
  variant: string;
  embedding: number[];
  createdAt: string;
  updatedAt?: string;
}

export interface GetAllProductEmbeddingsRequest {
  productId: string;
}

export interface GetAllProductEmbeddingsResponse {
  embeddings: GetProductEmbeddingResponse[];
}

export interface BackendSimilarProductDto {
  productId: string;
  similarityScore: number;
}
