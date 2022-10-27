import { Component, OnInit } from '@angular/core';
import { HouseService } from 'src/app/services/house.service';
import { RoomTypeService } from 'src/app/services/room-type.service';
import { RoomService } from 'src/app/services/room.service';
import { StatusService } from 'src/app/services/status.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardStaffComponent implements OnInit {
  //Total of available rooms
  availabelRoomsNum: number = 0;

  //Total of available capacity
  availableCapNum: number = 0;

  constructor(
    private roomService: RoomService,
    private houseService: HouseService,
    private roomTypeService: RoomTypeService,
    private statusService: StatusService,
  ) { }

  ngOnInit(): void {
    //Call API: get total of available rooms
    this.roomService.countAvailableRooms().subscribe(data => {
      this.availabelRoomsNum = data;
    });

    //Call API: get total of available capacity
    this.roomService.countAvailableCapacity().subscribe(data => {
      this.availableCapNum = data;
    });
  }

}
