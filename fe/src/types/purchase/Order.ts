import type { Address } from "./Address";
import type { OrderItem } from "./OrderItem";
import type { Payment } from "./Payment";

export interface Order {
  idOrder: string;
  customerId: string;
  status: number;
  createdAt: string;
  paidAt: string | null;
  items: OrderItem[];
  payments: Payment[];
  address: Address;
}
