import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Room } from '../models/room';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  readonly APIUrl = "https://localhost:5001/api/Rooms";

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  //[House Detail] Get List Available Rooms by HouseId
  getAvailableRooms(houseId: number): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + "/getAvailableRooms?HouseId=" + houseId);
  }

  //[Landlord: List Room] Get List of Rooms by HouseId
  getRooms(houseId: number): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + "/getByHouseId?HouseId=" + houseId);
  }

  //[Landlord: Create Room]
  addRoom(room: Room): Observable<any> {
    return this.http.post<any>(this.APIUrl, room, this.httpOptions);
  }

  //[Landlord: Update Room]
  updateRoom(room: Room): Observable<any> {
    return this.http.put<any>(this.APIUrl, room, this.httpOptions);
  }

  //[Landlord: Delete Room]
  deleteRoom(roomId: number): Observable<any> {
    return this.http.delete<any>(this.APIUrl + "/Rooms?roomId=" + roomId, this.httpOptions);
  }

  //[Landlord: Get Room] Get room by roomId
  getRoomByRoomId(roomId: number): Observable<Room> {
    return this.http.get<Room>(this.APIUrl + "/getByRoomId?RoomId=" + roomId);
  }

  //[Staff/Dashboard] Count available rooms
  countAvailableRooms(): Observable<any>{
    return this.http.get<Room>(this.APIUrl + "/CountAvailableRoom");
  }

  //[Staff/Dashboard] Count available capacity
  countAvailableCapacity(): Observable<any>{
    return this.http.get<Room>(this.APIUrl + "/CountAvailableCapacity");
  }
}

