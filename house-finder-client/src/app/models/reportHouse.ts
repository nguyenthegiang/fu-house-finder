import { Address } from "./address";
import { StaffReport } from "./staffReport";
import { User } from "./user";

export interface ReportHouse {
  houseId: number;
  houseName: string;
  landlord: User;
  address: Address;
  numberOfReport: number;
  listReports: StaffReport[];
}
