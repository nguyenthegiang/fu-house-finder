//DTO: Rates
export interface Rate {
  rateId: number;
  star?: number;
  comment: string;
  landlordReply: string;
  houseId: number;
  studentId: string;
  deleted: boolean;
  createdDate: Date;
  lastModifiedDate?: Date;
  createdBy: string;
  lastModifiedBy: string;
}
