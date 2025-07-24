import type { ProductDto } from "../types/product/ProductDto";
import type { User } from "../types/authorization/User.tsx";
import type { ToastItem } from "../components/common/ToastContainer";
import type { CartItem, Order, Theme, CartProduct } from "../types/index.ts";

export interface AppState {
  products: ProductDto[];
  cart: CartItem[];
  user: User | null;
  orders: Order[];
  theme: Theme;
  isAuthModalOpen: boolean;
  authMode: "login" | "register";
  isChangePasswordModalOpen: boolean;
  isRemindPasswordModalOpen: boolean;
  isResetPasswordModalOpen: boolean;
  resetPasswordEmail: string;
  toasts: ToastItem[];
}

export type AppAction =
  | {
      type: "ADD_TO_CART";
      payload: {
        product: ProductDto | CartProduct;
        size: string;
        color: string;
      };
    }
  | {
      type: "REMOVE_FROM_CART";
      payload: string | { productId: string; size?: string; color?: string };
    }
  | {
      type: "UPDATE_CART_QUANTITY";
      payload: { productId: string; quantity: number };
    }
  | {
      type: "SET_ORDERS";
      payload: Order[];
    }
  | { type: "CLEAR_CART" }
  | { type: "SET_USER"; payload: User | null }
  | { type: "SET_THEME"; payload: Theme }
  | { type: "TOGGLE_AUTH_MODAL"; payload?: "login" | "register" }
  | { type: "CLOSE_AUTH_MODAL" }
  | { type: "OPEN_CHANGE_PASSWORD_MODAL" }
  | { type: "CLOSE_CHANGE_PASSWORD_MODAL" }
  | { type: "OPEN_REMIND_PASSWORD_MODAL" }
  | { type: "CLOSE_REMIND_PASSWORD_MODAL" }
  | { type: "OPEN_RESET_PASSWORD_MODAL"; payload: string }
  | { type: "CLOSE_RESET_PASSWORD_MODAL" }
  | { type: "ADD_TOAST"; payload: ToastItem }
  | { type: "REMOVE_TOAST"; payload: string }
  | { type: "ADD_ORDER"; payload: Order }
  | { type: "SET_CART"; payload: CartItem[] };

export const initialState: AppState = {
  products: [],
  cart: [],
  user: null,
  orders: [],
  theme: "light",
  isAuthModalOpen: false,
  authMode: "login",
  isChangePasswordModalOpen: false,
  isRemindPasswordModalOpen: false,
  isResetPasswordModalOpen: false,
  resetPasswordEmail: "",
  toasts: [],
};

export function appReducer(state: AppState, action: AppAction): AppState {
  switch (action.type) {
    case "ADD_TO_CART": {
      // Convert ProductDto to Product if needed
      const product =
        "productDisplayName" in action.payload.product
          ? ({
              id: action.payload.product.id,
              name: action.payload.product.productDisplayName,
              price: action.payload.product.price,
              originalPrice: action.payload.product.originalPrice,
              image: `https://via.placeholder.com/600x600/cccccc/666666?text=${encodeURIComponent(
                action.payload.product.productDisplayName
              )}`,
              category: action.payload.product.subCategoryName,
              description: `${action.payload.product.brandName} - ${action.payload.product.productDisplayName}`,
              sizes: ["S", "M", "L", "XL"],
              colors: [action.payload.product.baseColourName || "Default"],
              rating: action.payload.product.rating,
              reviews: action.payload.product.reviews,
              isBestseller: action.payload.product.isBestseller,
              isNew: action.payload.product.isNew,
              subCategory: action.payload.product.subCategoryName,
              baseColour: action.payload.product.baseColourName,
            } as CartProduct)
          : action.payload.product;

      const existingItem = state.cart.find(
        (item) =>
          item.product.id === product.id &&
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
            product,
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
        cart: state.cart.filter((item) => {
          if (typeof action.payload === "string") {
            return item.product.id !== action.payload;
          } else {
            return !(
              item.product.id === action.payload.productId &&
              (action.payload.size === undefined ||
                item.size === action.payload.size) &&
              (action.payload.color === undefined ||
                item.color === action.payload.color)
            );
          }
        }),
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

    case "SET_ORDERS":
      return {
        ...state,
        orders: action.payload,
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
        isAuthModalOpen: true,
        authMode:
          action.payload || (state.authMode === "login" ? "register" : "login"),
      };

    case "CLOSE_AUTH_MODAL":
      return {
        ...state,
        isAuthModalOpen: false,
      };

    case "OPEN_CHANGE_PASSWORD_MODAL":
      return {
        ...state,
        isChangePasswordModalOpen: true,
      };

    case "CLOSE_CHANGE_PASSWORD_MODAL":
      return {
        ...state,
        isChangePasswordModalOpen: false,
      };

    case "OPEN_REMIND_PASSWORD_MODAL":
      return {
        ...state,
        isRemindPasswordModalOpen: true,
      };

    case "CLOSE_REMIND_PASSWORD_MODAL":
      return {
        ...state,
        isRemindPasswordModalOpen: false,
      };

    case "OPEN_RESET_PASSWORD_MODAL":
      return {
        ...state,
        isResetPasswordModalOpen: true,
        resetPasswordEmail: action.payload,
      };

    case "CLOSE_RESET_PASSWORD_MODAL":
      return {
        ...state,
        isResetPasswordModalOpen: false,
        resetPasswordEmail: "",
      };

    case "ADD_TOAST":
      return {
        ...state,
        toasts: [...state.toasts, action.payload],
      };

    case "REMOVE_TOAST":
      return {
        ...state,
        toasts: state.toasts.filter((toast) => toast.id !== action.payload),
      };

    case "ADD_ORDER":
      return {
        ...state,
        orders: [action.payload, ...state.orders],
      };

    case "SET_CART":
      return {
        ...state,
        cart: action.payload,
      };

    default:
      return state;
  }
}
