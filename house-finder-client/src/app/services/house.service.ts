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

  //[Home Page] Get List Houses
  getAllHouses(): Observable<any[]> {
    return this.http.get<any>(this.APIUrl);
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
  getMoneyForNotRentedRooms(houseId: number): Observable<any>{
    return this.http.get<any>(this.APIUrl + "/GetMoneyForNotRentedRooms?HouseId=" + houseId);
  }
}
