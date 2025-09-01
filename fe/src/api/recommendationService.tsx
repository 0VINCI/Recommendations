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
  BackendSimilarProductDto,
} from "../types/recommendation/RecommendationApi";
import { getProductById } from "./productService";

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

// Get Similar Products
export const getSimilarProducts = async (
  request: GetSimilarProductsRequest
): Promise<ApiResult<GetSimilarProductsResponse>> => {
  const vectorType = mapAlgorithmToVectorType(request.algorithm);
  const topCount = request.topCount || 10;

  try {
    // Pobierz podobne produkty (tylko ID i score)
    const response = await get<BackendSimilarProductDto[]>(
      `${modulePrefix}/product-embeddings/${request.productId}/${vectorType}/similar?topCount=${topCount}`
    );

    if (response.status !== 200 || !response.data) {
      return {
        status: response.status,
        message: response.message || "Nie udało się pobrać podobnych produktów",
      };
    }

    // Pobierz pełne dane produktów
    const products: ProductDto[] = [];
    for (const similarProduct of response.data) {
      try {
        const productResult = await getProductById({
          productId: similarProduct.productId,
        });
        if (productResult.status === 200 && productResult.data?.product) {
          products.push(productResult.data.product);
        }
      } catch (error) {
        console.error(
          `Error fetching product ${similarProduct.productId}:`,
          error
        );
      }
    }

    return {
      status: 200,
      data: {
        products,
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

// Get Product Embedding
export const getProductEmbedding = async (
  request: GetProductEmbeddingRequest
): Promise<ApiResult<GetProductEmbeddingResponse>> => {
  const vectorType = mapAlgorithmToVectorType(request.algorithm);

  return await get<GetProductEmbeddingResponse>(
    `${modulePrefix}/product-embeddings/${request.productId}/${vectorType}`
  );
};

// Get All Product Embeddings
export const getAllProductEmbeddings = async (
  request: GetAllProductEmbeddingsRequest
): Promise<ApiResult<GetAllProductEmbeddingsResponse>> => {
  return await get<GetAllProductEmbeddingsResponse>(
    `${modulePrefix}/product-embeddings/${request.productId}`
  );
};
