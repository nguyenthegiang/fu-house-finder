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
  filterOrder(
    pageSize: number,
    pageNumber: number,
    statusId?: number): Observable<Order[]>{

    //[Paging] count Skip and Top from pageSize & pageNumber
    const skip = pageSize * (pageNumber - 1);
    const top = pageSize;

    //define API here to append query options into it later
    var filterAPIUrl = this.APIUrl;
    filterAPIUrl += `?$skip=${skip}&$top=${top}`;

    //[Filter] check if user has at least 1 filter
    if (statusId != null) {
      //add filter to API
      filterAPIUrl += `&$filter=`;
    }

    //[Filter] flag to check if that filter is the first filter (if is first -> not have 'and')
    var checkFirstFilter = true;

    //[Filter] add filter by campus if has
    if (statusId != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `Status/StatusId eq ${statusId}`;
    }

    return this.http.get<Order[]>(filterAPIUrl);
  }

  //[Staff/dashboard] Get total order by month for bar chart
  getTotalOrderByMonth():Observable<number[]>{
    return this.http.get<number[]>(this.APIUrl + "/GetTotalOrderByMonth");
  }

  //[Staff/dashboard] Get total order by month for bar chart
  getSolvedOrderByMonth():Observable<number[]>{
    return this.http.get<number[]>(this.APIUrl + "/GetSolvedOrderByMonth");
  }

  //[Staff/list-order] Filter orders by status
  filterOrderByStatus(status: boolean): Observable<Order[]>{
    return this.http.get<Order[]>(this.APIUrl + "/?$filter=Solved%20eq%20" + status);
  }
}
