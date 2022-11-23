import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderStatus } from '../models/orderStatus';
//environment variable for API URL
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderStatusService {

  readonly APIUrl = `${environment.api_url}/OrderStatus`;

  constructor(private http: HttpClient) { }

  getAllStatus(): Observable<OrderStatus[]> {
    return this.http.get<OrderStatus[]>(this.APIUrl);
  }

}
