import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StatusService {

  readonly APIUrl = "https://localhost:5001/api/Status";

  constructor(private http: HttpClient) { }

  //[Landlord][List room] Get List of Statuses by HouseId
  getStatusesByHouseId(houseId: number): Observable<any[]>{
    return this.http.get<any[]>(this.APIUrl + "/getByHouseId?HouseId=" + houseId);
  }
}
