import { House } from "./house";
import { User } from "./user";
import { ReportHouse } from "./reportHouse";
import { Staff } from "./staff";
import { ReportStatus } from "./reportStatus";


//DTO: Report
export interface StaffReport {
  reportId: number;
  reportContent: string;
  student: User;
  house: ReportHouse;
  status: ReportStatus;
  reportedDate: Date;
  solvedDate?: Date;
  solvedByNavigation?: Staff;
}
