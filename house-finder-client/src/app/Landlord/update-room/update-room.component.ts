import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Room } from 'src/app/models/room';
import { RoomService } from 'src/app/services/room.service';
import { Location } from '@angular/common';
import { RoomStatus } from 'src/app/models/roomStatus';
import RoomStatusService from 'src/app/services/roomStatus.service';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';



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
    noLiveWithHost: false,
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
    delete: false,
    createdDate: new Date(),
    lastModifiedDate: new Date(),
    createdBy: "",
    lastModifiedBy: "",
    imagesOfRooms: []
  };
  roomCreate: Room = {
    roomId: 0,
    roomName: '',
    pricePerMonth: 0,
    information: '',
    areaByMeters: 0,
    fridge: false,
    kitchen: false,
    washingMachine: false,
    desk: false,
    noLiveWithHost: false,
    bed: false,
    closedToilet: false,
    maxAmountOfPeople: 4,
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
    houseId: 1,
    delete: false,
    createdDate: new Date(),
    lastModifiedDate: new Date(),
    createdBy: "LA000001",
    lastModifiedBy: "LA000001",
    imagesOfRooms: []
  };
  listStatus: RoomStatus[] = [];
  //test string
  // statusSelected: string = "";

  @ViewChild('roomErrorAlert') private roomErrorAlert: SwalComponent | undefined;
  @ViewChild('updateRoomSuccessAlert') private updateRoomSuccessAlert: SwalComponent | undefined;
  @ViewChild('addRoomSuccessAlert') private addRoomSuccessAlert: SwalComponent | undefined;
  @ViewChild('deleteRoomSuccessAlert') private deleteRoomSuccessAlert: SwalComponent | undefined;
  constructor(
    private roomService: RoomService,
    private location: Location,
    private roomStatusService: RoomStatusService
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

    this.initiateModal();
  }

  //determine whether this is a Create or Update modal
  initiateModal() {
    //call API to get detail info of room
    if (this.roomId != 0) {
      this.getRoomByRoomId();
    }
    else {
      this.roomDetail = this.roomCreate;
    }
  }

  goBack(): void {
    window.location.reload();
  }

  //Call API: update room
  updateRoom() {
    console.log(this.roomDetail);
    if (this.roomDetail.roomId === 0) {
      //add room
      this.roomService.addRoom(this.roomDetail).subscribe(
        data => {
          if (data.status == 400) {
            this.roomErrorAlert?.fire();
          } else if (data.status == 200) {
            this.addRoomSuccessAlert?.fire();
          }
        }
      );
    } else {
      //update room
      this.roomService.updateRoom(this.roomDetail).subscribe(
        data => {
          if (data.status == 200) {
            this.updateRoomSuccessAlert?.fire();
          } else if (data.status == 400) {
            this.roomErrorAlert?.fire();
          }
        },
        error => {
        });

    }

  }
  deleteRoom() {
    //console.log('delete');
    this.roomService.deleteRoom(this.roomDetail.roomId).subscribe(
      data => {
        if (data.status == 400) {
          this.roomErrorAlert?.fire();
        } else if (data.status == 200) {
          this.deleteRoomSuccessAlert?.fire();
        }
      }
    );
  }

  //Call API: Get Room Detail info from ID
  getRoomByRoomId() {
    //Not calling API on first time run
    if (this.roomId == 0) {
      return;
    }

    this.roomStatusService.getAllStatus().subscribe(data => {
      this.listStatus = data;
    })
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
