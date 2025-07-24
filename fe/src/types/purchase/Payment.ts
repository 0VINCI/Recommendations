export interface Payment {
  OrderId?: string;
  Method: number;
  PaymentDate: Date;
  Details: string;
}
