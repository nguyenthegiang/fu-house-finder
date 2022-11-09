import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Room } from '../models/room';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  readonly APIUrl = "https://localhost:5001/api/Room";

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

  updateStatusRoom(statusId: number, roomId: number): Observable<any> {
    return this.http.put<any>(this.APIUrl + "/changeStatus?statusId=" + statusId + "&roomId=" + roomId, this.httpOptions);
  }

  //[Landlord: Delete Room]
  deleteRoom(roomId: number): Observable<any> {
    return this.http.delete<any>(this.APIUrl + "/Rooms?roomId=" + roomId, this.httpOptions);
  }

  //[Landlord: Get Room] Get room by roomId
  getRoomByRoomId(roomId: number): Observable<Room> {
    return this.http.get<Room>(this.APIUrl + "/getByRoomId?RoomId=" + roomId);
  }

  //[Home Page] [Staff/Dashboard] Count total available rooms
  countAvailableRooms(): Observable<number> {
    return this.http.get<number>(this.APIUrl + "/CountAvailableRoom");
  }

  //[Staff/Dashboard] Count available capacity
  countAvailableCapacity(): Observable<any> {
    return this.http.get<any>(this.APIUrl + "/CountAvailableCapacity");
  }

  //[Staff/Dashboard] Count total rooms
  CountTotalRoom(): Observable<any> {
    return this.http.get<any>(this.APIUrl + "/CountTotalRoom");
  }

  //[Staff/Dashboard] Count total rooms
  CountTotalCapacity(): Observable<any>{
    return this.http.get<any>(this.APIUrl + "/CountTotalCapacity");
  }

  //[Staff/Dashboard] Count total rooms
  CountTotallyAvailableCapacity(): Observable<any>{
    return this.http.get<any>(this.APIUrl + "/CountTotallyAvailableCapacity");
  }


  //[Homepage] Count totally available rooms by house id
  countTotallyAvailableRoomByHouseId(houseId: number): Observable<any> {
    return this.http.get<Room>(this.APIUrl + "/CountTotallyAvailableRoomByHouseId?houseId=" + houseId);
  }
}

