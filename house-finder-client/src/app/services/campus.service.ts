import { Campus } from './../models/campus';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
//environment variable for API URL
import { environment } from 'src/environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class CampusService {
  readonly APIUrl = `${environment.api_url}/Campus`;

  constructor(private http: HttpClient) { }

  //[Home Page] (for Filter) Get all Campuses, Districts, Communes & Villages
  getAllCampuses(): Observable<Campus[]> {
    return this.http.get<Campus[]>(this.APIUrl);
  }
}
