export interface SignIn {
  Email: string;
  Password: string;
}

export interface SignInResponse {
  userId: string;
  name: string;
  surname: string;
  email: string;
}
