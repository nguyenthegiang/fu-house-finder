import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Order } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  readonly APIUrl = "https://localhost:5001/api/Order";

  constructor(private http: HttpClient) { }

  //[Staff/list-order] Get List of all Orders
  getAllOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(this.APIUrl);
  }
}
