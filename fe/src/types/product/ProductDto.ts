// DTO types matching backend exactly
export interface ProductDto {
  id: string;
  productDisplayName: string;
  brandName: string;
  subCategoryId: string;
  subCategoryName: string;
  articleTypeId: string;
  articleTypeName: string;
  baseColourId: string;
  baseColourName: string;
  price: number;
  originalPrice?: number;
  rating: number;
  reviews: number;
  isBestseller: boolean;
  isNew: boolean;
}

export interface ProductDetailsDto {
  id: string;
  productId: string;
  gender: string;
  season: string;
  year: number;
  usage: string;
  description?: string;
  styleNote?: string;
  materialsCare?: string;
  fit?: string;
  fabric?: string;
  articleNumber?: string;
  vat?: number;
  ageGroup?: string;
  fashionType?: string;
  landingPageUrl?: string;
  variantName?: string;
  myntraRating?: number;
  catalogAddDate?: number;
  colour1?: string;
  colour2?: string;
  visualTag?: string;
  styleType?: string;
  productTypeId?: number;
  displayCategories?: string;
  weight?: string;
  navigationId?: number;
}

export interface ProductImageDto {
  id: string;
  productId: string;
  imageUrl: string;
  imageType: string;
  resolution?: string;
  isPrimary: boolean;
}

export interface ProductFullDto {
  product: ProductDto;
  details?: ProductDetailsDto;
  images: ProductImageDto[];
}

export interface MasterCategoryDto {
  id: string;
  name: string;
  active: boolean;
  socialSharingEnabled: boolean;
  isReturnable: boolean;
  isExchangeable: boolean;
  pickupEnabled: boolean;
  isTryAndBuyEnabled: boolean;
}

export interface SubCategoryDto {
  id: string;
  name: string;
  masterCategoryId: string;
  masterCategoryName: string;
  active: boolean;
  socialSharingEnabled: boolean;
  isReturnable: boolean;
  isExchangeable: boolean;
  pickupEnabled: boolean;
  isTryAndBuyEnabled: boolean;
}

export interface ArticleTypeDto {
  id: string;
  name: string;
  subCategoryId: string;
  subCategoryName: string;
  active: boolean;
  socialSharingEnabled: boolean;
  isReturnable: boolean;
  isExchangeable: boolean;
  pickupEnabled: boolean;
  isTryAndBuyEnabled: boolean;
  isMyntsEnabled: boolean;
}

export interface BaseColourDto {
  id: string;
  name: string;
}
