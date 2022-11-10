import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
//environment variable for API URL
import { environment } from 'src/environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class RoomTypeService {

  readonly APIUrl = `${environment.api_url}/RoomType`;

  constructor(private http: HttpClient) { }

  //[Landlord][List room] Get List of Room Types by HouseId
  getRoomTypesByHouseId(houseId: number): Observable<any[]>{
    return this.http.get<any[]>(this.APIUrl + "/getByHouseId?HouseId=" + houseId);
  }

  //[Homepage] Get List of Room Types
  getRoomTypes(): Observable<any[]> {
    return this.http.get<any[]>(this.APIUrl);
  }
}
