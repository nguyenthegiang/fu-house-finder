import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Room } from 'src/app/models/room';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-update-room',
  templateUrl: './update-room.component.html',
  styleUrls: ['./update-room.component.scss']
})
export class UpdateRoomComponent implements OnInit, OnChanges {
  //Infomation of this room
  roomDetail: Room | undefined;

  constructor(
    private roomService: RoomService
  ) { }

  @Input() roomId!: number;

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.getRoomByRoomId();
  }

  //Call API: Get Room Detail info from ID
  getRoomByRoomId() {
    //Get id of room from Input
    const roomId = this.roomId;
    console.log('gogo: ' + roomId);
    this.roomService.getRoomByRoomId(roomId).subscribe(data => {
      this.roomDetail = data;
    })
  }

}
