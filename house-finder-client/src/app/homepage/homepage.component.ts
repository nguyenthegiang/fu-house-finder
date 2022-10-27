import { Village } from './../models/village';
import { Commune } from './../models/commune';
import { District } from './../models/district';
import { DistrictService } from './../services/district.service';
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
  roomTypes: RoomType[] = [];         //Room types
  campuses: Campus[] = [];
  roomUtilities: RoomUtility[] = [];  //List of utilities of Rooms
  districts: District[] = [];         //(Regions) All Districts
  communesOfSelectedDistrict: Commune[] = []; //(Regions) all Communes of 1 selected District (only display after user has selected 1 District)
  villagesOfSelectedCommune: Village[] = [];  //(Regions) all Villages of 1 selected Commune (only display after user has selected 1 Commune)

  constructor(
    private houseService: HouseService,
    private campusService: CampusService,
    private roomTypeService: RoomTypeService,
    private districtService: DistrictService,
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

    //(Filter) Get all Districts, Communes, Villages
    this.districtService.getAllDistricts().subscribe(data => {
      this.districts = data;
    });

    //--------------------------------

    //Generate data:

    //(Filter) Other utilities
    this.roomUtilities = [
      { "utilityName": "aircon", "displayName": "Điều hòa" },
      { "utilityName": "wifi", "displayName": "Wifi" },
      { "utilityName": "waterHeater", "displayName": "Bình nóng lạnh" },
      { "utilityName": "furniture", "displayName": "Nội thất" },
    ];
  }

  //[Filter] Change list of Communes after user selected District
  onDistrictSelected(selectedDistrictId: number) {
    //find the selected district
    this.districts.forEach((district) => {
      //assign the list of Commune as the communes of this District
      if (district.districtId == selectedDistrictId) {
        this.communesOfSelectedDistrict = district.communes;
      }
    });
  }

  onCommuneSelected(selectedCommuneId: number) {

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
