import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Report } from 'src/app/models/report';
import { ReportHouse } from 'src/app/models/reportHouse';
import { StaffReport } from 'src/app/models/staffReport';
import { HouseService } from 'src/app/services/house.service';
import { ReportService } from 'src/app/services/report.service';

@Component({
  selector: 'app-list-report',
  templateUrl: './list-report.component.html',
  styleUrls: ['./list-report.component.scss']
})

export class ListReportComponent implements OnInit
{
  //{Search} input value
  searchValue: string | undefined;
  //List all report
  houses: ReportHouse[] = [];

  constructor(private reportService: ReportService,
    private houseService: HouseService,
    private router: Router,)
  { }

  ngOnInit(): void
  {
    //Call API: get all reports of this house
    this.houseService.getReportHouse().subscribe(data => {
      this.houses = data;
    });
  }

  search(searchValue: string)
  {
  }

}
