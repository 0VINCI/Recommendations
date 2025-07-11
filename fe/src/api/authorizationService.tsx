import type {SignIn} from "../types/authorization/SignIn.tsx";
import {post} from "./client/httpClient.tsx";
import type {User} from "../types/authorization/User.tsx";
import type {ChangePassword} from "../types/authorization/ChangePassword.tsx";

export const signIn = async (signIn: SignIn) => {
    return await post<void>('/signIn', signIn);
}

export const signUp = async (signUp: User) => {
    return await post<void>('/signUp', signUp);
}

export const signOut = async () => {
    return await post<void>('/signUp', signUp);
}

export const changePassword = async (changePasswordData: ChangePassword) => {
    return await post<void>('/changePassword', changePasswordData);
}

export const remindPassword = async (email: string) => {
    return await post<void>('/remindPassword', email);
}