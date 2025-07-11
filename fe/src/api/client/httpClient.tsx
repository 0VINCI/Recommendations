import axios from "axios";
import type {ApiResult} from "../../types/api/ApiResult.tsx";
import {handleAxiosError} from "./handleAxiosError.tsx";

export const api = {
    address: import.meta.env.VITE_RECOMMENDATIONS_API_URL
};

const instance = axios.create({
    baseURL: api.address,
    headers: {
        'Accept': "*/*",
        "Content-Type": "application/json",
    },
    withCredentials: true
});

export async function get<T>(url: string): Promise<ApiResult<T>> {
    try {
        const response = await instance.get<T>(url);
        return {
            status: response.status,
            data: response.data,
            message: (response.data as { message?: string })?.message,
        };
    } catch (err: unknown) {
        return handleAxiosError<T>(err);
    }
}

export async function post<T>(url: string, body?: unknown): Promise<ApiResult<T>> {
    try {
        const response = await instance.post<T>(url, body);
        return {
            status: response.status,
            data: response.data,
            message: (response.data as { message?: string })?.message,
        };
    } catch (err: unknown) {
        return handleAxiosError<T>(err);
    }
}
