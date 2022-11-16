import { Address } from './address';
import { ImagesOfHouse } from './imagesOfHouse';

//DTO: Houses but to display in Home Page -> different attributes
export interface HouseHomePage {
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

    //attributes only for Home Page
    averageRate: number;

    //statistic
    totallyAvailableRoomCount: number;
    partiallyAvailableRoomCount: number;
    availableCapacityCount: number;
}
