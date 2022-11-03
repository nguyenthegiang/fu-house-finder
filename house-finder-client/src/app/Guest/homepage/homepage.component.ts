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
  searchName: string | undefined;         //(filter by name)
  filterCampusId: number | undefined;     //(filter by campus)
  maxPrice: number | undefined;           //(filter by price)
  minPrice: number | undefined;           //(filter by price)
  selectedRoomTypeIds: number[] = [];     //(filter by roomType)    //list of roomTypeId of roomTypes that got selected
  selectedHouseUtilities: string[] = [];  //(filter by Utility)     //list of Utilities of House that got selected
  selectedRoomUtilities: string[] = [];   //(filter by Utility)     //list of Utilities of Room that got selected
  selectedDistrictId: number | undefined;   //(filter by Region)
  selectedCommuneId: number | undefined;    //(filter by Region)
  selectedVillageId: number | undefined;    //(filter by Region)

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
    this.filterHouse(false);

    // (Paging) Count available Houses for total number of pages
    this.houseService.countTotalAvailableHouse().subscribe(data => {
      this.countAvailableHouses = data;
      console.log(data);

      // (Paging) Calculate number of pages
      this.pageCount = Math.ceil(this.countAvailableHouses / this.pageSize);  //divide & round up

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
      { "utilityName": "NoLiveWithHost", "displayName": "Không chung chủ" },
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
  filterHouse(resetPaging: boolean) {
    //if user filter -> reset Paging (back to page 1)
    if (resetPaging) {
      this.pageNumber = 1;
    }

    this.houseService.filterAvailableHouses(
      this.pageSize,
      this.pageNumber,
      this.selectedRoomTypeIds,
      this.selectedHouseUtilities,
      this.selectedRoomUtilities,
      this.searchName,
      this.filterCampusId,
      this.maxPrice,
      this.minPrice,
      this.selectedDistrictId,
      this.selectedCommuneId,
      this.selectedVillageId,
    ).subscribe(data => {
      this.houses = data;
      this.scrollToTop();
    });
  }

  //[Paging] User click on a Page number -> Go to that page
  goToPage(pageNumber: number) {
    // Call API: go to Page Number
    this.pageNumber = pageNumber;
    this.filterHouse(false);
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

  //[Filter by Region] Filter by Commune
  //Change list of Communes after user selected District
  onDistrictSelected(stringSelectedDistrictId: string) {
    // convert string to number
    var numberDistrictId: number = +stringSelectedDistrictId;

    // find the selected district
    this.districts.forEach((district) => {
      // assign the list of Commune as the communes of this District
      if (district.districtId == numberDistrictId) {
        this.communesOfSelectedDistrict = district.communes;
        return;
      }
    });

    //no need for filtering by commune & village 
    this.selectedCommuneId = undefined; 
    this.selectedVillageId = undefined;
    // Call API to update list houses with the selected district
    this.selectedDistrictId = numberDistrictId;
    this.filterHouse(true);
  }

  //[Filter by Region] Filter by Commune
  //Change list of Villages after user selected Commune
  onCommuneSelected(stringSelectedCommuneId: string) {
    // convert string to number
    var numberCommuneId: number = +stringSelectedCommuneId;

    // find the selected commune
    this.communesOfSelectedDistrict.forEach((commune) => {
      // assign the list of Villages as the villages of this Commune
      if (commune.communeId == numberCommuneId) {
        this.villagesOfSelectedCommune = commune.villages;
        return;
      }
    });

    //no need for filtering by district & village 
    this.selectedDistrictId = undefined; 
    this.selectedVillageId = undefined;
    // Call API to update list houses with the selected commune
    this.selectedCommuneId = numberCommuneId;
    this.filterHouse(true);
  }

  //[Filter by Region] Filter by Village
  onVillageSelected(stringSelectedVillageId: string) {
    // convert string to number
    var numberVillageId: number = +stringSelectedVillageId;

    //no need for filtering by district & commune 
    this.selectedDistrictId = undefined; 
    this.selectedCommuneId = undefined;
    // Call API to update list houses with the selected village
    this.selectedVillageId = numberVillageId;
    this.filterHouse(true);
  }

  //[Filter] Filter by House Utility
  onHouseUtilitySelected(event: any, utilityName: string) {
    //see if user just checked or unchecked the checkbox
    const isChecked = (<HTMLInputElement>event.target).checked;

    //if user check -> add houseUtility to the list
    if (isChecked) {
      this.selectedHouseUtilities.push(utilityName);
    } else {
      //if user uncheck -> remove the houseUtility from the list
      const index = this.selectedHouseUtilities.indexOf(utilityName, 0);
      if (index > -1) {
        this.selectedHouseUtilities.splice(index, 1);
      }
    }

    // Call API to update list houses with the selected room type
    this.filterHouse(true);
  }

  //[Filter] Filter by Room Utility
  onRoomUtilitySelected(event: any, utilityName: string) {
    //see if user just checked or unchecked the checkbox
    const isChecked = (<HTMLInputElement>event.target).checked;

    //if user check -> add roomUtility to the list
    if (isChecked) {
      this.selectedRoomUtilities.push(utilityName);
    } else {
      //if user uncheck -> remove the roomUtility from the list
      const index = this.selectedRoomUtilities.indexOf(utilityName, 0);
      if (index > -1) {
        this.selectedRoomUtilities.splice(index, 1);
      }
    }

    // Call API to update list houses with the selected room type
    this.filterHouse(true);
  }

  //[Filter] Cancel all Filter values
  onCancelFilter() {
    //reload page
    window.location.reload();
  }
}
