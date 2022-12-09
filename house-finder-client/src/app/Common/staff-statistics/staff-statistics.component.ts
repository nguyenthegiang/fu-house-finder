import { Component, OnInit } from '@angular/core';
import { HouseService } from 'src/app/services/house.service';
import { OrderService } from 'src/app/services/order.service';
import { ReportService } from 'src/app/services/report.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-staff-statistics',
  templateUrl: './staff-statistics.component.html',
  styleUrls: ['./staff-statistics.component.scss']
})
export class StaffStatisticsComponent implements OnInit {
  totalHouse: number = 0;
  availableHouse: number = 0;
  totalRoom: number = 0;
  availableRoom: number = 0;
  totalCapacity: number = 0;
  availableCapacity: number = 0;

  constructor(private houseService: HouseService,
    private roomService: RoomService)
  { }

  ngOnInit(): void
  {
    this.houseService.getTotalHouse().subscribe(data => {
      this.totalHouse = data;
    });

    this.houseService.countTotalAvailableHouse().subscribe(data => {
      this.availableHouse = data;
    });

    this.roomService.CountTotalRoom().subscribe(data => {
      this.totalRoom = data;
    });

    this.roomService.countAvailableRooms().subscribe(data => {
      this.availableRoom = data;
    });

    this.roomService.CountTotalCapacity().subscribe(data => {
      this.totalCapacity = data;
    });

    this.roomService.countAvailableCapacity().subscribe(data => {
      this.availableCapacity = data;
    });
  }
}
