export interface Product {
  id: string;
  productDisplayName: string;
  brandName: string;
  price: number;
  originalPrice?: number;
  rating: number;
  reviews: number;
  isBestseller: boolean;
  isNew: boolean;

  subCategoryId: string;
  subCategory?: SubCategory;
  articleTypeId: string;
  articleType?: ArticleType;
  baseColourId: string;
  baseColour?: BaseColour;

  details?: ProductDetails;
  images: ProductImage[];
}

export interface ProductDetails {
  id: string;
  productId: string;
  product?: Product;

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

export interface ProductImage {
  id: string;
  productId: string;
  product?: Product;

  imageUrl: string;
  imageType: string; // "default", "front", "back", "search"
  resolution?: string; // "150X200", "360X480", etc.
  isPrimary: boolean;
}

export interface SubCategory {
  id: string;
  name: string;
  masterCategoryId: string;
  masterCategory?: MasterCategory;
  active: boolean;
  socialSharingEnabled: boolean;
  isReturnable: boolean;
  isExchangeable: boolean;
  pickupEnabled: boolean;
  isTryAndBuyEnabled: boolean;

  articleTypes?: ArticleType[];
}

export interface MasterCategory {
  id: string;
  name: string;
  active: boolean;
  socialSharingEnabled: boolean;
  isReturnable: boolean;
  isExchangeable: boolean;
  pickupEnabled: boolean;
  isTryAndBuyEnabled: boolean;

  subCategories?: SubCategory[];
}

export interface ArticleType {
  id: string;
  name: string;
  subCategoryId: string;
  subCategory?: SubCategory;
  active: boolean;
  socialSharingEnabled: boolean;
  isReturnable: boolean;
  isExchangeable: boolean;
  pickupEnabled: boolean;
  isTryAndBuyEnabled: boolean;
  isMyntsEnabled: boolean;
}

export interface BaseColour {
  id: string;
  name: string;
  products?: Product[];
}
