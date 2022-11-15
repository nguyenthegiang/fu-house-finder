import { Component, OnInit } from '@angular/core';
import { HouseService } from 'src/app/services/house.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-staff-header',
  templateUrl: './staff-header.component.html',
  styleUrls: ['./staff-header.component.scss']
})
export class StaffHeaderComponent implements OnInit
{
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

  //Home button -> go back to /home
  backHome() {
    //use reload() so that when user is already in home -> they will get a reload of page
    window.location.href = "/home";
  }

  login(){
    window.location.href = "/login";
  }
}
