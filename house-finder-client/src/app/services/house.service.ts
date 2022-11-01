import { House } from './../models/house';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { StringMap } from '@angular/compiler/src/compiler_facade_interface';

@Injectable({
  providedIn: 'root'
})
export class HouseService {
  readonly APIUrl = "https://localhost:5001/api/Houses";

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
    searchName?: string,
    campusId?: number,
    maxPrice?: number,
    minPrice?: number,
  ): Observable<House[]> {
    //define API here to append query options into it later
    var filterAPIUrl = this.APIUrl + `/availableHouses?`;

    //[Paging] count Skip and Top from pageSize & pageNumber
    const skip = pageSize * (pageNumber - 1);
    const top = pageSize;
    filterAPIUrl += `$skip=${skip}&$top=${top}`;

    //[Filter] check if user has at least 1 filter
    if (searchName || campusId || maxPrice) {
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
