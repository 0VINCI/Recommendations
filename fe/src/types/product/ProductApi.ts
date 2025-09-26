import type {
  ProductDto,
  ProductFullDto,
  SubCategoryDto,
  MasterCategoryDto,
  ArticleTypeDto,
  BaseColourDto,
} from "./ProductDto";

export interface GetProductsRequest {
  page?: number;
  pageSize?: number;
  subCategoryId?: string;
  masterCategoryId?: string;
  articleTypeId?: string;
  baseColourId?: string;
  minPrice?: number;
  maxPrice?: number;
  isBestseller?: boolean;
  isNew?: boolean;
  searchTerm?: string;
}

export interface GetProductsResponse {
  products: ProductDto[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface GetProductByIdRequest {
  productId: string;
}

export interface GetProductByIdResponse {
  product: ProductDto;
}

export interface GetProductFullByIdResponse {
  product: ProductFullDto;
}

export interface GetMasterCategoriesRequest {}

export interface GetMasterCategoriesResponse {
  masterCategories: MasterCategoryDto[];
}

export type MasterCategoriesResponse = MasterCategoryDto[];

export interface GetSubCategoriesRequest {
  masterCategoryId?: string;
}

export interface GetSubCategoriesResponse {
  subCategories: SubCategoryDto[];
}

export interface GetArticleTypesRequest {
  subCategoryId?: string;
}

export interface GetArticleTypesResponse {
  articleTypes: ArticleTypeDto[];
}

export interface GetBaseColoursResponse {
  baseColours: BaseColourDto[];
}

export interface SearchProductsResponse {
  products: ProductDto[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  searchTerm: string;
}
