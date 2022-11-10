import { LandlordInfo } from './../models/landlord';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
//environment variable for API URL
import { environment } from 'src/environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class LandlordInformationService
{
  readonly APIUrl = `${environment.api_url}/LandLordInfomation`;

  constructor(private http: HttpClient) { }

  //[Dashboard] Get Landlord Information
  getLandLordInfomation(landlordId: string): Observable<LandlordInfo> {
    return this.http.get<LandlordInfo>(this.APIUrl + "/LandlordInfo?LandlordId=" + landlordId);
  }
}
