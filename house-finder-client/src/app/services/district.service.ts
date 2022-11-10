import { District } from './../models/district';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
//environment variable for API URL
import { environment } from 'src/environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class DistrictService {

  readonly APIUrl = `${environment.api_url}/District`;

  constructor(private http: HttpClient) { }

  //[Home Page] (for Filter) Get all Districts, Communes & Villages
  getAllDistricts(): Observable<District[]> {
    return this.http.get<District[]>(this.APIUrl);
  }
}
