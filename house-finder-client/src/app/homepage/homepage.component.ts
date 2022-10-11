import { HouseService } from './../services/house.service';
import { Component, OnInit } from '@angular/core';
import { House } from '../models/house';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']
})
export class HomepageComponent implements OnInit {
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
