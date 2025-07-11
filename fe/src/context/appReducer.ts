import type { Product, CartItem, Order, Theme } from "../types";
import type {User} from "../types/authorization/User.tsx";

export interface AppState {
  products: Product[];
  cart: CartItem[];
  user: User | null;
  orders: Order[];
  theme: Theme;
  isAuthModalOpen: boolean;
  authMode: "login" | "register";
}

export type AppAction =
  | {
      type: "ADD_TO_CART";
      payload: { product: Product; size: string; color: string };
    }
  | { type: "REMOVE_FROM_CART"; payload: string }
  | {
      type: "UPDATE_CART_QUANTITY";
      payload: { productId: string; quantity: number };
    }
  | { type: "CLEAR_CART" }
  | { type: "SET_USER"; payload: User | null }
  | { type: "SET_THEME"; payload: Theme }
  | { type: "TOGGLE_AUTH_MODAL"; payload?: "login" | "register" }
  | { type: "ADD_ORDER"; payload: Order };

export const initialState: AppState = {
  products: [],
  cart: [],
  user: null,
  orders: [],
  theme: "light",
  isAuthModalOpen: false,
  authMode: "login",
};

export function appReducer(state: AppState, action: AppAction): AppState {
  switch (action.type) {
    case "ADD_TO_CART": {
      const existingItem = state.cart.find(
        (item) =>
          item.product.id === action.payload.product.id &&
          item.size === action.payload.size &&
          item.color === action.payload.color
      );

      if (existingItem) {
        return {
          ...state,
          cart: state.cart.map((item) =>
            item === existingItem
              ? { ...item, quantity: item.quantity + 1 }
              : item
          ),
        };
      }

      return {
        ...state,
        cart: [
          ...state.cart,
          {
            product: action.payload.product,
            quantity: 1,
            size: action.payload.size,
            color: action.payload.color,
          },
        ],
      };
    }

    case "REMOVE_FROM_CART":
      return {
        ...state,
        cart: state.cart.filter(
          (_, index) => index.toString() !== action.payload
        ),
      };

    case "UPDATE_CART_QUANTITY":
      return {
        ...state,
        cart: state.cart.map((item) =>
          item.product.id === action.payload.productId
            ? { ...item, quantity: action.payload.quantity }
            : item
        ),
      };

    case "CLEAR_CART":
      return {
        ...state,
        cart: [],
      };

    case "SET_USER":
      return {
        ...state,
        user: action.payload,
      };

    case "SET_THEME":
      return {
        ...state,
        theme: action.payload,
      };

    case "TOGGLE_AUTH_MODAL":
      return {
        ...state,
        isAuthModalOpen: !state.isAuthModalOpen,
        authMode: action.payload || state.authMode,
      };

    case "ADD_ORDER":
      return {
        ...state,
        orders: [action.payload, ...state.orders],
      };

    default:
      return state;
  }
}
