import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Report } from '../models/report';
import { StaffReport } from '../models/staffReport';
//environment variable for API URL
import { environment } from 'src/environments/environment';
import { ReportHouse } from '../models/reportHouse';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  readonly APIUrl = `${environment.api_url}/Report`;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {}

  //POST: [Report] Send Report
  addReport(report: Report): Observable<any> {
    return this.http.post<any>(this.APIUrl, report, this.httpOptions);
  }

  //[Staff/Dashboard]
  getTotalReportByMonth(): Observable<any> {
    return this.http.get<any>(this.APIUrl + '/GetTotalReportByMonth');
  }

  //[Staff/list-report] Get list of all reports
  getAllReport(): Observable<StaffReport[]> {
    console.log(this.APIUrl);
    return this.http.get<StaffReport[]>(this.APIUrl);
  }

  //[Staff/list-report] Search reports by house's name
  searchReportByHouseName(key: string): Observable<StaffReport[]> {
    return this.http.get<StaffReport[]>(
      this.APIUrl + '/SearchReportByHouseName/' + key
    );
  }

  //[Staff/list-report] Count total report by house id
  countTotalReportByHouseId(houseId: number): Observable<number> {
    return this.http.get<number>(
      this.APIUrl + '/CountTotalReportByHouseId/' + houseId
    );
  }

  //[Staff/list-report] Count total of report
  countTotalReport(): Observable<number> {
    return this.http.get<number>(this.APIUrl + '/CountTotalReport');
  }

  //[Staff/list-report] Filter reports
  filterReport(
    pageSize: number,
    pageNumber: number,
    fromDate?: string,
    toDate?: string,
    orderBy?: string,
    searchName?: string,
  ): Observable<StaffReport[]> {
    //[Paging] count Skip and Top from pageSize & pageNumber
    const skip = pageSize * (pageNumber - 1);
    const top = pageSize;

    //define API here to append query options into it later
    var filterAPIUrl = this.APIUrl;
    filterAPIUrl += `?$skip=${skip}&$top=${top}`;

    //[Filter] check if user has at least 1 filter
    if (fromDate || toDate || searchName) {
      //add filter to API
      filterAPIUrl += `&$filter=`;
    }

    //[Filter] flag to check if that filter is the first filter (if is first -> not have 'and')
    var checkFirstFilter = true;

    //[Filter] add filter by name if has (contains name)
    if (searchName != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `contains(ReportContent, '${searchName}')`;
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
      filterAPIUrl += `ReportedDate ge ${fromDate}`;
    }

    if (toDate != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }
      filterAPIUrl += `ReportedDate le ${toDate}`;
    }

    if (orderBy != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }
      filterAPIUrl += `&$orderby=ReportedDate ${orderBy}`;
    }

    console.log(filterAPIUrl);
    return this.http.get<StaffReport[]>(filterAPIUrl);
  }
}
