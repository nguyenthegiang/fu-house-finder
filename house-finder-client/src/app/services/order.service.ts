import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Order } from '../models/order';
//environment variable for API URL
import { environment } from 'src/environments/environment';
import { CreateOrder } from '../models/createOrder';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  readonly APIUrl = `${environment.api_url}/Order`;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    withCredentials: true
  };

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
    statusId?: number,
    fromDate?: string,
    toDate?: string,
    orderBy?: string): Observable<Order[]> {

    //[Paging] count Skip and Top from pageSize & pageNumber
    const skip = pageSize * (pageNumber - 1);
    const top = pageSize;

    //define API here to append query options into it later
    var filterAPIUrl = this.APIUrl;
    filterAPIUrl += `?$skip=${skip}&$top=${top}`;

    //[Filter] check if user has at least 1 filter
    if ((statusId != undefined && statusId != 0) || fromDate || toDate) {
      //add filter to API
      filterAPIUrl += `&$filter=`;
    }

    //[Filter] flag to check if that filter is the first filter (if is first -> not have 'and')
    var checkFirstFilter = true;

    //[Filter] add filter by status if has
    if (statusId != undefined && statusId != 0) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }
      filterAPIUrl += `Status/StatusId eq ${statusId}`;
    }

    //[Filter] add filter by date if it has
    if (fromDate != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }
      filterAPIUrl += `OrderedDate ge ${fromDate}`;
    }

    if (toDate != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }
      filterAPIUrl += `OrderedDate le ${toDate}`;
    }

    if (orderBy != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }
      filterAPIUrl += `&$orderby=OrderedDate ${orderBy}`;
    }


    return this.http.get<Order[]>(filterAPIUrl);
  }

  //[Staff/dashboard] Get total order by month for bar chart
  getTotalOrderByMonth(): Observable<number[]> {
    return this.http.get<number[]>(this.APIUrl + "/GetTotalOrderByMonth");
  }

  //[Staff/dashboard] Get total order by month for bar chart
  getSolvedOrderByMonth(): Observable<number[]> {
    return this.http.get<number[]>(this.APIUrl + "/GetSolvedOrderByMonth");
  }

  //[Staff/list-order] Filter orders by status
  filterOrderByStatus(status: boolean): Observable<Order[]> {
    return this.http.get<Order[]>(this.APIUrl + "/?$filter=Solved%20eq%20" + status);
  }

  //[Staff/list-order] Update order's status
  updateOrderStatus(orderId: number, statusId: number): Observable<any> {
    return this.http.put<any>(this.APIUrl + "/" + orderId + "/" + statusId, this.httpOptions, {withCredentials: true});
  }

  //POST Order for user
  addOrder(order: CreateOrder): Observable<any> {
    return this.http.post<any>(this.APIUrl, order, this.httpOptions);
  }

  //Count total order solved by current staff
  countTotalOrderSolvedByAccount() : Observable<number>{
    return this.http.get<number>(this.APIUrl + "/CountTotalOrderSolvedBy", {withCredentials: true});
  }

  CountSolvedOrderByStaffInAYear() : Observable<number[]>{
    return this.http.get<number[]>(this.APIUrl + "/CountSolvedOrderByStaffInAYear", {withCredentials: true});
  }
}
