import { House } from "./house";
import { User } from "./user";


//DTO: Report
export interface StaffReport {
  reportId: number;
  reportContent: string;
  student: User;
  houseId: number;
  createdDate: Date;
}
