import { Commune } from './commune';

//DTO: Districts
export interface District {
    districtId: number;
    districtName: string;
    campusId: number;
    communes: Commune[];
}
