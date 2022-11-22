import { OrderStatus } from "./orderStatus";
import { Staff } from "./staff";

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
  solvedDate?: Date;
  solvedByNavigation?: Staff;
}
