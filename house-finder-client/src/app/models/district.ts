import { Commune } from './commune';

//DTO: Districts
export interface District {
    districtId: number;
    districtName: string;
    communes: Commune[];
}
