import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class RoomTypeService {

  readonly APIUrl = "https://localhost:5001";

  constructor(private http: HttpClient) { }

  //[Landlord][List room] Get List of Room Types
  getRoomTypes(): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl + "/roomType/Get?$select=RoomTypeName");
  }
}
