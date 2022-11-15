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
  styleUrls: ['./list-report.component.scss'],
})
export class ListReportComponent implements OnInit {
  //{Search} input value
  searchValue: string | undefined;
  //List all reported houses
  houses: ReportHouse[] = [];
  //List reports of selected house
  reportsOfSelectedHouse: StaffReport[] = [];
  //List all reports
  reports: StaffReport[] = [];

  //Filter
  selectedFromDate: string | undefined;
  selectedToDate: string | undefined;

  constructor(
    private reportService: ReportService,
    private houseService: HouseService,
    private router: Router
  ) {}

  ngOnInit(): void {
    //Call API: get all reports of this house
    this.houseService.getReportedHouses().subscribe((data) => {
      this.houses = data;
    });

    this.reportService.getAllReport().subscribe((data) => {
      this.reports = data;
    })
  }

  search(searchValue: string) {}

  //
  changeSelectedHouse(houseId: number) {
    //Find the house which id == houseId
    var selectedHouse = this.houses.find((house) => house.houseId == houseId);
    if (selectedHouse?.listReports != undefined) {
      this.reportsOfSelectedHouse = selectedHouse.listReports;
    }
  }

  onFromDateSelected(selectedDate: string) {
    this.selectedFromDate = selectedDate;
    console.log(selectedDate);
  }

  onToDateSelected(selectedDate: string) {
    this.selectedToDate = selectedDate;
    console.log(selectedDate);
  }
}
