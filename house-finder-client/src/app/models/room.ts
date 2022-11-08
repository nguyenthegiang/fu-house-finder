import { RoomType } from './roomType';
import { RoomStatus } from './roomStatus';
import { ImagesOfRoom } from './imagesOfRoom';

//DTO: Rooms
export interface Room {
    roomId: number;
    roomName: string;
    pricePerMonth: number;
    information: string;
    areaByMeters: number;
    fridge: boolean;
    kitchen: boolean;
    washingMachine: boolean;
    desk: boolean;
    noLiveWithHost: boolean;
    bed: boolean;
    closedToilet: boolean;
    maxAmountOfPeople: number;
    currentAmountOfPeople: number;
    buildingNumber: number;
    floorNumber: number;
    status: RoomStatus;
    roomType: RoomType;
    houseId: number;
    delete: boolean;
    createdDate: Date;
    lastModifiedDate: Date;
    createdBy: string;
    lastModifiedBy: string;
    imagesOfRooms: ImagesOfRoom[];
}
