import { LandlordInformationService } from './../../services/landlord-information.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { House } from 'src/app/models/house';
import { HouseService } from 'src/app/services/house.service';
import { Router } from '@angular/router';
import { RoomService } from 'src/app/services/room.service';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  //Child alert
  @ViewChild('orderErrorAlert') private orderErrorAlert: SwalComponent | undefined;
  //List of all houses
  houses: House[] = [];

  //{Search} input value
  houseCount: number = 0;
  roomCount: number = 0;
  roomAvailableCount: number = 0;

  //[Update and Delete] houseId to pass into <update-house>
  houseId: number = 0;

  totalHouse: number = 0;
  availableHouse: number = 0;
  totalRoom: number = 0;
  availableRoom: number = 0;
  totalCapacity: number = 0;
  availableCapacity: number = 0;
  stay: boolean = true;

  constructor(private houseService: HouseService,
    private roomService: RoomService,
    private lanlord_informationService: LandlordInformationService,
    private router: Router) { }

  ngOnInit(): void {
    //Get List of all Houses
    var user = null;
    user = localStorage.getItem("user");
    if (user === null) {
      //user not logged in => Alert
      this.orderErrorAlert?.fire();
    } else {
      //Check user logged in from Server => if not => alert
      this.houseService.getListHousesByLandlordId(user).subscribe(data => {
        this.houses = data;
      });
      this.lanlord_informationService.getLandLordInfomation(user).subscribe(data => {
        this.houseCount = data.houseCount;
        this.roomCount = data.roomCount;
        this.roomAvailableCount = data.roomAvailableCount;
      });

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

  viewHouse(id: number) {
    if (this.stay === true) {
      this.router.navigate(['/Landlord/landlord-house-detail/' + id]);
    }
  }

  viewRate(id: number) {
    this.router.navigate(['/Landlord/rate-house/' + id]);
  }

  updateHouse(id: number) {
    this.router.navigate(['/Landlord/update-house/' + id]);
  }

  deleteHouse(id: number) {
    this.stay = false;
    this.houseId = id;
  }

  addHouse() {
    this.router.navigate(['/Landlord/add-house']);
  }

  search(searchValue: string) { }

  logout() {
    window.location.href = "/login";
  }
}
