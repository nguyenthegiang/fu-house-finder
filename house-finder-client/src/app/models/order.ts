import { OrderStatus } from "./orderStatus";

//DTO: Orders
export interface Order {
  orderId: number;
  studentId: string;
  studentName: string;
  phoneNumber: string;
  email: string;
  orderContent: string;
  status: OrderStatus;
  orderedDate: Date;
  solvedDate: Date;
}
