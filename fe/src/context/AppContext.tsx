import React, { createContext, useReducer, useEffect } from "react";
import {
  appReducer,
  initialState,
  type AppState,
  type AppAction,
} from "./appReducer";
import { getCurrentUser } from "../api/authorizationService";

const AppContext = createContext<{
  state: AppState;
  dispatch: React.Dispatch<AppAction>;
} | null>(null);

export function AppProvider({ children }: { children: React.ReactNode }) {
  const [state, dispatch] = useReducer(appReducer, initialState);

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
    getCurrentUser().then((res) => {
      if (res.status === 200 && res.data) {
        const user = {
          IdUser: res.data.idUser,
          Name: res.data.name,
          Surname: res.data.surname,
          Email: res.data.email,
        };
        dispatch({ type: "SET_USER", payload: user });
      }
    });
  }, []);

  return (
    <AppContext.Provider value={{ state, dispatch }}>
      {children}
    </AppContext.Provider>
  );
}

export { AppContext };
