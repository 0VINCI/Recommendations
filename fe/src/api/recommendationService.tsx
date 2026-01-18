import { get } from "./client/httpClient.tsx";
import type { ApiResult } from "../types/api/ApiResult.tsx";
import type { ProductDto } from "../types/product/ProductDto";
import { EmbeddingSource } from "../types/recommendation/EmbeddingSource";
import { RecommendationAlgorithm } from "../types/recommendation/RecommendationAlgorithm";
import type {
  GetSimilarProductsRequest,
  GetSimilarProductsResponse,
  GetProductEmbeddingRequest,
  GetProductEmbeddingResponse,
  GetAllProductEmbeddingsRequest,
  GetAllProductEmbeddingsResponse,
} from "../types/recommendation/RecommendationApi";

const contentBasedPrefix = "/content-based";
const trackingPrefix = "/tracking";

const mapAlgorithmToVectorType = (algorithm: string): string => {
  switch (algorithm) {
    case "ContentBasedFull":
      return "Full";
    case "ContentBasedNoBrand":
      return "NoBrand";
    case "ContentBasedNoBrandAndAttributes":
      return "NoBrandAndAttributes";
    case "ContentBasedOnlyDescription":
      return "OnlyDescription";
    default:
      return "Full";
  }
};

const isCollaborativeFiltering = (algorithm: string): boolean => {
  return algorithm === RecommendationAlgorithm.CollaborativeFiltering;
};

export const getSimilarProducts = async (
  request: GetSimilarProductsRequest
): Promise<ApiResult<GetSimilarProductsResponse>> => {
  const topCount = request.topCount || 10;

  try {
    let response: ApiResult<ProductDto[]>;

    if (isCollaborativeFiltering(request.algorithm)) {

      response = await get<ProductDto[]>(
        `${trackingPrefix}/cf/similar-items/${request.productId}/products?topCount=${topCount}`
      );
    } else {

      const vectorType = mapAlgorithmToVectorType(request.algorithm);
      const source = request.embeddingSource ?? EmbeddingSource.New;
      response = await get<ProductDto[]>(
        `${contentBasedPrefix}/product-embeddings/${request.productId}/${vectorType}/similar/${source}?topCount=${topCount}`
      );
    }

    if (response.status !== 200 || !response.data) {
      return {
        status: response.status,
        message: response.message || "Nie udało się pobrać podobnych produktów",
      };
    }

    return {
      status: 200,
      data: {
        products: response.data,
        algorithm: request.algorithm,
        productId: request.productId,
      },
      message: "Success",
    };
  } catch {
    return {
      status: 500,
      message: "Błąd podczas pobierania podobnych produktów",
    };
  }
};

export const getProductEmbedding = async (
  request: GetProductEmbeddingRequest
): Promise<ApiResult<GetProductEmbeddingResponse>> => {
  const vectorType = mapAlgorithmToVectorType(request.algorithm);

  return await get<GetProductEmbeddingResponse>(
    `${contentBasedPrefix}/product-embeddings/${request.productId}/${vectorType}`
  );
};

export const getAllProductEmbeddings = async (
  request: GetAllProductEmbeddingsRequest
): Promise<ApiResult<GetAllProductEmbeddingsResponse>> => {
  return await get<GetAllProductEmbeddingsResponse>(
    `${contentBasedPrefix}/product-embeddings/${request.productId}`
  );
};

export const getRecommendationsForUser = async (
  userId: string,
  topCount: number = 8
): Promise<ApiResult<ProductDto[]>> => {
  try {
    const response = await get<ProductDto[]>(
      `${trackingPrefix}/cf/for-user/${userId}/products?topCount=${topCount}`
    );

    if (response.status === 404) {

      return {
        status: 404,
        message: "Brak spersonalizowanych rekomendacji",
      };
    }

    return response;
  } catch {
    return {
      status: 500,
      message: "Błąd podczas pobierania rekomendacji",
    };
  }
};
