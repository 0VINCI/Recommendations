// API types
export type { ApiResult } from "./api/ApiResult";

// Authorization types
export type { User } from "./authorization/User";
export type { SignIn, SignInResponse } from "./authorization/SignIn";
export type { ChangePassword } from "./authorization/ChangePassword";

// Cart types
export type { Product as CartProduct, CartItem, Order, Theme } from "./cart";

export type {
  AddItemToCartRequest,
  RemoveItemFromCartRequest,
  UpdateCartItemQuantityRequest,
  GetCartRequest,
  CartItemResponse,
  ShoppingCartResponse,
} from "./cart/CartApi";

// Product types
export type {
  Product,
  ProductDetails,
  ProductImage,
  SubCategory,
  MasterCategory,
  ArticleType,
  BaseColour,
} from "./product";

export type {
  GetProductsRequest,
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
} from "./product/ProductApi";
