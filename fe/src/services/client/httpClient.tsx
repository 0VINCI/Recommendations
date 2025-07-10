import axios, { type AxiosResponse } from "axios";

export const api = {
  address: process.env.REACT_APP_API_URL,
};

const instance = axios.create({
  baseURL: api.address,
  headers: {
    Accept: "*/*",
    "Content-Type": "application/json",
  },
  withCredentials: true,
});

export const get = async function <T>(
  request: string
): Promise<AxiosResponse<T>> {
  try {
    return await instance.get<T>(request);
  } catch (err: unknown) {
    if (axios.isAxiosError(err)) {
      return Promise.reject(err.message);
    }
    return Promise.reject("Wystąpił nieoczekiwany błąd!");
  }
};

export const post = async function <T>(
  request: string,
  params: any = null
): Promise<AxiosResponse<T>> {
  try {
    const payload = params ? JSON.stringify(params) : null;
    const response = await instance.post<T>(request, payload, {
      headers: {
        "Content-Type": "application/json",
      },
    });
    return response;
  } catch (err: unknown) {
    if (axios.isAxiosError(err)) {
      return Promise.reject(err.message);
    }
    return Promise.reject("Wystąpił nieoczekiwany błąd!");
  }
};
