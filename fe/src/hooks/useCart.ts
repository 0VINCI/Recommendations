import { useCallback, useEffect, useRef } from "react";
import { useApp } from "../context/useApp";
import { useToast } from "./useToast";
import {
  addItemToCartDb,
  removeItemFromCartDb,
  updateCartItemQuantityDb,
  clearCartDb,
  getUserCartDb,
} from "../api/cartService";
import type { CartItemDbResponse } from "../types/cart/CartApi";
import { getProductById } from "../api/productService";
import type { ProductDto } from "../types/product/ProductDto";
import type { CartItem } from "../types/cart/Cart";

export function useCart() {
  const { state, dispatch } = useApp();
  const { showError, showSuccess } = useToast();
  const prevUserRef = useRef(state.user);

  const addToCart = useCallback(
    async (product: ProductDto, size: string = "", color: string = "") => {
      dispatch({
        type: "ADD_TO_CART",
        payload: { product, size, color },
      });
      if (state.user) {
        try {
          const result = await addItemToCartDb({
            ProductId: product.id,
            Name: product.productDisplayName,
            Price: product.price,
            Quantity: 1,
          });
          if (result.status >= 400) {
            showError("Błąd podczas zapisywania do bazy danych");
          } else {
            showSuccess("Produkt został dodany do koszyka!");
          }
        } catch {
          showError("Błąd podczas dodawania do koszyka");
        }
      } else {
        showSuccess("Produkt został dodany do koszyka!");
      }
    },
    [dispatch, showError, showSuccess, state.user]
  );

  const removeFromCart = useCallback(
    async (productId: string) => {
      dispatch({
        type: "REMOVE_FROM_CART",
        payload: productId,
      });
      if (state.user) {
        try {
          const result = await removeItemFromCartDb({ ProductId: productId });
          if (result.status >= 400) {
            showError("Błąd podczas usuwania z bazy danych");
          }
        } catch {
          showError("Błąd podczas usuwania z koszyka");
        }
      }
    },
    [dispatch, showError, state.user]
  );

  const updateQuantity = useCallback(
    async (productId: string, quantity: number) => {
      dispatch({
        type: "UPDATE_CART_QUANTITY",
        payload: { productId, quantity },
      });
      if (state.user) {
        try {
          const result = await updateCartItemQuantityDb({
            ProductId: productId,
            Quantity: quantity,
          });
          if (result.status >= 400) {
            showError("Błąd podczas aktualizacji w bazie danych");
          }
        } catch {
          showError("Błąd podczas aktualizacji ilości");
        }
      }
    },
    [dispatch, showError, state.user]
  );

  const clearCart = useCallback(async () => {
    dispatch({ type: "CLEAR_CART" });
    if (state.user) {
      try {
        const result = await clearCartDb();
        if (result.status >= 400) {
          showError("Błąd podczas czyszczenia bazy danych");
        }
      } catch {
        showError("Błąd podczas czyszczenia koszyka");
      }
    }
  }, [dispatch, showError, state.user]);

  useEffect(() => {
    const prevUser = prevUserRef.current;
    if (!prevUser && state.user && state.cart.length > 0) {
      const syncAndFetch = async () => {
        await Promise.all(
          state.cart.map(async (item) => {
            const product = item.product;
            const name =
              "productDisplayName" in product
                ? product.productDisplayName
                : product.name;
            if (name) {
              await addItemToCartDb({
                ProductId: product.id,
                Name: String(name),
                Price: product.price,
                Quantity: item.quantity,
              });
            }
          })
        );
        const backendCart = await getUserCartDb();
        if (
          backendCart.data &&
          backendCart.data.items &&
          backendCart.data.items.length > 0
        ) {
          const newCart: CartItem[] = await Promise.all(
            backendCart.data.items.map(async (item: CartItemDbResponse) => {
              let image = `https://picsum.photos/300/300?random=${item.productId}`;
              try {
                const productRes = await getProductById({
                  productId: item.productId,
                });
                if (
                  productRes.status === 200 &&
                  productRes.data &&
                  productRes.data.product &&
                  productRes.data.product.images &&
                  productRes.data.product.images.length > 0
                ) {
                  image =
                    productRes.data.product.images.find((img) => img.isPrimary)
                      ?.imageUrl || productRes.data.product.images[0].imageUrl;
                }
              } catch {
                void 0;
              }
              return {
                product: {
                  id: item.productId,
                  name: item.name,
                  price: item.unitPrice,
                  image,
                  category: "",
                  description: "",
                  sizes: [],
                  colors: [],
                  rating: 0,
                  reviews: 0,
                },
                quantity: item.quantity,
                size: "",
                color: "",
              };
            })
          );
          dispatch({ type: "SET_CART", payload: newCart });
        } else {
          dispatch({ type: "CLEAR_CART" });
        }
      };
      syncAndFetch().catch(() => {});
    }
    prevUserRef.current = state.user;
  }, [state.user, state.cart, dispatch]);

  return {
    cart: state.cart,
    addToCart,
    removeFromCart,
    updateQuantity,
    clearCart,
    cartTotal: state.cart.reduce(
      (total, item) => total + item.product.price * item.quantity,
      0
    ),
    cartItemCount: state.cart.reduce((count, item) => count + item.quantity, 0),
  };
}
