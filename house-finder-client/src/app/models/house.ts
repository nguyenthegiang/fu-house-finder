import { Address } from './address';
import { ImagesOfHouse } from './imagesOfHouse';

//DTO: Houses
export interface House {
    houseId: number;
    houseName: string;
    information: string;
    landlordId: string;
    powerPrice: number;
    waterPrice: number;
    fingerprintLock: boolean;
    camera: boolean;
    parking: boolean;
    lowestRoomPrice: number;
    highestRoomPrice: number;
    address: Address;
    imagesOfHouses?: ImagesOfHouse[];
}
