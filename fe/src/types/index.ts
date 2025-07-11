export interface Product {
  id: string;
  name: string;
  price: number;
  originalPrice?: number;
  image: string;
  category: string;
  description: string;
  sizes: string[];
  colors: string[];
  rating: number;
  reviews: number;
  isBestseller?: boolean;
  isNew?: boolean;
  // Additional fields from fashion dataset
  gender?: string;
  masterCategory?: string;
  subCategory?: string;
  articleType?: string;
  baseColour?: string;
  season?: string;
  year?: number;
  usage?: string;
}

export interface CartItem {
  product: Product;
  quantity: number;
  size: string;
  color: string;
}

export interface Order {
  id: string;
  userId: string;
  items: CartItem[];
  total: number;
  status: "pending" | "processing" | "shipped" | "delivered";
  createdAt: Date;
  shippingAddress: {
    street: string;
    city: string;
    postalCode: string;
    country: string;
  };
}

export type Theme = "light" | "dark";
