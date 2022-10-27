import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Room } from 'src/app/models/room';
import { RoomService } from 'src/app/services/room.service';
//import { StatusService } from 'src/app/services/status.service';
//import { Status } from 'src/app/models/status';



@Component({
  selector: 'app-update-room',
  templateUrl: './update-room.component.html',
  styleUrls: ['./update-room.component.scss']
})
export class UpdateRoomComponent implements OnInit, OnChanges {
  //Infomation of this room
  roomDetail: Room | undefined;
  //listStatus: Status[] = [];
  //test string
  statusSelected: string = "";

  constructor(
    private roomService: RoomService,
    //demo status selected
    //private statusService: StatusService,

  ) { }

  @Input() roomId!: number;

  ngOnInit(): void {
  }

  // When @Input() roomId changes => user has clicked on 'Update' button
  ngOnChanges(changes: SimpleChanges): void {

    // status demo
    //this.getAllStatus();

    //call API to get detail info of room
    this.getRoomByRoomId();
  }

  //Call API: Get Room Detail info from ID
  getRoomByRoomId() {
    //Not calling API on first time run
    if (this.roomId == 0) {
      return;
    }

    this.roomService.getRoomByRoomId(this.roomId).subscribe(data => {
      this.roomDetail = data;
      //demo selected
      //this.statusSelected = this.roomDetail.status.statusName;

    });
  }

  // getAllStatus() {
  //   this.statusService.getAllStatus().subscribe(data => {
  //     this.listStatus = data;
  //   })
  // }
}
