import type { Product, User, CartItem } from "../types";

const API_BASE_URL = "http://localhost:5051";

export interface ApiProduct {
  id: string;
  productDisplayName: string;
  gender: string;
  masterCategory: string;
  subCategoryId: string;
  subCategoryName: string;
  articleTypeId: string;
  articleTypeName: string;
  baseColourId: string;
  baseColourName: string;
  season: string;
  year: number;
  usage: string;
  imageUrl: string;
  price: number;
  originalPrice?: number;
  rating: number;
  reviews: number;
  isBestseller: boolean;
  isNew: boolean;
}

export interface ApiProductDetails {
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

export interface ApiProductImage {
  id: string;
  productId: string;
  imageUrl: string;
  imageType: string;
  resolution?: string;
  isPrimary: boolean;
}

export interface ApiProductFull {
  product: ApiProduct;
  details?: ApiProductDetails;
  images: ApiProductImage[];
}

// Backend DTOs
export interface SignInDto {
  email: string;
  password: string;
}

export interface SignUpDto {
  name: string;
  surname: string;
  email: string;
  password: string;
}

export interface SignedInDto {
  idUser: string;
  token: string;
}

export interface UserDto {
  idUser: string;
  name: string;
  surname: string;
  email: string;
}

// Cart DTOs
export interface CartItemDto {
  productId: string;
  name: string;
  quantity: number;
  unitPrice: number;
  subtotal: number;
}

export interface ShoppingCartDto {
  idCart: string;
  createdAt: string;
  total: number;
  items: CartItemDto[];
}

// Order DTOs
export interface OrderDto {
  id: string;
  userId: string;
  items: CartItemDto[];
  total: number;
  status: string;
  createdAt: string;
  shippingAddress: {
    street: string;
    city: string;
    postalCode: string;
    country: string;
  };
}

export interface CreateOrderDto {
  items: CartItemDto[];
  total: number;
  shippingAddress: {
    street: string;
    city: string;
    postalCode: string;
    country: string;
  };
}

class ApiService {
  private async request<T>(
    endpoint: string,
    options?: RequestInit
  ): Promise<T> {
    const url = `${API_BASE_URL}${endpoint}`;
    const headers: HeadersInit = {
      "Content-Type": "application/json",
      ...options?.headers,
    };

    const response = await fetch(url, {
      headers,
      credentials: "include", // Include cookies in requests
      ...options,
    });

    if (!response.ok) {
      throw new Error(
        `API request failed: ${response.status} ${response.statusText}`
      );
    }

    return response.json();
  }

  // Product endpoints
  async getAllProducts(): Promise<Product[]> {
    const products = await this.request<ApiProduct[]>("/dic/products");
    return products.map(this.mapApiProductToProduct);
  }

  async getProductById(id: string): Promise<Product | null> {
    try {
      const product = await this.request<ApiProduct>(`/dic/products/${id}`);
      return this.mapApiProductToProduct(product);
    } catch (error) {
      if (error instanceof Error && error.message.includes("404")) {
        return null;
      }
      throw error;
    }
  }

  async getProductsByCategory(category: string): Promise<Product[]> {
    const products = await this.request<ApiProduct[]>(
      `/dic/products/category/${category}`
    );
    return products.map(this.mapApiProductToProduct);
  }

  async getBestsellers(): Promise<Product[]> {
    const products = await this.request<ApiProduct[]>(
      "/dic/products/bestsellers"
    );
    return products.map(this.mapApiProductToProduct);
  }

  async getNewProducts(): Promise<Product[]> {
    const products = await this.request<ApiProduct[]>("/dic/products/new");
    return products.map(this.mapApiProductToProduct);
  }

  async searchProducts(searchTerm: string): Promise<Product[]> {
    const products = await this.request<ApiProduct[]>(
      `/dic/products/search?query=${encodeURIComponent(searchTerm)}`
    );
    return products.map(this.mapApiProductToProduct);
  }

  async getProductFullById(id: string): Promise<ApiProductFull | null> {
    try {
      return await this.request<ApiProductFull>(`/dic/products/${id}/full`);
    } catch (error) {
      if (error instanceof Error && error.message.includes("404")) {
        return null;
      }
      throw error;
    }
  }

  // Authorization endpoints
  async signIn(
    email: string,
    password: string
  ): Promise<{ token: string; user: User }> {
    const signInDto: SignInDto = { email, password };
    const signedInDto = await this.request<SignedInDto>(
      "/authorization/signIn",
      {
        method: "POST",
        body: JSON.stringify(signInDto),
      }
    );

    // Map backend response to frontend User type
    const user: User = {
      id: signedInDto.idUser,
      email: email, // We have email from the request
      name: email.split("@")[0], // Fallback name from email
      avatar: undefined,
    };

    return { token: signedInDto.token, user };
  }

  async signUp(name: string, email: string, password: string): Promise<void> {
    const signUpDto: SignUpDto = {
      name: name.split(" ")[0] || name, // First name
      surname: name.split(" ").slice(1).join(" ") || name, // Last name or fallback
      email,
      password,
    };

    await this.request("/authorization/signUp", {
      method: "POST",
      body: JSON.stringify(signUpDto),
    });
  }

  async signOut(): Promise<void> {
    await this.request("/authorization/signOut", {
      method: "POST",
    });
  }

  // Cart endpoints
  async getUserCart(): Promise<ShoppingCartDto | null> {
    try {
      return await this.request<ShoppingCartDto>("/cart/user");
    } catch (error) {
      if (error instanceof Error && error.message.includes("404")) {
        return null;
      }
      throw error;
    }
  }

  async getUserCartWithProducts(): Promise<CartItem[] | null> {
    try {
      const cartDto = await this.request<ShoppingCartDto>("/cart/user");
      if (!cartDto) return null;

      // Map each cart item to full product info
      const cartItems = await Promise.all(
        cartDto.items.map((item) => this.mapCartItemDtoToCartItem(item))
      );

      return cartItems;
    } catch (error) {
      if (error instanceof Error && error.message.includes("404")) {
        return null;
      }
      throw error;
    }
  }

  async addToCart(
    productId: string,
    quantity: number,
    productName: string,
    productPrice: number
  ): Promise<void> {
    await this.request("/cart/addItem", {
      method: "POST",
      body: JSON.stringify({
        productId,
        name: productName,
        price: productPrice,
        quantity,
      }),
    });
  }

  async removeFromCart(productId: string): Promise<void> {
    await this.request("/cart/removeItem", {
      method: "POST",
      body: JSON.stringify({ productId }),
    });
  }

  async updateCartQuantity(productId: string, quantity: number): Promise<void> {
    await this.request("/cart/updateQuantity", {
      method: "POST",
      body: JSON.stringify({ productId, quantity }),
    });
  }

  async clearCart(): Promise<void> {
    await this.request("/cart/clearCart", {
      method: "POST",
      body: JSON.stringify({}),
    });
  }

  // Purchase endpoints
  async getCustomerOrders(): Promise<OrderDto[]> {
    return this.request<OrderDto[]>("/orders/mine");
  }

  async getOrderById(orderId: string): Promise<OrderDto> {
    return this.request<OrderDto>(`/orders/${orderId}`);
  }

  async createOrder(orderData: CreateOrderDto): Promise<void> {
    await this.request("/orders/create", {
      method: "POST",
      body: JSON.stringify(orderData),
    });
  }

  // Helper method to map API product to frontend product
  private mapApiProductToProduct(apiProduct: ApiProduct): Product {
    return {
      id: apiProduct.id,
      name: apiProduct.productDisplayName,
      price: apiProduct.price,
      originalPrice: apiProduct.originalPrice,
      image: apiProduct.imageUrl,
      category: apiProduct.masterCategory,
      description: apiProduct.usage,
      sizes: [],
      colors: [],
      rating: apiProduct.rating,
      reviews: apiProduct.reviews,
      isBestseller: apiProduct.isBestseller,
      isNew: apiProduct.isNew,
      // Additional fields from fashion dataset
      gender: apiProduct.gender,
      masterCategory: apiProduct.masterCategory,
      subCategory: apiProduct.subCategoryName,
      articleType: apiProduct.articleTypeName,
      baseColour: apiProduct.baseColourName,
      season: apiProduct.season,
      year: apiProduct.year,
      usage: apiProduct.usage,
    };
  }

  // Helper method to map CartItemDto to CartItem with full product info
  async mapCartItemDtoToCartItem(cartItemDto: CartItemDto): Promise<CartItem> {
    const product = await this.getProductById(cartItemDto.productId);
    if (!product) {
      throw new Error(`Product with ID ${cartItemDto.productId} not found`);
    }

    return {
      product,
      quantity: cartItemDto.quantity,
      size: "",
      color: "",
    };
  }
}

export const apiService = new ApiService();
