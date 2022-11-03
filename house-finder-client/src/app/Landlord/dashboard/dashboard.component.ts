import { LandlordInformationService } from './../../services/landlord-information.service';
import { Component, OnInit } from '@angular/core';
import { House } from 'src/app/models/house';
import { HouseService } from 'src/app/services/house.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  //List of all houses
  houses: House[] = [];

  //{Search} input value
  houseCount: number = 0;
  roomCount: number = 0;
  roomAvailableCount: number = 0;

  constructor(private houseService: HouseService,
    private lanlord_informationService: LandlordInformationService,
    private router: Router)
  { }

  ngOnInit(): void {
    //Get List of all Houses
    this.houseService.getListHousesByLandlordId("LA000003").subscribe(data => {
      this.houses = data;
    });

    this.lanlord_informationService.getLandLordInfomation("LA000003").subscribe(data => {
      this.houseCount = data.houseCount;
      this.roomCount = data.roomCount;
      this.roomAvailableCount = data.roomAvailableCount;
    });
  }

  viewHouse(id: number)
  {
    console.log(id);
    this.router.navigate(['/Landlord/landlord-house-detail/' + id]);
  }

  addHouse()
  {

  }
}
