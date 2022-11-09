import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Report } from 'src/app/models/report';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-list-report',
  templateUrl: './list-report.component.html',
  styleUrls: ['./list-report.component.scss']
})

export class ListReportComponent implements OnInit
{
  //{Search} input value
  searchLandlordName: any;
  //List all report
  reports: Report[] = [];
  constructor(private reportService: ReportService,
    private router: Router,)
  { }

  ngOnInit(): void
  {
    //Call API: get all reports of this house
    this.reportService.getAllReport().subscribe(data => {
      this.reports = data;
    });
  }

  searchReport()
  {}

  viewReportDetail(id: number)
  {
    console.log(id);
    this.router.navigate(['/Staff/staff-report-detail/' + id]);
  }
}
