import { post, get } from "./client/httpClient.tsx";
import type { ApiResult } from "../types/api/ApiResult.tsx";
import type {
  AddItemToCartRequest,
  RemoveItemFromCartRequest,
  UpdateCartItemQuantityRequest,
  GetCartRequest,
  ShoppingCartResponse,
} from "../types/cart/CartApi";

const modulePrefix = "/cart";

// Cart operations
export const addItemToCart = async (
  request: AddItemToCartRequest
): Promise<ApiResult<void>> => {
  return await post<void>(`${modulePrefix}/addItem`, request);
};

export const removeItemFromCart = async (
  request: RemoveItemFromCartRequest
): Promise<ApiResult<void>> => {
  return await post<void>(`${modulePrefix}/removeItem`, request);
};

export const updateCartItemQuantity = async (
  request: UpdateCartItemQuantityRequest
): Promise<ApiResult<void>> => {
  return await post<void>(`${modulePrefix}/updateQuantity`, request);
};

export const clearCart = async (): Promise<ApiResult<void>> => {
  return await post<void>(`${modulePrefix}/clearCart`, {});
};

// Cart queries
export const getCartItems = async (
  request: GetCartRequest
): Promise<ApiResult<ShoppingCartResponse>> => {
  return await post<ShoppingCartResponse>(
    `${modulePrefix}/getCartItems`,
    request
  );
};

export const getUserCart = async (): Promise<
  ApiResult<ShoppingCartResponse>
> => {
  return await get<ShoppingCartResponse>(`${modulePrefix}/user`);
};
