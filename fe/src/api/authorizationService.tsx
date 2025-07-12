import type { SignIn, SignInResponse } from "../types/authorization/SignIn.tsx";
import { post } from "./client/httpClient.tsx";
import type { User } from "../types/authorization/User.tsx";
import type { ChangePassword } from "../types/authorization/ChangePassword.tsx";
import type { ApiResult } from "../types/api/ApiResult.tsx";

export const signIn = async (
  signIn: SignIn
): Promise<ApiResult<SignInResponse>> => {
  return await post<SignInResponse>("/signIn", signIn);
};

export const signUp = async (signUp: User) => {
  return await post<void>("/signUp", signUp);
};

export const signOut = async () => {
  return await post<void>("/signOut");
};

export const changePassword = async (changePasswordData: ChangePassword) => {
  return await post<void>("/changePassword", changePasswordData);
};

export const remindPassword = async (email: string) => {
  return await post<void>("/remindPassword", email);
};
