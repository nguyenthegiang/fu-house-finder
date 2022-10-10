import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HouseService {
  readonly APIUrl = "https://localhost:5001/api/House";

  constructor(private http: HttpClient) { }

  getAllHouses(): Observable<any[]> {
    return this.http.get<any>(this.APIUrl);
  }
}
