import { Staff } from "./staff";

//DTO: Report
export interface Report {
    reportContent: string;
    studentId: string;
    houseId: number;
    statusId: number;
    deleted: boolean;
    reportedDate: Date;
    solvedDate?: Date;
    solvedByNavigation?: Staff;
    //no need for createdDate & lastModifiedDate
}
