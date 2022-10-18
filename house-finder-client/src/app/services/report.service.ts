import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { Report } from '../models/report';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  readonly APIUrl = "https://localhost:5001/api/Report";

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  //POST: [Report] Send Report
  addReport(report: Report): Observable<any> {
    return this.http.post<any>(this.APIUrl, report, this.httpOptions);
  }

}
