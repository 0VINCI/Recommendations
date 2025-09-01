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
  details?: ProductDetailsDto;
  images: ProductImageDto[];
}

export interface ProductDetailsDto {
  gender: string;
  season: string;
  usage: string;
  year?: string;
  description?: string;
  sleeveLength?: string;
  fit?: string;
  fabric?: string;
  collar?: string;
  bodyOrGarmentSize?: string;
  pattern?: string;
  ageGroup?: string;
}

export interface ProductImageDto {
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

export interface MasterCategoryDto {
  id: string;
  name: string;
  active: boolean;
  socialSharingEnabled: boolean;
  isReturnable: boolean;
  isExchangeable: boolean;
  pickupEnabled: boolean;
  isTryAndBuyEnabled: boolean;
  subCategories: SubCategoryDto[];
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
