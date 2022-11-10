import { Component, OnInit } from '@angular/core';
import { HouseService } from 'src/app/services/house.service';
import { OrderService } from 'src/app/services/order.service';
import { ReportService } from 'src/app/services/report.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-staff-navbar',
  templateUrl: './staff-navbar.component.html',
  styleUrls: ['./staff-navbar.component.scss']
})
export class StaffNavbarComponent implements OnInit {
  //Total of available rooms
  availabelRoomsNum: number = 0;
  //Total of available capacity
  availableCapNum: number = 0;
  //Total of houses
  totalHouses: number = 0;
  //Total of rooms
  totalRooms: number = 0;
  //Total of available houses
  availableHouseNum: number = 0;

  constructor(private roomService: RoomService,
    private houseService: HouseService,
    private reportService: ReportService,
    private orderService: OrderService,) { }

  ngOnInit(): void {
    //Call API: get total of available rooms
    this.roomService.countAvailableRooms().subscribe(data => {
      this.availabelRoomsNum = data;
    });

    //Call API: get number of total rooms
    this.roomService.CountTotalRoom().subscribe(data => {
      this.totalRooms = data;
    });

    //Call API: get total of available capacity
    this.roomService.countAvailableCapacity().subscribe(data => {
      this.availableCapNum = data;
    });

    //Call API: get total houses
    this.houseService.getTotalHouse().subscribe(data => {
      this.totalHouses = data;
    });

    //Call API: get total of available houses
    this.houseService.countTotalAvailableHouse().subscribe(data => {
      this.availableHouseNum = data;
    });
  }
}
