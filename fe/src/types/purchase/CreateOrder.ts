import type { OrderItem } from "./OrderItem";
import type { Payment } from "./Payment";

export interface CreateOrder {
  CustomerId: string;
  ShippingAddressId: string;
  Status: number;
  CreatedAt: Date;
  PaidAt: Date;
  Items: OrderItem[];
  Payments: Payment[];
}
