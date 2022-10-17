import { RoomType } from './roomType';
import { Status } from './status';

//DTO: Rooms
export interface Room {
    roomId: number;
    roomName: string;
    pricePerMonth: number;
    information: string;
    areaByMeters: number;
    aircon: boolean;
    maxAmountOfPeople: number;
    currentAmountOfPeople: number;
    buildingNumber: number;
    floorNumber: number;
    status: Status;
    roomType: RoomType;
}
