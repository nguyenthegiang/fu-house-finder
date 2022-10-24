import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Room } from '../models/room';
import { RoomService } from '../services/room.service';

@Component({
  selector: 'app-update-room',
  templateUrl: './update-room.component.html',
  styleUrls: ['./update-room.component.scss']
})
export class UpdateRoomComponent implements OnInit {
  //Infomation of this room
  roomDetail: Room | undefined;

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomService
  ) { }

  ngOnInit(): void {
  }
  getRoomByRoomId() {
    //Get id of room from router
    const roomId = Number(this.route.snapshot.paramMap.get('roomId'));
    this.roomService.getRoomByRoomId(roomId).subscribe(data => {
      this.roomDetail = data;
    })
  }

}
