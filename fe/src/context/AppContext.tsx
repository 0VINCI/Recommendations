import React, { createContext, useReducer, useEffect } from "react";
import { mockProducts } from "../data/mockProducts";
import {
  appReducer,
  initialState,
  type AppState,
  type AppAction,
} from "./appReducer";

const AppContext = createContext<{
  state: AppState;
  dispatch: React.Dispatch<AppAction>;
} | null>(null);

export function AppProvider({ children }: { children: React.ReactNode }) {
  const [state, dispatch] = useReducer(appReducer, {
    ...initialState,
    products: mockProducts,
  });

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

  return (
    <AppContext.Provider value={{ state, dispatch }}>
      {children}
    </AppContext.Provider>
  );
}

export { AppContext };
