import type { SignIn, SignInResponse } from "../types/authorization/SignIn.tsx";
import { post } from "./client/httpClient.tsx";
import type { User } from "../types/authorization/User.tsx";
import type { ChangePassword } from "../types/authorization/ChangePassword.tsx";
import type { ApiResult } from "../types/api/ApiResult.tsx";
import type { RemindPassword } from "../types/authorization/RemindPassword.tsx";

const modulePrefix = "/authorization";

export const signIn = async (
  signIn: SignIn
): Promise<ApiResult<SignInResponse>> => {
  return await post<SignInResponse>(`${modulePrefix}/signIn`, signIn);
};

export const signUp = async (signUp: User) => {
  return await post<void>(`${modulePrefix}/signUp`, signUp);
};

export const signOut = async () => {
  return await post<void>(`${modulePrefix}/signOut`);
};

export const changePassword = async (changePasswordData: ChangePassword) => {
  return await post<void>(`${modulePrefix}/changePassword`, changePasswordData);
};

export const remindPassword = async (remindPasswordData: RemindPassword) => {
  return await post<void>(`${modulePrefix}/remindPassword`, remindPasswordData);
};

export const getCurrentUser = async (): Promise<ApiResult<SignInResponse>> => {
  return await post<SignInResponse>(`${modulePrefix}/me`);
};
