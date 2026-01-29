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
const visualPrefix = "/visual";

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

const isVisualBased = (algorithm: string): boolean => {
  return algorithm === RecommendationAlgorithm.VisualBased;
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
    } else if (isVisualBased(request.algorithm)) {

      response = await get<ProductDto[]>(
        `${visualPrefix}/similar-items/${request.productId}/products?topCount=${topCount}`
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

// Visual-Based: podobne wizualnie produkty
export const getVisualSimilarProducts = async (
  productId: string,
  topCount: number = 8
): Promise<ApiResult<ProductDto[]>> => {
  try {
    const response = await get<ProductDto[]>(
      `${visualPrefix}/similar-items/${productId}/products?topCount=${topCount}`
    );

    if (response.status === 404) {
      return {
        status: 404,
        message: "Brak podobnych wizualnie produktów",
      };
    }

    return response;
  } catch {
    return {
      status: 500,
      message: "Błąd podczas pobierania podobnych wizualnie produktów",
    };
  }
};

// CF item-to-item: klienci kupili również
export const getCfSimilarProducts = async (
  productId: string,
  topCount: number = 8
): Promise<ApiResult<ProductDto[]>> => {
  try {
    const response = await get<ProductDto[]>(
      `${trackingPrefix}/cf/similar-items/${productId}/products?topCount=${topCount}`
    );

    if (response.status === 404) {
      return {
        status: 404,
        message: "Brak danych o zakupach innych klientów",
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

// Ostatnio oglądane produkty
export const getRecentlyViewedProducts = async (
  userId: string,
  limit: number = 8
): Promise<ApiResult<ProductDto[]>> => {
  try {
    const response = await get<ProductDto[]>(
      `${trackingPrefix}/recently-viewed/${userId}/products?limit=${limit}`
    );

    if (response.status === 404) {
      return {
        status: 404,
        message: "Brak ostatnio oglądanych produktów",
      };
    }

    return response;
  } catch {
    return {
      status: 500,
      message: "Błąd podczas pobierania ostatnio oglądanych produktów",
    };
  }
};

// Rekomendacje dla koszyka - CF item-to-item dla wielu produktów
export const getCartRecommendations = async (
  productIds: string[],
  topCount: number = 8
): Promise<ApiResult<ProductDto[]>> => {
  if (productIds.length === 0) {
    return { status: 200, data: [], message: "Empty cart" };
  }

  try {
    // Pobierz rekomendacje dla każdego produktu z koszyka
    const allRecommendations: ProductDto[] = [];
    const seenIds = new Set(productIds); // Nie pokazuj produktów już w koszyku

    for (const productId of productIds.slice(0, 3)) { // Limit do 3 produktów żeby nie przeciążać
      const response = await getCfSimilarProducts(productId, 4);
      if (response.status === 200 && response.data) {
        for (const product of response.data) {
          if (!seenIds.has(String(product.id))) {
            seenIds.add(String(product.id));
            allRecommendations.push(product);
          }
        }
      }
    }

    // Ogranicz do topCount
    return {
      status: 200,
      data: allRecommendations.slice(0, topCount),
      message: "Success",
    };
  } catch {
    return {
      status: 500,
      message: "Błąd podczas pobierania rekomendacji dla koszyka",
    };
  }
};
