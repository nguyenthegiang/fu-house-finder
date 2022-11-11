import { Address } from './address';
import { District } from './district';

//DTO: Campus
export interface Campus {
    campusId: number;
    campusName: string;
    districts: District[];
    address: Address;
}