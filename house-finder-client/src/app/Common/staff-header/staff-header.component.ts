import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HouseService } from 'src/app/services/house.service';
import { RoomService } from 'src/app/services/room.service';
import { UserService } from 'src/app/services/user.service';

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

  userDisplayName: string = "";

  constructor(private houseService: HouseService,
    private roomService: RoomService,
    private router: Router,
    private userService: UserService)
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

    this.userDisplayName = localStorage.getItem('user') || '{}';
  }

  //Home button -> go back to /home
  backHome() {
    //use reload() so that when user is already in home -> they will get a reload of page
    window.location.href = "/home";
  }

  logout(){
    this.userService.logout().subscribe(resp => {
      localStorage.clear();
      sessionStorage.clear();
      window.location.href = "/login";
    });
  }

  isDashboardRoute()
  {
    return this.router.url === "/Staff/dashboard";
  }

  isListLandlordRoute()
  {
    return this.router.url === "/Staff/list-landlord";
  }

  isListHouseRoute()
  {
    return this.router.url === "/Staff/list-house";
  }

  isListReportRoute()
  {
    return this.router.url === "/Staff/list-report";
  }

  isListRequestRoute()
  {
    return this.router.url === "/Staff/list-landlord-signup-request";
  }

  isListOrderRoute()
  {
    return this.router.url === "/Staff/list-order";
  }

  isUpdateProfileRoute()
  {
    return this.router.url === "/Staff/staff-update-profile";
  }
}
