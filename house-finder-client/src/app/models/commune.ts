import { Village } from './village';

//DTO: Communes
export interface Commune {
    communeId: number;
    communeName: string;
    districtId: number;
    villages: Village[];
}
