import { District } from './district';

//DTO: Campus
export interface Campus {
    campusId: number;
    campusName: string;
    addressId: number;
    districts: District[];
}