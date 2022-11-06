import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OrderStatus } from '../models/orderStatus';

@Injectable({
  providedIn: 'root'
})
export class OrderStatusService {

  readonly APIUrl = "https://localhost:5001/api/OrderStatus";

  constructor(private http: HttpClient) { }

  getAllStatus(): Observable<OrderStatus[]> {
    return this.http.get<OrderStatus[]>(this.APIUrl);
  }
}
