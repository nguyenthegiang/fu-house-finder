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
  getAllOrders(): Observable<any> {
    return this.http.get<any>(this.APIUrl);
  }

  //[Staff/list-order] Count total orders
  countTotalOrder(): Observable<any> {
    return this.http.get<any>(this.APIUrl + "/CountTotalOrder");
  }

  //[Staff/list-order]
  getListOrderForPaging(
    pageSize: number,
    pageNumber: number,): Observable<Order[]>{
    //[Paging] count Skip and Top from pageSize & pageNumber
    const skip = pageSize * (pageNumber - 1);
    const top = pageSize;
    //define API here to append query options into it later
    var filterAPIUrl = this.APIUrl;
    filterAPIUrl += `?$skip=${skip}&$top=${top}`;
    return this.http.get<Order[]>(filterAPIUrl);
  }

  //[Staff/dashboard] Get total order by month for bar chart
  getTotalOrderByMonth():Observable<number[]>{

    console.log("Get here: getTotalOrderByMonth");
    console.log(this.APIUrl + "/GetTotalOrderByMonth");
    return this.http.get<number[]>(this.APIUrl + "/GetTotalOrderByMonth");
  }
}
