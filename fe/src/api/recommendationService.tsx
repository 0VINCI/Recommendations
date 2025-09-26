import { get } from "./client/httpClient.tsx";
import type { ApiResult } from "../types/api/ApiResult.tsx";
import type { ProductDto } from "../types/product/ProductDto";
import type {
  GetSimilarProductsRequest,
  GetSimilarProductsResponse,
  GetProductEmbeddingRequest,
  GetProductEmbeddingResponse,
  GetAllProductEmbeddingsRequest,
  GetAllProductEmbeddingsResponse,
} from "../types/recommendation/RecommendationApi";

const modulePrefix = "/content-based";

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

export const getSimilarProducts = async (
  request: GetSimilarProductsRequest
): Promise<ApiResult<GetSimilarProductsResponse>> => {
  const vectorType = mapAlgorithmToVectorType(request.algorithm);
  const topCount = request.topCount || 10;

  try {
    const response = await get<ProductDto[]>(
      `${modulePrefix}/product-embeddings/${request.productId}/${vectorType}/similar?topCount=${topCount}`
    );

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
    `${modulePrefix}/product-embeddings/${request.productId}/${vectorType}`
  );
};

export const getAllProductEmbeddings = async (
  request: GetAllProductEmbeddingsRequest
): Promise<ApiResult<GetAllProductEmbeddingsResponse>> => {
  return await get<GetAllProductEmbeddingsResponse>(
    `${modulePrefix}/product-embeddings/${request.productId}`
  );
};
