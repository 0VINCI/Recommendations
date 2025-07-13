import type {
  ProductDto,
  ProductFullDto,
  SubCategoryDto,
  MasterCategoryDto,
  ArticleTypeDto,
  BaseColourDto,
} from "./ProductDto";

// Get Products
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

// Get Product by ID
export interface GetProductByIdRequest {
  productId: string;
}

export interface GetProductByIdResponse {
  product: ProductDto;
}

export interface GetProductFullByIdResponse {
  product: ProductFullDto;
}

// Get Categories
export interface GetMasterCategoriesRequest {
  active?: boolean;
}

export interface GetMasterCategoriesResponse {
  masterCategories: MasterCategoryDto[];
}

export interface GetSubCategoriesRequest {
  masterCategoryId?: string;
  active?: boolean;
}

export interface GetSubCategoriesResponse {
  subCategories: SubCategoryDto[];
}

export interface GetArticleTypesRequest {
  subCategoryId?: string;
  active?: boolean;
}

export interface GetArticleTypesResponse {
  articleTypes: ArticleTypeDto[];
}

// GetBaseColoursRequest is empty - no filters needed

export interface GetBaseColoursResponse {
  baseColours: BaseColourDto[];
}

// Search Products
export interface SearchProductsResponse {
  products: ProductDto[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  searchTerm: string;
}
