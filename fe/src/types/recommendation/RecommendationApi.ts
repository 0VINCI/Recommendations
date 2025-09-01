import type { ProductDto } from "../product/ProductDto";
import type { RecommendationAlgorithm } from "./RecommendationAlgorithm";

// Get Similar Products
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

// Get Product Embedding
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

// Get All Product Embeddings
export interface GetAllProductEmbeddingsRequest {
  productId: string;
}

export interface GetAllProductEmbeddingsResponse {
  embeddings: GetProductEmbeddingResponse[];
}

// Backend Similar Product DTO
export interface BackendSimilarProductDto {
  productId: string;
  similarityScore: number;
}
