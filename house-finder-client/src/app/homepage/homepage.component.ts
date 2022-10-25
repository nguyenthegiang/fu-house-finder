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

  constructor(
    private houseService: HouseService,
    private campusService: CampusService,
    private roomTypeService: RoomTypeService,
  ) { }

  ngOnInit(): void {
    //Call APIs:

    //Get List of all Houses
    this.houseService.getAllHouses().subscribe(data => {
      this.houses = data;
    });

    //Get Campuses
    this.campusService.getAllCampuses().subscribe(data => {
      this.campuses = data;
    });

    //Get Room types
    this.roomTypeService.getRoomTypes().subscribe(data => {
      this.roomTypes = data;
    });
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
