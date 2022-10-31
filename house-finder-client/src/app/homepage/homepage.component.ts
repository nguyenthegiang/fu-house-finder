import { RoomService } from 'src/app/services/room.service';
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
  //List of available houses to display in Main Content
  houses: House[] = [];

  //to display in Main Content
  countAvailableRooms: number = 0;

  //(Paging)
  countAvailableHouses = 0; //items count
  pageSize = 9;             //number of items per page
  pageNumber = 1;           //starts at page 1
  pageCount = 0;            //number of pages
  pageList: number[] = [];  //array to loop with *ngFor in HTML Template

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
    private roomService: RoomService,
  ) { }

  ngOnInit(): void {
    //Call APIs:

    //(List) Get available Houses - default: page 1, 9 items
    this.houseService.filterAvailableHouses(this.pageSize, this.pageNumber).subscribe(data => {
      this.houses = data;
    });

    //(Paging) Count available Houses for total number of pages
    this.houseService.countTotalAvailableHouse().subscribe(data => {
      this.countAvailableHouses = data;

      // (Paging) Calculate number of pages
      this.pageCount = Math.round(this.countAvailableHouses / this.pageSize);

      // (Paging) Render pageList based on pageCount
      this.pageList = Array.from(Array(this.pageCount).keys());
      //pageList is now an array like {1, 2, 3, ..., n | n = pageCount} 
    });

    //(List) Count available Rooms
    this.roomService.countAvailableRooms().subscribe(data => {
      this.countAvailableRooms = data;
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

  //Search House by Name (contains)
  searchHouseByName(searchHouseName: string) {
    //not perform search for empty input
    if (!searchHouseName.trim()) {
      return;
    }

    //call API (filter by name contains)
    this.houseService.filterAvailableHouses(this.pageSize, this.pageNumber, searchHouseName).subscribe(data => {
      this.houses = data;
    });
  }

  //[Filter] Filter by Campus
  onCampusSelected(selectedCampusId: string) {
    // convert string to number
    var numberCampusId: number = +selectedCampusId;

    // Call API: update list houses with the campus user chose
    alert(numberCampusId);
  }

  //[Filter] Filter by Distance
  onDistanceSelected(minDistance: string, maxDistance: string) {
    // convert string to number
    var numMinDistance: number = +minDistance;
    var numMaxDistance: number = +maxDistance;

    // (special case) 0 or empty input -> not handle
    if (numMinDistance == 0 || numMaxDistance == 0) {
      return;
    }

    /* (special case) -> alert:
        min > max
        negative number
    */
    if (numMinDistance > numMaxDistance || numMinDistance < 0 || numMaxDistance < 0) {
      alert('Giá trị nhập vào không hợp lệ!');
      return;
    }

    // Call API to update list houses with the distance user chose
    alert('distance: ' + numMinDistance + ' ' + numMaxDistance);
  }

  //[Filter] Filter by Price
  onPriceSelected(minPrice: string, maxPrice: string) {
    // convert string to number
    var numMinPrice: number = +minPrice;
    var numMaxPrice: number = +maxPrice;

    // (special case) 0 or empty input -> not handle
    if (numMinPrice == 0 || numMaxPrice == 0) {
      return;
    }

    /* (special case) -> alert:
        min > max
        negative number
    */
    if (numMinPrice > numMaxPrice || numMinPrice < 0 || numMaxPrice < 0) {
      alert('Giá trị nhập vào không hợp lệ!');
      return;
    }

    // Call API to update list houses with the price user chose
    alert('price: ' + numMinPrice + ' ' + numMaxPrice);
  }

  //[Filter] Filter by Room Type
  onRoomTypeSelected(event: any, roomTypeId: number) {
    //see if user just checked or unchecked the checkbox
    const isChecked = (<HTMLInputElement>event.target).checked;

    // Call API to update list houses with the selected room type
    alert('Event: ' + isChecked + ' Room type: ' + roomTypeId);
  }

  //[Filter] Change list of Communes after user selected District
  onDistrictSelected(selectedDistrictId: string) {
    // convert string to number
    var numberDistrictId: number = +selectedDistrictId;

    // find the selected district
    this.districts.forEach((district) => {
      // assign the list of Commune as the communes of this District
      if (district.districtId == numberDistrictId) {
        this.communesOfSelectedDistrict = district.communes;
        return;
      }
    });

    //TODO: call API to search for houses with this District
  }

  //[Filter] Change list of Villages after user selected Commune
  onCommuneSelected(selectedCommuneId: string) {
    // convert string to number
    var numberCommuneId: number = +selectedCommuneId;

    // find the selected commune
    this.communesOfSelectedDistrict.forEach((commune) => {
      // assign the list of Villages as the villages of this Commune
      if (commune.communeId == numberCommuneId) {
        this.villagesOfSelectedCommune = commune.villages;
        return;
      }
    });

    //TODO: call API to search for houses with this Commune
  }

  //[Filter] TODO: call API to search for houses with this Village
  onVillageSelected(selectedVillageId: string) {

  }

  //[Filter] Filter by Room Utility
  onUtilitySelected(event: any, utilityName: string) {
    //see if user just checked or unchecked the checkbox
    const isChecked = (<HTMLInputElement>event.target).checked;

    // Call API to update list houses with the selected room type
    alert('Event: ' + isChecked + ' Utility Name: ' + utilityName);
  }
}
