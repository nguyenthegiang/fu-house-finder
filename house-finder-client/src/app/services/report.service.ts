import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Report } from '../models/report';
import { StaffReport } from '../models/staffReport';
//environment variable for API URL
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  readonly APIUrl = `${environment.api_url}/Report`;

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  //POST: [Report] Send Report
  addReport(report: Report): Observable<any> {
    return this.http.post<any>(this.APIUrl, report, this.httpOptions);
  }

  //[Staff/Dashboard]
  getTotalReportByMonth():Observable<any>{
    return this.http.get<any>(this.APIUrl + "/GetTotalReportByMonth");
  }

  //[Staff/list-report] Get list of all reports
  getAllReport():Observable<StaffReport[]>{
    console.log(this.APIUrl);
    return this.http.get<StaffReport[]>(this.APIUrl);
  }

  //[Staff/list-report] Search reports by house's name
  searchReportByHouseName(key: string):Observable<StaffReport[]>{
    return this.http.get<StaffReport[]>(this.APIUrl + "/SearchReportByHouseName/" + key);
  }

  //[Staff/list-report] Count total report by house id
  countTotalReportByHouseId(houseId: number):Observable<number>{
    return this.http.get<number>(this.APIUrl + "/CountTotalReportByHouseId/" + houseId);
  }

}
