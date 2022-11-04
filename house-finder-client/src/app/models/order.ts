//DTO: Orders
export interface Order {
  orderId: number;
  studentId: string;
  studentName: string;
  phoneNumber: string;
  email: string;
  orderContent: string;
  solved: boolean;
  orderedDate: Date;
  solvedDate: Date;
}
