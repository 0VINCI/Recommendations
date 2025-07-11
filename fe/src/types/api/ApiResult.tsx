export type ApiResult<T = unknown> = {
    status: number;
    data?: T;
    message?: string;
};