//DTO: Report
export interface Report {
    reportContent: string;
    studentId: string;
    houseId: number;
    statusId: number;
    deleted: boolean;
    reportedDate: Date;
    solvedDate: Date;
    solvedBy?: string;


    //no need for createdDate & lastModifiedDate
}
