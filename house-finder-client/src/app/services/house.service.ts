import { House } from './../models/house';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

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
  filterAvailableHouses(pageSize: number = 9, pageNumber: number = 1, searchName?: string): Observable<House[]> {
    //count Skip and Top from pageSize & pageNumber
    const skip = pageSize * (pageNumber - 1);
    const top = pageSize;
    
    return this.http.get<House[]>(this.APIUrl + `/availableHouses?$skip=${skip}&$top=${top}`);
  }

  //[Home Page] Search house by name
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
