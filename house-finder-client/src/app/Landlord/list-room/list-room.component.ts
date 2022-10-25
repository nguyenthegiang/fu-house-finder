import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Room } from 'src/app/models/room';
import { RoomType } from 'src/app/models/roomType';
import { Status } from 'src/app/models/status';
import { HouseService } from 'src/app/services/house.service';
import { RoomTypeService } from 'src/app/services/room-type.service';
import { RoomService } from 'src/app/services/room.service';
import { StatusService } from 'src/app/services/status.service';

@Component({
  selector: 'app-list-room',
  templateUrl: './list-room.component.html',
  styleUrls: ['./list-room.component.scss']
})
export class ListRoomComponent implements OnInit {
  //List of rooms
  rooms: Room[] = [];
  //Money of not rented rooms
  moneyForNotRentedRooms: number = 0;
  //List of room types
  roomTypes: RoomType[] = [];
  //List of statuses
  statuses: Status[] = [];

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomService,
    private houseService: HouseService,
    private roomTypeService: RoomTypeService,
    private statusService: StatusService,
  ) { }

  ngOnInit(): void {
    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));

    //Call API: get available rooms of this house
    this.roomService.getRooms(id).subscribe(data => {
      this.rooms = data;
    });

    //Call API: get total money for not rented rooms of this house
    this.houseService.getMoneyForNotRentedRooms(id).subscribe(data => {
      this.moneyForNotRentedRooms = data;
    });

    //Call API: get room types of this house
    this.roomTypeService.getRoomTypesByHouseId(id).subscribe(data => {
      this.roomTypes = data;
    });

    //Call API: get statuses of this house
    this.statusService.getStatusesByHouseId(id).subscribe(data => {
      this.statuses = data;
    });
  }

  counter(i: number) {
    return new Array(i);
  }
}
