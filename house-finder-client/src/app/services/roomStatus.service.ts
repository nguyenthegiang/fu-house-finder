import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';
import { RoomStatus } from '../models/roomStatus';

@Injectable({
  providedIn: 'root'
})
export default class RoomStatusService {

  readonly APIUrl = "https://localhost:5001/api/Status";

  constructor(private http: HttpClient) { }

  //[Landlord][List room] Get List of Statuses by HouseId
  getStatusesByHouseId(houseId: number): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + "/getByHouseId?HouseId=" + houseId);
  }
  getAllStatus(): Observable<RoomStatus[]> {
    return this.http.get<RoomStatus[]>(this.APIUrl);
  }
}
