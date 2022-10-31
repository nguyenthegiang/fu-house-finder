import { RoomType } from './roomType';
import { Status } from './status';
import { ImagesOfRoom } from './imagesOfRoom';

//DTO: Rooms
export interface Room {
    roomId: number;
    roomName: string;
    pricePerMonth: number;
    information: string;
    areaByMeters: number;
    aircon: boolean;
    wifi: boolean;
    waterHeater: boolean;
    furniture: boolean;
    maxAmountOfPeople: number;
    currentAmountOfPeople: number;
    buildingNumber: number;
    floorNumber: number;
    status: Status;
    roomType: RoomType;
    houseId: number;
    createdDate: Date;
    lastModifiedDate: Date;
    createdBy: string;
    lastModifiedBy: string;
    imagesOfRooms: ImagesOfRoom[];
}
