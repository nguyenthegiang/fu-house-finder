import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
//environment variable for API URL
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VillageService {

  readonly APIUrl = `${environment.api_url}/Village`;

  constructor(private http: HttpClient) { }

  countVillageHavingHouse(): Observable<number> {
    return this.http.get<number>(this.APIUrl + "/CountVillageHavingHouse");
  }
}
