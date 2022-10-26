import { RoomUtility } from './../models/roomUtilities';
import { CampusService } from './../services/campus.service';
import { Campus } from './../models/campus';
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
  //List of houses to display in Main Content
  houses: House[] = [];

  //Data for Filter column
  roomTypes: RoomType[] = [];   //Room types
  campuses: Campus[] = [];
  otherUtilities: RoomUtility[] = [];  //List of utilities of Rooms

  constructor(
    private houseService: HouseService,
    private campusService: CampusService,
    private roomTypeService: RoomTypeService,
  ) { }

  ngOnInit(): void {
    //Call APIs:

    //(List) Get all Houses
    this.houseService.getAllHouses().subscribe(data => {
      this.houses = data;
    });

    //(Filter) Get all Campuses
    this.campusService.getAllCampuses().subscribe(data => {
      this.campuses = data;
    });

    //(Filter) Get all Room types
    this.roomTypeService.getRoomTypes().subscribe(data => {
      this.roomTypes = data;
    });

    //--------------------------------

    //Generate data:

    //(Filter) Other utilities
    this.otherUtilities = [
      { "utilityName": "aircon", "displayName": "Điều hòa" },
      { "utilityName": "wifi", "displayName": "Wifi" },
      { "utilityName": "waterHeater", "displayName": "Bình nóng lạnh" },
      { "utilityName": "furniture", "displayName": "Nội thất" },
    ];

  }

  //Search House by Name
  searchHouseByName(searchHouseName: string) {
    //not perform search for empty input
    if (!searchHouseName.trim()) {
      return;
    }
    
    //call API
    this.houseService.searchHouseByName(searchHouseName).subscribe(data => {
      this.houses = data;
    });
  }
}
