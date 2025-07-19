import { get } from "./client/httpClient.tsx";
import type { ApiResult } from "../types/api/ApiResult.tsx";
import type {
  GetProductsResponse,
  GetProductByIdRequest,
  GetProductByIdResponse,
  GetMasterCategoriesRequest,
  GetMasterCategoriesResponse,
  GetSubCategoriesRequest,
  GetSubCategoriesResponse,
  GetArticleTypesRequest,
  GetArticleTypesResponse,
  GetBaseColoursResponse,
  SearchProductsResponse,
  GetProductsRequest,
} from "../types/product/ProductApi";

const modulePrefix = "/dic";

function toQueryString(params: object): string {
  return Object.entries(params)
    .filter(([_, v]) => v !== undefined && v !== null && v !== "")
    .map(
      ([k, v]) => `${encodeURIComponent(k)}=${encodeURIComponent(String(v))}`
    )
    .join("&");
}

export const getProducts = async (
  params: GetProductsRequest
): Promise<ApiResult<GetProductsResponse>> => {
  const query = toQueryString(params).toString();
  return await get<GetProductsResponse>(`${modulePrefix}/products?${query}`);
};

export const getProductById = async (
  request: GetProductByIdRequest
): Promise<ApiResult<GetProductByIdResponse>> => {
  return await get<GetProductByIdResponse>(
    `${modulePrefix}/products/${request.productId}`
  );
};

export const getProductFullById = async (
  request: GetProductByIdRequest
): Promise<ApiResult<GetProductByIdResponse>> => {
  return await get<GetProductByIdResponse>(
    `${modulePrefix}/products/${request.productId}/full`
  );
};

export const getProductsByCategory = async (
  category: string
): Promise<ApiResult<GetProductsResponse>> => {
  return await get<GetProductsResponse>(
    `${modulePrefix}/products/category/${category}`
  );
};

export const getBestsellers = async (
  params: GetProductsRequest
): Promise<ApiResult<GetProductsResponse>> => {
  const query = toQueryString(params).toString();
  return await get<GetProductsResponse>(
    `${modulePrefix}/products/bestsellers?${query}`
  );
};

export const getNewProducts = async (
  params: GetProductsRequest
): Promise<ApiResult<GetProductsResponse>> => {
  const query = toQueryString(params).toString();
  return await get<GetProductsResponse>(
    `${modulePrefix}/products/new?${query}`
  );
};

// Category operations
export const getMasterCategories = async (
  request: GetMasterCategoriesRequest = {}
): Promise<ApiResult<GetMasterCategoriesResponse>> => {
  const params = new URLSearchParams();
  if (request.active !== undefined) {
    params.append("active", request.active.toString());
  }
  const queryString = params.toString();
  const url = queryString
    ? `${modulePrefix}/masterCategories?${queryString}`
    : `${modulePrefix}/masterCategories`;
  return await get<GetMasterCategoriesResponse>(url);
};

export const getSubCategories = async (
  request: GetSubCategoriesRequest = {}
): Promise<ApiResult<GetSubCategoriesResponse>> => {
  const params = new URLSearchParams();
  if (request.masterCategoryId) {
    params.append("masterCategoryId", request.masterCategoryId);
  }
  if (request.active !== undefined) {
    params.append("active", request.active.toString());
  }
  const queryString = params.toString();
  const url = queryString
    ? `${modulePrefix}/subCategories?${queryString}`
    : `${modulePrefix}/subCategories`;
  return await get<GetSubCategoriesResponse>(url);
};

export const getArticleTypes = async (
  request: GetArticleTypesRequest = {}
): Promise<ApiResult<GetArticleTypesResponse>> => {
  const params = new URLSearchParams();
  if (request.subCategoryId) {
    params.append("subCategoryId", request.subCategoryId);
  }
  if (request.active !== undefined) {
    params.append("active", request.active.toString());
  }
  const queryString = params.toString();
  const url = queryString
    ? `${modulePrefix}/articleTypes?${queryString}`
    : `${modulePrefix}/articleTypes`;
  return await get<GetArticleTypesResponse>(url);
};

export const getBaseColours = async (): Promise<
  ApiResult<GetBaseColoursResponse>
> => {
  return await get<GetBaseColoursResponse>(`${modulePrefix}/baseColours`);
};

// Search operations
export const searchProducts = async (
  query: string
): Promise<ApiResult<SearchProductsResponse>> => {
  return await get<SearchProductsResponse>(
    `${modulePrefix}/products/search?query=${encodeURIComponent(query)}`
  );
};
