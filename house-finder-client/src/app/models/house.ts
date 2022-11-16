import { Address } from './address';
import { ImagesOfHouse } from './imagesOfHouse';

//DTO: Houses
export interface House {
    houseId: number;
    houseName: string;
    information: string;
    distanceToCampus: number;

    //price
    powerPrice: number;
    waterPrice: number;
    lowestRoomPrice: number;
    highestRoomPrice: number;

    //utility
    fingerprintLock: boolean;
    camera: boolean;
    parking: boolean;

    //relationship
    landlordId: string;
    address: Address;
    imagesOfHouses?: ImagesOfHouse[];
    campusId: number;
    districtId: number;
    communeId: number;
    villageId: number;
}
