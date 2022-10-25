import { Campus } from './../models/campus';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CampusService {
  readonly APIUrl = "https://localhost:5001/api/Campus";

  constructor(private http: HttpClient) { }

  //[Home Page] Get List of all Campuses for Filter
  getAllCampuses(): Observable<Campus[]> {
    return this.http.get<Campus[]>(this.APIUrl);
  }
}
