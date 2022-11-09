//DTO: Report
export interface Report {
    reportContent: string;
    studentId: string;
    houseId: number;
    createdBy: string;
    lastModifiedBy: string;
    //no need for createdDate & lastModifiedDate
}
