import React, { createContext, useReducer, useEffect, useState } from "react";
import {
  appReducer,
  initialState,
  type AppState,
  type AppAction,
} from "./appReducer";
import { getCurrentUser } from "../api/authorizationService";
import { getUserCartDb } from "../api/cartService";

const AppContext = createContext<{
  state: AppState;
  dispatch: React.Dispatch<AppAction>;
  userLoading: boolean;
} | null>(null);

export function AppProvider({ children }: { children: React.ReactNode }) {
  const [state, dispatch] = useReducer(appReducer, initialState);
  const [userLoading, setUserLoading] = useState(true);

  useEffect(() => {
    const savedTheme = localStorage.getItem("theme") as AppState["theme"];
    if (savedTheme) {
      dispatch({ type: "SET_THEME", payload: savedTheme });
    }
  }, []);

  useEffect(() => {
    localStorage.setItem("theme", state.theme);
    if (state.theme === "dark") {
      document.documentElement.classList.add("dark");
    } else {
      document.documentElement.classList.remove("dark");
    }
  }, [state.theme]);

  useEffect(() => {
    setUserLoading(true);
    getCurrentUser()
      .then((res) => {
        if (res.status === 200 && res.data) {
          const user = {
            IdUser: res.data.idUser,
            Name: res.data.name,
            Surname: res.data.surname,
            Email: res.data.email,
          };
          dispatch({ type: "SET_USER", payload: user });
        }
      })
      .finally(() => setUserLoading(false));
  }, []);

  useEffect(() => {
    if (state.user) {
      getUserCartDb().then((backendCart) => {
        if (
          backendCart.data &&
          backendCart.data.items &&
          backendCart.data.items.length > 0
        ) {
          const newCart = backendCart.data.items.map((item: any) => ({
            product: {
              id: item.productId,
              name: item.name,
              price: item.unitPrice,
              image: `https://picsum.photos/300/300?random=${item.productId}`,
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
          }));
          dispatch({ type: "SET_CART", payload: newCart });
        } else {
          dispatch({ type: "CLEAR_CART" });
        }
      });
    }
  }, [state.user]);

  return (
    <AppContext.Provider value={{ state, dispatch, userLoading }}>
      {children}
    </AppContext.Provider>
  );
}

export { AppContext };
