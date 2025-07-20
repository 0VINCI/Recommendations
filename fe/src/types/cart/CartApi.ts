// Request types
export interface AddItemToCartRequest {
  ProductId: string; // Guid
  Name: string;
  Price: number;
  Quantity: number;
}

export interface RemoveItemFromCartRequest {
  ProductId: string; // Guid
}

export interface UpdateCartItemQuantityRequest {
  ProductId: string; // Guid
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

// Extended types for database cart operations
export interface AddItemToCartDbRequest {
  ProductId: string;
  Name: string;
  Price: number;
  Quantity: number;
}

export interface UpdateCartItemQuantityDbRequest {
  ProductId: string;
  Quantity: number;
}

export interface CartItemDbResponse {
  productId: string;
  name: string;
  quantity: number;
  unitPrice: number;
  subtotal: number;
  size?: string;
  color?: string;
}

export interface ShoppingCartDbResponse {
  idCart: string;
  createdAt: string;
  total: number;
  items: CartItemDbResponse[];
}
