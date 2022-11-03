import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Room } from 'src/app/models/room';
import { RoomService } from 'src/app/services/room.service';
import { Location } from '@angular/common';
//import { StatusService } from 'src/app/services/status.service';
//import { Status } from 'src/app/models/status';



@Component({
  selector: 'app-update-room',
  templateUrl: './update-room.component.html',
  styleUrls: ['./update-room.component.scss']
})
export class UpdateRoomComponent implements OnInit, OnChanges {
  //Infomation of this room
  roomDetail: Room = {
    roomId: 0,
    roomName: '',
    pricePerMonth: 0,
    information: '',
    areaByMeters: 0,
    fridge: false,
    kitchen: false,
    washingMachine: false,
    desk: false,
    liveWithHost: false,
    bed: false,
    closedToilet: false,
    maxAmountOfPeople: 0,
    currentAmountOfPeople: 0,
    buildingNumber: 0,
    floorNumber: 0,
    status: {
      statusId: 1,
      statusName: 'Available'
    },
    roomType: {
      roomTypeId: 1,
      roomTypeName: 'Khép kín'
    },
    houseId: 0,
    createdDate: new Date(),
    lastModifiedDate: new Date(),
    createdBy: "",
    lastModifiedBy: "",
    imagesOfRooms: []
  };
  //listStatus: Status[] = [];
  //test string
  // statusSelected: string = "";

  constructor(
    private roomService: RoomService,
    private location: Location
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
    console.log("hello");


  }
  goBack(): void {
    window.location.reload();
  }

  //Call API: update room
  updateRoom() {
    this.roomService.updateRoom(this.roomDetail).subscribe(() => this.goBack());
  }
  deleteRoom() {
    console.log('delete');
    this.roomService.deleteRoom(this.roomDetail.roomId).subscribe(() => this.goBack());
  }

  //Call API: Get Room Detail info from ID
  getRoomByRoomId() {
    //Not calling API on first time run
    if (this.roomId == 0) {
      return;
    }

    this.roomService.getRoomByRoomId(this.roomId).subscribe(data => {
      this.roomDetail = data;

      // if (this.statusCheck == 2) {
      //   //this.deleteRoom(data.roomId);
      // }

      // //demo selected
      // //this.statusSelected = this.roomDetail.status.statusName;

    });
  }

  // getAllStatus() {
  //   this.statusService.getAllStatus().subscribe(data => {
  //     this.listStatus = data;
  //   })
  // }
}
