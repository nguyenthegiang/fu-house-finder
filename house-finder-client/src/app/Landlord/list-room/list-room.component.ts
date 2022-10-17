import { Component, OnInit } from '@angular/core';
import { Room } from 'src/app/models/room';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-list-room',
  templateUrl: './list-room.component.html',
  styleUrls: ['./list-room.component.scss']
})
export class ListRoomComponent implements OnInit {
  //List of rooms
  rooms: Room[] = [];

  constructor(
    private roomService: RoomService,
  ) { }

  ngOnInit(): void {
    //Get id of House from Route
    const id = 1;
    //Call API: get available rooms of this house
    this.roomService.getRooms(id).subscribe(data => {
      this.rooms = data;
    });
  }

}
