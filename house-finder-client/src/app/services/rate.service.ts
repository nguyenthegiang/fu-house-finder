import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Rate } from '../models/rate';

@Injectable({
  providedIn: 'root'
})
export class RateService {
  readonly APIUrl = `${environment.api_url}/Rate`;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    withCredentials: true
  };

  constructor(private http: HttpClient) { }

  /**
   * [House Detail] Create Rate
   */
  addRate(houseId: number, star: number, comment: string): Observable<any> {
    return this.http.post<any>(this.APIUrl + "?houseId=" + houseId + "&star=" + star + "&comment=" + comment, this.httpOptions);
  }
}
