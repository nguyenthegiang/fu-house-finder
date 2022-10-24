import { HouseService } from './../services/house.service';
import { Component, OnInit } from '@angular/core';
import { House } from '../models/house';
import { RoomType } from '../models/roomType';
import { RoomTypeService } from '../services/room-type.service';

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

  //List of room types
  roomTypes: RoomType[] = [];

  constructor(
    private houseService: HouseService,
    private roomTypeService: RoomTypeService,
  ) { }

  ngOnInit(): void {
    //Get List of all Houses
    this.houseService.getAllHouses().subscribe(data => {
      this.houses = data;
    });

    //Call API: get Room types
    this.roomTypeService.getRoomTypes().subscribe(data => {
      this.roomTypes = data;
    })
  }

  //Search House by Name
  searchHouseByName() {
    var value = this.searchHouseName;
    //call API
    this.houseService.searchHouseByName(value).subscribe(data => {
      this.houses = data;
    });
  }
}
