import { HouseHomePage } from './../../models/houseHomePage';
import { Campus } from './../../models/campus';
import { Router } from '@angular/router';
import { RoomService } from 'src/app/services/room.service';
import { Village } from '../../models/village';
import { Commune } from '../../models/commune';
import { District } from '../../models/district';
import { DistrictService } from '../../services/district.service';
import { OtherUtility } from '../../models/otherUtilities';
import { CampusService } from '../../services/campus.service';
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
  houses: HouseHomePage[] | undefined;

  //to display in [Statistics]
  countAvailableRooms: number = 0;
  countAvailableCapacity: number = 0;

  //to display in count
  countFilterHouses: number = 0;

  //(Paging)
  countAvailableHouses = 0; //items count
  pageSize = 9;             //number of items per page
  pageNumber = 1;           //starts at page 1
  pageCount = 0;            //number of pages
  pageList: number[] = [];  //array to loop with *ngFor in HTML Template

  //[Filter] Data for Filter column
  roomTypes: RoomType[] = [];         //Room types
  houseUtilities: OtherUtility[] = [];  //List of utilities of Houses
  roomUtilities: OtherUtility[] = [];  //List of utilities of Rooms
  campuses: Campus[] = [];                    //(Regions) All Campuses (and Districts, Communes, Villages)
  districtsOfSelectedCampus: District[] = []; //(Regions) all Districts of 1 selected Campus (only display after user has selected 1 Campus)
  communesOfSelectedDistrict: Commune[] = []; //(Regions) all Communes of 1 selected District (only display after user has selected 1 District)
  villagesOfSelectedCommune: Village[] = [];  //(Regions) all Villages of 1 selected Commune (only display after user has selected 1 Commune)
  //[Order by] Data for Order select
  orderValues: string[] = [];

  //[Filter] Filter values for passing into API
  searchName: string | undefined;           //(filter by name)
  selectedCampusId: number | undefined;     //(filter by campus)
  maxPrice: number | undefined;             //(filter by price)
  minPrice: number | undefined;             //(filter by price)
  selectedRoomTypeIds: number[] = [];       //(filter by roomType)    //list of roomTypeId of roomTypes that got selected
  selectedHouseUtilities: string[] = [];    //(filter by Utility)     //list of Utilities of House that got selected
  selectedRoomUtilities: string[] = [];     //(filter by Utility)     //list of Utilities of Room that got selected
  selectedDistrictId: number | undefined;   //(filter by Region)
  selectedCommuneId: number | undefined;    //(filter by Region)
  selectedVillageId: number | undefined;    //(filter by Region)
  selectedRate: number | undefined;         //(filter by Rate)
  maxDistance: number | undefined;          //(filter by Distance)
  minDistance: number | undefined;          //(filter by Distance)
  //[Order by] Selected Order value
  selectedOrderValue: string | undefined;

  //For Placeholder
  placeHolderItemCount: number[] = [];

  constructor(
    private houseService: HouseService,
    private campusService: CampusService,
    private roomTypeService: RoomTypeService,
    private roomService: RoomService,
  ) { }

  ngOnInit(): void {
    //init array for placeholder loop
    this.placeHolderItemCount = Array.from({ length: this.pageSize }, (_, i) => i + 1);

    //Call APIs:

    //(List) Get available Houses - default: page 1, 9 items
    this.filterHouse(false);

    //(Statistics)
    this.roomService.countAvailableRooms().subscribe(data => {
      this.countAvailableRooms = data;
    });
    this.roomService.countAvailableCapacity().subscribe(data => {
      this.countAvailableCapacity = data;
    });
    this.houseService.countTotalAvailableHouse().subscribe(data => {
      this.countAvailableHouses = data;
    });

    //(Filter) Get all Campuses (with their Districts, Communes, Villages)
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
    this.houseUtilities = [
      { utilityName: "FingerprintLock", displayName: "Khóa vân tay" },
      { utilityName: "Camera", displayName: "Camera an ninh" },
      { utilityName: "Parking", displayName: "Chỗ để xe" },
    ];

    this.roomUtilities = [
      { utilityName: "Fridge", displayName: "Tủ lạnh" },
      { utilityName: "Kitchen", displayName: "Bếp" },
      { utilityName: "WashingMachine", displayName: "Máy giặt" },
      { utilityName: "Desk", displayName: "Bàn học" },
      { utilityName: "NoLiveWithHost", displayName: "Không chung chủ" },
      { utilityName: "Bed", displayName: "Giường" },
      { utilityName: "ClosedToilet", displayName: "Vệ sinh khép kín" },
    ];

    //(Order by) Order values
    this.orderValues = [
      "Giá: Thấp đến Cao",
      "Giá: Cao đến Thấp",
      "Khoảng cách: Gần đến Xa",
      "Khoảng cách: Xa đến Gần",
      "Đánh giá: Cao đến Thấp",
      "Đánh giá: Thấp đến Cao",
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
    //Before calling API: reset list house to change screen to loading
    this.houses = undefined;
    this.countFilterHouses = 0;

    //if user filter -> reset Paging (back to page 1)
    if (resetPaging) {
      this.pageNumber = 1;
    }

    //Get data
    this.houseService.filterAvailableHouses(
      this.pageSize,
      this.pageNumber,
      this.selectedRoomTypeIds,
      this.selectedHouseUtilities,
      this.selectedRoomUtilities,
      this.searchName,
      this.selectedCampusId,
      this.maxPrice,
      this.minPrice,
      this.maxDistance,
      this.minDistance,
      this.selectedDistrictId,
      this.selectedCommuneId,
      this.selectedVillageId,
      this.selectedRate,
      this.selectedOrderValue,
    ).subscribe(data => {
      this.houses = data;
      this.scrollToTop();
    });

    //For Paging: Count total number of items
    this.houseService.filterAvailableHouses(
      1000,
      1,
      this.selectedRoomTypeIds,
      this.selectedHouseUtilities,
      this.selectedRoomUtilities,
      this.searchName,
      this.selectedCampusId,
      this.maxPrice,
      this.minPrice,
      this.maxDistance,
      this.minDistance,
      this.selectedDistrictId,
      this.selectedCommuneId,
      this.selectedVillageId,
      this.selectedRate,
      this.selectedOrderValue,
    ).subscribe(data => {
      //Store list house count
      this.countFilterHouses = data.length;

      // (Paging) Calculate number of pages
      this.pageCount = Math.ceil(data.length / this.pageSize);  //divide & round up

      // (Paging) Render pageList based on pageCount
      this.pageList = Array.from({ length: this.pageCount }, (_, i) => i + 1);
      //pageList is now an array like {1, 2, 3, ..., n | n = pageCount}
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

    // find the selected campus
    this.campuses.forEach((campus) => {
      // assign the list of Commune as the communes of this District
      if (campus.campusId == numberCampusId) {
        this.districtsOfSelectedCampus = campus.districts;
        return;
      }
    });

    //Reset lower Region Filter
    this.selectedDistrictId = undefined;
    this.selectedCommuneId = undefined;
    this.selectedVillageId = undefined;
    this.communesOfSelectedDistrict = [];
    this.villagesOfSelectedCommune = [];

    // Call API: update list houses with the campus user chose
    this.selectedCampusId = numberCampusId;
    this.filterHouse(true);
  }

  // When user clicked on select District
  onDistrictClicked() {
    // check if user has chosen Campus
    if (!this.selectedCampusId) {
      // if not => alert that they have to choose Campus before District
      alert('Vui lòng chọn Cơ sở bạn muốn tìm trước');
    }
  }

  //[Filter by Region] Filter by Commune
  //Change list of Communes after user selected District
  onDistrictSelected(stringSelectedDistrictId: string) {
    // convert string to number
    var numberDistrictId: number = +stringSelectedDistrictId;

    // find the selected district
    this.districtsOfSelectedCampus.forEach((district) => {
      // assign the list of Commune as the communes of this District
      if (district.districtId == numberDistrictId) {
        this.communesOfSelectedDistrict = district.communes;
        return;
      }
    });

    //Reset lower Region Filter
    this.villagesOfSelectedCommune = [];

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

  //[Filter] Filter by Distance
  onDistanceSelected(minDistance: string, maxDistance: string) {
    // convert string to number
    var numMinDistance: number = +minDistance;
    var numMaxDistance: number = +maxDistance;

    // (special case) 0 or empty input -> not handle
    if (numMinDistance == 0 || numMaxDistance == 0) {
      return;
    }

    /* Only allow filtering by Distance after choosing Campus */
    if (!this.selectedCampusId) {
      alert('Vui lòng chọn Cơ sở bạn học trước!');
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
    this.maxDistance = numMaxDistance;
    this.minDistance = numMinDistance;
    this.filterHouse(true);
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

  //[Filter] Filter by Rate
  onRateSelected(rate: number) {
    // Call API to update list houses with the selected rate
    this.selectedRate = rate;
    this.filterHouse(true);
  }

  //[Order by] Order by values
  onOrderBy(selectedOrder: string) {
    // Call API to update list houses with the selected order
    this.selectedOrderValue = selectedOrder;
    this.filterHouse(true);
  }

  // //[Filter] Cancel all Filter values
  // onCancelFilter() {
  //   //reload page
  //   window.location.reload();
  // }
}
