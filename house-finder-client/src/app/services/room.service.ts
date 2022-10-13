import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Room } from '../models/room';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  readonly APIUrl = "https://localhost:5001/api/Rooms";

  constructor(private http: HttpClient) {}

  //[House Detail] Get List Available Rooms by HouseId
  getAvailableRooms(houseId: number): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + "/getAvailableRooms?HouseId=" + houseId);
  }
}
