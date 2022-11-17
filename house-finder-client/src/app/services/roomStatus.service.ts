import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';
import { RoomStatus } from '../models/roomStatus';
//environment variable for API URL
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export default class RoomStatusService {

  readonly APIUrl = `${environment.api_url}/RoomStatus`;

  constructor(private http: HttpClient) { }

  //[Landlord][List room] Get List of Statuses by HouseId
  getStatusesByHouseId(houseId: number): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + "/getByHouseId?HouseId=" + houseId);
  }
  getAllStatus(): Observable<RoomStatus[]> {
    return this.http.get<RoomStatus[]>(this.APIUrl);
  }
}
