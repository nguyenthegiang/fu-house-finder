import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Report } from 'src/app/models/report';
import { ReportHouse } from 'src/app/models/reportHouse';
import { StaffReport } from 'src/app/models/staffReport';
import { User } from 'src/app/models/user';
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
  //Landlord of selected house
  landlordOfSelectedHouse: User | undefined;

  //Filter
  selectedFromDate: string | undefined;
  selectedToDate: string | undefined;

  //(Paging) for Reports
  totalReport = 0; //items count
  reportPageSize = 10; //number of items per page
  reportPageNumber = 1; //starts at page 1
  reportPageCount = 0; //number of pages
  reportPageList: number[] = []; //array to loop with *ngFor in HTML Template

  //(Paging) for Reported Houses
  totalReportedHouse = 0; //number of items
  housePageSize = 10; //number of items per page
  housePageNumber = 1;
  housePageCount = 0; // number of pages
  housePageList: number[] = [];

  constructor(
    private reportService: ReportService,
    private houseService: HouseService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.filterReport(false);

    // (Paging for Reports) Count available Houses for total number of pages
    this.reportService.countTotalReport().subscribe((data) => {
      this.totalReport = data;
      console.log(data);

      // (Paging) Calculate number of pages
      this.reportPageCount = Math.ceil(this.totalReport / this.reportPageSize); //divide & round up

      // (Paging) Render pageList based on pageCount
      this.reportPageList = Array.from({ length: this.reportPageCount }, (_, i) => i + 1);
      //pageList is now an array like {1, 2, 3, ..., n | n = pageCount}
    });

    this.filterReportedHouse(false);
    //Call API: get all reports of this house
    this.houseService.countTotalReportedHouse().subscribe((data) => {
      this.totalReportedHouse = data;

      //(Paging) Count total number of pages
      this.housePageCount = Math.ceil(this.totalReportedHouse / this.housePageSize);
      this.housePageList = Array.from({ length: this.housePageCount }, (_, i) => i + 1);
    });
  }

  search(searchValue: string) {}

  //
  changeSelectedHouse(houseId: number) {
    //Find the house which id == houseId
    var selectedHouse = this.houses.find((house) => house.houseId == houseId);
    if (selectedHouse?.listReports != undefined) {
      this.reportsOfSelectedHouse = selectedHouse.listReports;
      this.landlordOfSelectedHouse = selectedHouse.landlord;
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

  //[Paging] User click on a Page number -> Go to that page
  goToPage(pageNumber: number) {
    // Call API: go to Page Number
    this.reportPageNumber = pageNumber;
    this.filterReport(false);
    this.scrollToTop();
  }

  goToHousePage(pageNumber: number) {
    // Call API: go to Page Number
    this.housePageNumber = pageNumber;
    this.filterReportedHouse(false);
    this.scrollToTop();
  }

  // Go to top of Page: used whenever user filter/paging data -> refresh list data
  scrollToTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
  }

    // Call API to update list house with selected Filter value & Paging
    filterReport(resetPaging: boolean) {
      //if user filter -> reset Paging (back to page 1)
      if (resetPaging) {
        this.reportPageNumber = 1;
      }

      this.reportService
        .filterReport(
          this.reportPageSize,
          this.reportPageNumber,
        )
        .subscribe((data) => {
          this.reports = data;
          this.scrollToTop();
        });
    }

    filterReportedHouse(resetPaging: boolean) {
      //if user filter -> reset Paging (back to page 1)
      if (resetPaging) {
        this.housePageNumber = 1;
      }

      this.houseService.filterReportedHouse(
          this.housePageSize,
          this.housePageNumber,
        )
        .subscribe((data) => {
          this.houses = data;
          this.scrollToTop();
        });
    }
}
