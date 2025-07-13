// Request types
export interface AddItemToCartRequest {
  ProductId: string;
  Quantity: number;
}

export interface RemoveItemFromCartRequest {
  ProductId: string;
}

export interface UpdateCartItemQuantityRequest {
  ProductId: string;
  Quantity: number;
}

export interface GetCartRequest {
  CartId: string;
}

// Response types
export interface CartItemResponse {
  ProductId: string;
  Name: string;
  Quantity: number;
  UnitPrice: number;
  Subtotal: number;
}

export interface ShoppingCartResponse {
  IdCart: string;
  CreatedAt: string;
  Total: number;
  Items: CartItemResponse[];
}
