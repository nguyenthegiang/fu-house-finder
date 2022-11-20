import { House } from "./house";
import { User } from "./user";
import { ReportHouse } from "./reportHouse";


//DTO: Report
export interface StaffReport {
  reportId: number;
  reportContent: string;
  student: User;
  house: ReportHouse;
  reportedDate: Date;
  solvedDate: Date;
  solvedBy: string;
}
