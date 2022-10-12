import { Component, OnInit } from '@angular/core';
import { House } from 'src/app/models/house';
import { HouseService } from 'src/app/services/house.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  //List of all houses
  houses: House[] = [];

  //{Search} input value
  searchHouseName: any;

  constructor(private houseService: HouseService) { }

  ngOnInit(): void {
    //Get List of all Houses
    this.houseService.getAllHouses().subscribe(data => {
      this.houses = data;
    });
  }

  //Search House by Name
  searchHouseByName()
  {
    var value = this.searchHouseName;
    //call API
    this.houseService.searchHouseByName(value).subscribe(data => {
      this.houses = data;
    });
  }
}
