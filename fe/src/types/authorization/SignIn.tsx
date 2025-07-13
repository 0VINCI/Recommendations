export interface SignIn {
  Email: string;
  Password: string;
}

export interface SignInResponse {
  idUser: string;
  name: string;
  surname: string;
  email: string;
}
