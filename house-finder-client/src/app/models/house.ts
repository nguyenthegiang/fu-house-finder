import { Address } from './address';

//DTO: Houses
export interface House {
    houseId: number;
    houseName: string;
    information: string;
    landlordId: string;
    powerPrice: number;
    waterPrice: number;
    lowestRoomPrice: number;
    highestRoomPrice: number;
    address: Address;
}