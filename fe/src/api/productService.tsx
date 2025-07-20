import { get } from "./client/httpClient.tsx";
import type { ApiResult } from "../types/api/ApiResult.tsx";
import type { ProductDto } from "../types/product/ProductDto";
import type {
  GetProductsResponse,
  GetProductByIdRequest,
  GetProductByIdResponse,
  GetMasterCategoriesRequest,
  GetMasterCategoriesResponse,
  MasterCategoriesResponse,
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
  const result = await get<GetProductByIdResponse>(
    `${modulePrefix}/products/${request.productId}`
  );

  if (result.status === 200) {
    const productData = result.data as unknown as ProductDto;
    if (productData.id) {
      return {
        status: 200,
        data: { product: productData },
        message: "Success",
      };
    }
    return result;
  }

  const productsResult = await get<GetProductsResponse>(
    `${modulePrefix}/products?page=1&pageSize=1000`
  );

  if (productsResult.status === 200 && productsResult.data) {
    const product = productsResult.data.products.find(
      (p) => p.id === request.productId
    );
    if (product) {
      return {
        status: 200,
        data: { product },
        message: "Success",
      };
    }
  }

  return result;
};

export const getProductFullById = async (
  request: GetProductByIdRequest
): Promise<ApiResult<GetProductByIdResponse>> => {
  return await get<GetProductByIdResponse>(
    `${modulePrefix}/products/${request.productId}/full`
  );
};

export const getProductsByCategory = async (
  masterCategoryId?: string,
  subCategoryId?: string,
  page: number = 1,
  pageSize: number = 20
): Promise<ApiResult<GetProductsResponse>> => {
  const params = new URLSearchParams();
  if (masterCategoryId) params.append("masterCategoryId", masterCategoryId);
  if (subCategoryId) params.append("subCategoryId", subCategoryId);
  params.append("page", page.toString());
  params.append("pageSize", pageSize.toString());

  const url = `${modulePrefix}/products/category?${params.toString()}`;
  return await get<GetProductsResponse>(url);
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
): Promise<ApiResult<MasterCategoriesResponse>> => {
  const params = new URLSearchParams();
  if (request.active !== undefined) {
    params.append("active", request.active.toString());
  }
  const queryString = params.toString();
  const url = queryString
    ? `${modulePrefix}/products/categories?${queryString}`
    : `${modulePrefix}/products/categories`;
  return await get<MasterCategoriesResponse>(url);
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
