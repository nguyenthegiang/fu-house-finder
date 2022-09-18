import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CampusService {
  readonly APIUrl = "https://localhost:44363/api/Campus";

  constructor(private http: HttpClient) { }

  getAllCampuses(): Observable<any[]> {
    return this.http.get<any>(this.APIUrl);
  }
}
