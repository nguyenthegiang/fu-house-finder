import { LandlordInfo } from './../models/landlord';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
//environment variable for API URL
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LandlordInformationService {
  readonly APIUrl = `${environment.api_url}/LandLordInfomation`;

  constructor(private http: HttpClient) { }

  /**
   * Get Landlord Information by LandlordId
   * [Staff]
   */
  getLandLordInfomation(landlordId: string): Observable<LandlordInfo> {
    return this.http.get<LandlordInfo>(this.APIUrl + "/LandlordInfo?LandlordId=" + landlordId);
  }

  /**
   * Get current Landlord Information using session
   * [Landlord/dashboard]
   */
  getCurrentLandlordInfo(): Observable<LandlordInfo> {
    return this.http.get<LandlordInfo>(this.APIUrl + "/GetCurrentLandlordInfo", { withCredentials: true });
  }
}
