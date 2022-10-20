import { Component, OnInit } from '@angular/core';
import { Room } from 'src/app/models/room';
import { HouseService } from 'src/app/services/house.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-list-room',
  templateUrl: './list-room.component.html',
  styleUrls: ['./list-room.component.scss']
})
export class ListRoomComponent implements OnInit {
  //List of rooms
  rooms: Room[] = [];
  //Money of not rented rooms
  money:number = 0;

  constructor(
    private roomService: RoomService,
    private houseService: HouseService,
  ) { }

  ngOnInit(): void {
    //Get id of House from Route
    const id = 1;
    //Call API: get available rooms of this house
    this.roomService.getRooms(id).subscribe(data => {
      this.rooms = data;
    });
    //Call API: get total money for not rented rooms of this house
    this.houseService.getMoneyForNotRentedRooms(id).subscribe(data => {
         this.money = data;
    })
  }

  counter(i: number) {
    return new Array(i);
}

}
