import type { Address } from "./Address";

export interface Customer {
  idCustomer: string;
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  addresses: Address[];
}
