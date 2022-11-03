import { Router } from '@angular/router';
import { RoomService } from 'src/app/services/room.service';
import { Village } from '../../models/village';
import { Commune } from '../../models/commune';
import { District } from '../../models/district';
import { DistrictService } from '../../services/district.service';
import { OtherUtility } from '../../models/otherUtilities';
import { CampusService } from '../../services/campus.service';
import { Campus } from '../../models/campus';
import { HouseService } from '../../services/house.service';
import { Component, OnInit } from '@angular/core';
import { House } from '../../models/house';
import { RoomType } from '../../models/roomType';
import { RoomTypeService } from '../../services/room-type.service';

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

  //[Filter] Data for Filter column
  roomTypes: RoomType[] = [];         //Room types
  campuses: Campus[] = [];
  houseUtilities: OtherUtility[] = [];  //List of utilities of Houses
  roomUtilities: OtherUtility[] = [];  //List of utilities of Rooms
  districts: District[] = [];         //(Regions) All Districts
  communesOfSelectedDistrict: Commune[] = []; //(Regions) all Communes of 1 selected District (only display after user has selected 1 District)
  villagesOfSelectedCommune: Village[] = [];  //(Regions) all Villages of 1 selected Commune (only display after user has selected 1 Commune)

  //[Filter] Filter values for passing into API
  searchName: string | undefined;
  filterCampusId: number | undefined;
  maxPrice: number | undefined;
  minPrice: number | undefined;
  selectedRoomTypeIds: number[] = [];  //list of roomTypeId of roomTypes that got selected

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
    this.filterHouse();

    // (Paging) Count available Houses for total number of pages
    this.houseService.countTotalAvailableHouse().subscribe(data => {
      this.countAvailableHouses = data;

      // (Paging) Calculate number of pages
      this.pageCount = Math.ceil(this.countAvailableHouses / this.pageSize);  //divide & round up
      console.log(this.countAvailableHouses);

      // (Paging) Render pageList based on pageCount
      this.pageList = Array.from({ length: this.pageCount }, (_, i) => i + 1);
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
    this.houseUtilities = [
      { "utilityName": "FingerprintLock", "displayName": "Khóa vân tay" },
      { "utilityName": "Camera", "displayName": "Camera an ninh" },
      { "utilityName": "Parking", "displayName": "Chỗ để xe" },
    ];

    this.roomUtilities = [
      { "utilityName": "Fridge", "displayName": "Tủ lạnh" },
      { "utilityName": "Kitchen", "displayName": "Bếp" },
      { "utilityName": "WashingMachine", "displayName": "Máy giặt" },
      { "utilityName": "Desk", "displayName": "Bàn học" },
      { "utilityName": "LiveWithHost", "displayName": "Chung chủ" },
      { "utilityName": "Bed", "displayName": "Giường" },
      { "utilityName": "ClosedToilet", "displayName": "Vệ sinh khép kín" },
    ];
  }

  // Go to top of Page: used whenever user filter/paging data -> refresh list data
  scrollToTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth'
    });
  }

  // Call API to update list house with selected Filter value & Paging
  filterHouse(resetPaging: boolean = false) {
    //if user filter -> reset Paging (back to page 1)
    if (resetPaging) {
      this.pageNumber = 1;
    }

    this.houseService.filterAvailableHouses(
      this.pageSize,
      this.pageNumber,
      this.selectedRoomTypeIds,
      this.searchName,
      this.filterCampusId,
      this.maxPrice,
      this.minPrice,
    ).subscribe(data => {
      this.houses = data;
      this.scrollToTop();
    });
  }

  //[Paging] User click on a Page number -> Go to that page
  goToPage(pageNumber: number) {
    // Call API: go to Page Number
    this.pageNumber = pageNumber;
    this.filterHouse();
  }

  //Search House by Name (contains)
  searchHouseByName(searchHouseName: string) {
    //not perform search for empty input
    if (!searchHouseName.trim()) {
      return;
    }

    // Call API (filter by name contains)
    this.searchName = searchHouseName;
    this.filterHouse(true);
  }

  //[Filter] Filter by Campus
  onCampusSelected(selectedCampusId: string) {
    // convert string to number
    var numberCampusId: number = +selectedCampusId;

    // Call API: update list houses with the campus user chose
    this.filterCampusId = numberCampusId;
    this.filterHouse(true);
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

    // TODO: Call API to update list houses with the distance user chose
    alert('distance: ' + numMinDistance + ' ' + numMaxDistance);
  }

  //[Filter] Filter by Price
  onPriceSelected(minPriceString: string, maxPriceString: string) {
    // convert string to number
    var numMinPrice: number = +minPriceString;
    var numMaxPrice: number = +maxPriceString;

    // (special case) 0 or empty input of Max -> not handle
    if (numMaxPrice == 0) {
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
    this.maxPrice = numMaxPrice;
    this.minPrice = numMinPrice;
    this.filterHouse(true);
  }

  //[Filter] Filter by Room Type
  onRoomTypeSelected(event: any, roomTypeId: number) {
    //see if user just checked or unchecked the checkbox
    const isChecked = (<HTMLInputElement>event.target).checked;

    //if user check -> add roomTypeId to the list
    if (isChecked) {
      this.selectedRoomTypeIds.push(roomTypeId);
    } else {
      //if user uncheck -> remove the roomTypeId from the list
      const index = this.selectedRoomTypeIds.indexOf(roomTypeId, 0);
      if (index > -1) {
        this.selectedRoomTypeIds.splice(index, 1);
      }
    }

    // Call API to update list houses with the selected room type
    this.filterHouse(true);
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
  onRoomUtilitySelected(event: any, utilityName: string) {
    //see if user just checked or unchecked the checkbox
    const isChecked = (<HTMLInputElement>event.target).checked;

    // Call API to update list houses with the selected room utility
    alert('Event: ' + isChecked + ' Utility Name: ' + utilityName);
  }

  //[Filter] Filter by House Utility
  onHouseUtilitySelected(event: any, utilityName: string) {
    //see if user just checked or unchecked the checkbox
    const isChecked = (<HTMLInputElement>event.target).checked;

    // Call API to update list houses with the selected House utility
    alert('Event: ' + isChecked + ' Utility Name: ' + utilityName);
  }

  //[Filter] Cancel all Filter values
  onCancelFilter() {
    //reload page
    window.location.reload();
  }
}
