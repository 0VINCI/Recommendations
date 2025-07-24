import type { AddAddress } from "../types/purchase/AddAddress.ts";
import type { AddCustomer } from "../types/purchase/AddCustomer.ts";
import type { Address } from "../types/purchase/Address.ts";
import type { CreateOrder } from "../types/purchase/CreateOrder.ts";
import type { Customer } from "../types/purchase/Customer.ts";
import type { Order } from "../types/purchase/Order.ts";
import type { Payment } from "../types/purchase/Payment.ts";
import type { UpdateOrderStatus } from "../types/purchase/UpdateOrderStatus.ts";
import { get, patch, post } from "./client/httpClient.tsx";

const modulePrefix = "/orders";

export const createOrder = async (newOrder: CreateOrder) => {
  return await post<string>(`${modulePrefix}/create`, newOrder);
};

export const getMyOrders = async () => {
  return await get<Order[]>(`${modulePrefix}/mine`);
};

export const getOrderById = async (orderId: string) => {
  return await get<Order>(`${modulePrefix}/${orderId}`);
};

export const payForOrder = async (payment: Payment) => {
  return await post<void>(`${modulePrefix}/pay`, payment);
};

export const getOrdersStatus = async (orderIds: number[]) => {
  return await post<void>(`${modulePrefix}/status`, orderIds);
};

export const updateOrderStatus = async (orderStatus: UpdateOrderStatus) => {
  return await patch<void>(`${modulePrefix}/status`, orderStatus);
};

export const getCustomer = async () => {
  return await get<Customer>(`${modulePrefix}/customer`);
};

export const addNewCustomer = async (newCustomer: AddCustomer) => {
  return await post<void>(`${modulePrefix}/customer`, {
    CustomerDto: newCustomer,
  });
};

export const updateCustomer = async (customer: Customer) => {
  return await post<void>(`${modulePrefix}/customer`, customer);
};

export const addNewAddress = async (newAddress: AddAddress) => {
  return await post<void>(`${modulePrefix}/customer/addresses`, {
    AddressDto: newAddress,
  });
};

export const updateAddress = async (address: Address) => {
  return await post<void>(`${modulePrefix}/customer/address`, address);
};
