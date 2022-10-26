import { Component, OnInit } from '@angular/core';
import { House } from 'src/app/models/house';
import { HouseService } from 'src/app/services/house.service';
import { LandlordInformationService } from 'src/app/services/landlord-information.service';

@Component({
  selector: 'app-list-house',
  templateUrl: './list-house.component.html',
  styleUrls: ['./list-house.component.scss']
})
export class ListHouseComponent implements OnInit {
//List of all houses
  houses: House[] = [];

  //{Search} input value
  houseCount: number = 0;
  roomCount: number = 0;
  roomAvailableCount: number = 0;

  constructor(private houseService: HouseService, private lanlord_informationService: LandlordInformationService) { }

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
}
