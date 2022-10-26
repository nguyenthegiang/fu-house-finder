import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Room } from 'src/app/models/room';
import { RoomService } from 'src/app/services/room.service';

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

  @Input() roomId!: number;
  ngOnInit(): void {

  }
  getRoomByRoomId() {
    //Get id of room from router
    const roomId = this.roomId;
    console.log(roomId)
    this.roomService.getRoomByRoomId(roomId).subscribe(data => {
      this.roomDetail = data;
    })
  }

}
