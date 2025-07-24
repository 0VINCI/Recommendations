import type { Address } from "./Address";

export interface AddCustomer {
  UserId: string;
  FirstName: string;
  LastName: string;
  Email: string;
  PhoneNumber: string;
  Addresses: Address[];
}
