import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
//environment variable for API URL
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommuneService {
  readonly APIUrl = `${environment.api_url}/Commune`;

  constructor(private http: HttpClient) { }

  countCommuneHavingHouse(): Observable<number> {
    return this.http.get<number>(this.APIUrl + "CountCommuneHavingHouse");
  }
}
