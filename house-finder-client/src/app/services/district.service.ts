import { District } from './../models/district';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DistrictService {

  readonly APIUrl = "https://localhost:5001/api/District";

  constructor(private http: HttpClient) { }

  //[Home Page] (for Filter) Get all Districts, Communes & Villages
  getAllDistricts(): Observable<District[]> {
    return this.http.get<District[]>(this.APIUrl);
  }
}
