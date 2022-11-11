import { Address } from "./address";
import { User } from "./user";

export interface ReportHouse {
  houseId: number;
  houseName: string;
  landlord: User;
  address: Address;
  numberOfReport: number;
}
