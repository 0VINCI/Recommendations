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
