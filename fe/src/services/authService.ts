import axios from "axios";
import type { User } from "../types/authorization/User";

const apiUrl = process.env.REACT_APP_API_URL;

export const userLogin = async (
  user: User
): Promise<{ userId?: string; error?: string }> => {
  try {
    const response = await axios.post(`${apiUrl}/signIn`, user, {
      withCredentials: true,
    });
    if (response.status === 200 && response.data.userId) {
      return { userId: response.data.userId };
    } else {
      return { error: "Nieprawidłowa odpowiedź z serwera." };
    }
  } catch (error) {
    if (axios.isAxiosError(error) && error.response) {
      if (error.response.status === 403) {
        return { error: "Nieprawidłowe dane logowania." };
      } else {
        return {
          error:
            "Błąd podczas logowania: " +
            (error.response.data.error || "Nieznany błąd"),
        };
      }
    } else {
      console.log("Błąd podczas nawiązywania połączenia z API", error);
      throw new Error("Błąd sieci: Nie można nawiązać połączenia z API.");
    }
  }
};
