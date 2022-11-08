import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-list-report',
  templateUrl: './list-report.component.html',
  styleUrls: ['./list-report.component.scss']
})

export class ListReportComponent implements OnInit
{
  //{Search} input value
  searchLandlordName: any;

  constructor()
  { }

  ngOnInit(): void
  {
  }

  searchReport()
  {}
}
