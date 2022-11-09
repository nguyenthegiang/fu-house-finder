import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { House } from 'src/app/models/house';
import { HouseService } from 'src/app/services/house.service';
import { LandlordInformationService } from 'src/app/services/landlord-information.service';

@Component({
  selector: 'app-staff-landlord-detail',
  templateUrl: './staff-landlord-detail.component.html',
  styleUrls: ['./staff-landlord-detail.component.scss']
})
export class StaffLandlordDetailComponent implements OnInit {
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

  searchHouseByName()
  {}
}
