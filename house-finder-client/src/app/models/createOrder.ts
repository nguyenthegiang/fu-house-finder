

//DTO: Orders
export interface CreateOrder {
    orderId: number;
    studentId?: string;
    studentName: string;
    phoneNumber: string;
    email: string;
    orderContent: string;
    statusId: number;
    orderedDate?: Date;
    solvedDate?: Date;
}
