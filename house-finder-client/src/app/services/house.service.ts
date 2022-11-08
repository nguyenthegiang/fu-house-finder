import { House } from './../models/house';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { StringMap } from '@angular/compiler/src/compiler_facade_interface';

@Injectable({
  providedIn: 'root'
})
export class HouseService {
  readonly APIUrl = "https://localhost:5001/api/House";

  constructor(private http: HttpClient) { }

  //Unused
  //Get List of all Houses
  getAllHouses(): Observable<House[]> {
    return this.http.get<House[]>(this.APIUrl);
  }

  //Unused
  //Get List of available Houses
  getAvailableHouses(): Observable<House[]> {
    return this.http.get<House[]>(this.APIUrl + "/availableHouses");
  }

  //[Home Page] Filter available Houses using OData
  filterAvailableHouses(
    pageSize: number,
    pageNumber: number,
    selectedRoomTypeIds: number[],
    selectedHouseUtilities: string[],
    selectedRoomUtilities: string[],
    searchName?: string,
    campusId?: number,
    maxPrice?: number,
    minPrice?: number,
    selectedDistrictId?: number,
    selectedCommuneId?: number,
    selectedVillageId?: number,
    selectedRate?: number,
    selectedOrderValue?: string,
  ): Observable<House[]> {
    //define API here to append query options into it later
    var filterAPIUrl = this.APIUrl + `/availableHouses?`;

    //[Paging] count Skip and Top from pageSize & pageNumber
    const skip = pageSize * (pageNumber - 1);
    const top = pageSize;
    filterAPIUrl += `$skip=${skip}&$top=${top}`;

    //[Filter] check if user has at least 1 filter
    if (searchName || campusId || maxPrice || selectedRoomTypeIds.length > 0 ||
      selectedDistrictId || selectedCommuneId || selectedVillageId ||
      selectedHouseUtilities.length > 0 || selectedRoomUtilities.length > 0
      || selectedRate) {
      //add filter to API
      filterAPIUrl += `&$filter=`;
    }

    //[Filter] flag to check if that filter is the first filter (if is first -> not have 'and')
    var checkFirstFilter = true;

    //[Filter] add filter by name if has (contains name)
    if (searchName != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `contains(HouseName,'${searchName}')`;
    }

    //[Filter] add filter by campus if has
    if (campusId != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `CampusId eq ${campusId}`;
    }

    //[Filter] add filter by room price if has
    if (maxPrice != undefined && minPrice != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `LowestRoomPrice le ${maxPrice} and HighestRoomPrice ge ${minPrice}`;
    }

    //[Filter] add filter by roomType if has
    if (selectedRoomTypeIds != undefined && selectedRoomTypeIds.length > 0) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      //with each roomTypeId got selected -> append to query string
      for (let i = 0; i < selectedRoomTypeIds.length; i++) {
        filterAPIUrl += `contains(RoomTypeIds,'${selectedRoomTypeIds[i]}')`;
        //if isn't last roomTypeId -> need an 'or'
        if (i < selectedRoomTypeIds.length - 1) {
          filterAPIUrl += ' or ';
        }
      }
    }

    //[Filter] add filter by region if has
    if (selectedVillageId != undefined) {   //filter by Village
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `VillageId eq ${selectedVillageId}`;
    } else if (selectedCommuneId != undefined) {    //filter by Commune
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `CommuneId eq ${selectedCommuneId}`;
    } else if (selectedDistrictId != undefined) {    //filter by District
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `DistrictId eq ${selectedDistrictId}`;
    }

    //[Filter] add filter by houseUtility if has: select house with all of these houseUtility
    if (selectedHouseUtilities != undefined && selectedHouseUtilities.length > 0) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      //with each houseUtility got selected -> append to query string
      for (let i = 0; i < selectedHouseUtilities.length; i++) {
        filterAPIUrl += `${selectedHouseUtilities[i]} eq true`;
        //if isn't last houseUtility -> need an 'and'
        if (i < selectedHouseUtilities.length - 1) {
          filterAPIUrl += ' and ';
        }
      }
    }

    //[Filter] add filter by roomUtility if has: select house with all of these roomUtility
    if (selectedRoomUtilities != undefined && selectedRoomUtilities.length > 0) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      //with each roomUtility got selected -> append to query string
      for (let i = 0; i < selectedRoomUtilities.length; i++) {
        filterAPIUrl += `${selectedRoomUtilities[i]} eq true`;
        //if isn't last roomUtility -> need an 'and'
        if (i < selectedRoomUtilities.length - 1) {
          filterAPIUrl += ' and ';
        }
      }
    }

    //[Filter] add filter by Rate if has
    if (selectedRate != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `AverageRate ge ${selectedRate}`;
    }

    //[Order by] add Order by if has
    if (selectedOrderValue != undefined) {
      //this is not Filter so no need for 'and'

      //add suitable query string based on selected Order
      switch (selectedOrderValue) {
        case 'Giá: Thấp đến Cao':
          filterAPIUrl += `&$orderby=LowestRoomPrice asc`;
          break;
        case 'Giá: Cao đến Thấp':
          filterAPIUrl += `&$orderby=HighestRoomPrice desc`;
          break;
        case 'Khoảng cách: Gần đến Xa':
          //TODO: distance
          filterAPIUrl += ``;
          break;
        case 'Khoảng cách: Xa đến Gần':
          //TODO: distance
          filterAPIUrl += ``;
          break;
        case 'Đánh giá: Cao đến Thấp':
          filterAPIUrl += `&$orderby=AverageRate desc`;
          break;
        case 'Đánh giá: Thấp đến Cao':
          filterAPIUrl += `&$orderby=AverageRate asc`;
          break;
        default:
          break;
      }
    }

    return this.http.get<House[]>(filterAPIUrl);
  }

  //Unused
  //Search house by name
  searchHouseByName(houseName: string): Observable<any[]> {
    return this.http.get<any>(this.APIUrl + "/search?name=" + houseName);
  }

  //[House Detail] Get House detail information
  getHouseByHouseId(houseId: number): Observable<House> {
    return this.http.get<House>(this.APIUrl + "/" + houseId);
  }

  //[Dashboard] Get List Houses by Landlord Id
  getListHousesByLandlordId(landlordId: string): Observable<any[]> {
    return this.http.get<any>(this.APIUrl + "/GetHousesByLandlord?LandlordId=" + landlordId);
  }

  //[Landlord][List room] Get total money for not rented rooms
  getMoneyForNotRentedRooms(houseId: number): Observable<any> {
    return this.http.get<any>(this.APIUrl + "/GetMoneyForNotRentedRooms?HouseId=" + houseId);
  }

  //[Staff][Dashboard] Get total of houses
  getTotalHouse(): Observable<any> {
    return this.http.get<any>(this.APIUrl + "/CountTotalHouse");
  }

  //[Staff][Dashboard] Get total of available houses
  //[Home Page] For Paging
  countTotalAvailableHouse(): Observable<any> {
    return this.http.get<any>(this.APIUrl + "/CountAvailableHouse");
  }
}
