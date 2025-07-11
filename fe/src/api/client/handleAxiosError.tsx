import type {ApiResult} from "../../types/api/ApiResult.tsx";
import axios from "axios";

export function handleAxiosError<T>(err: unknown): ApiResult<T> {
    if (axios.isAxiosError(err)) {
        const message =
            typeof err.response?.data === "object" && err.response?.data && "message" in err.response.data
                ? (err.response.data as { message?: string }).message
                : err.message;
        return {
            status: err.response?.status ?? 0,
            message,
        };
    }
    return {
        status: 0,
        message: "Wystąpił nieoczekiwany błąd!",
    };
}