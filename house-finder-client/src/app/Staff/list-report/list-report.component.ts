import { ValueConverter } from '@angular/compiler/src/render3/view/template';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
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
  //List all reported reportedHouses
  reportedHouses: ReportHouse[] = [];
  //List reports of selected house
  reportsOfSelectedHouse: StaffReport[] = [];
  //List all reports
  reports: StaffReport[] = [];
  //Landlord of selected house
  landlordOfSelectedHouse: User | undefined;

  //Filter report
  selectedFromDate: string | undefined;
  selectedToDate: string | undefined;
  selectedOrderBy: string | undefined;
  searchName: string | undefined;

  //Filter house
  selectedOrderByHouse: string = "desc";  //Default: Báo cáo nhiều nhất
  selectedActiveStatus: string | undefined;
  searchHouse: string | undefined;

  //(Paging) for Reports
  totalReport = 0; //items count
  reportPageSize = 10; //number of items per page
  reportPageNumber = 1; //starts at page 1
  reportPageCount = 0; //number of pages
  reportPageList: number[] = []; //array to loop with *ngFor in HTML Template

  //(Paging) for Reported reportedHouses
  totalReportedHouse = 0; //number of items
  housePageSize = 1000; //number of items per page
  housePageNumber = 1;
  housePageCount = 0; // number of pages
  housePageList: number[] = [];

  selectedReportStudentName: string | undefined;
  selectedReportHouse: string | undefined;
  selectedReportDate: Date | undefined;
  selectedReportSolvedDate: Date | undefined;
  selectedReportSolvedPerson: string | undefined;
  selectedReportContent: string | undefined;

  selectedReport: StaffReport | undefined;
  selectedStatusIdToUpdate: number | undefined;
  selectedStatusId: number | undefined;

  @ViewChild('searchReportContent') searchReportContent: any;
  @ViewChild('searchHouseName') searchHouseName: any;


  constructor(
    private reportService: ReportService,
    private houseService: HouseService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.selectedOrderBy = 'desc';
    this.filterReport(true);

    this.filterReportedHouse(false);
    //Call API: get all reports of this house
    this.houseService.countTotalReportedHouse().subscribe((data) => {
      this.totalReportedHouse = data;
      console.log(data);

      //(Paging) Count total number of pages
      this.housePageCount = Math.ceil(
        this.totalReportedHouse / this.housePageSize
      );
      this.housePageList = Array.from(
        { length: this.housePageCount },
        (_, i) => i + 1
      );
    });
  }

  searchReportedHouse(searchValue: string) {
    this.searchHouse = searchValue;
  }

  //Show modal
  changeSelectedHouse(houseId: number) {
    //Find the house which id == houseId
    var selectedHouse = this.reportedHouses.find((house) => house.houseId == houseId);
    if (selectedHouse?.listReports != undefined) {
      this.reportsOfSelectedHouse = selectedHouse.listReports;
      this.landlordOfSelectedHouse = selectedHouse.landlord;
    }
  }


  //Filter reports
  onFromDateSelected(selectedDate: string) {
    this.selectedFromDate = selectedDate;
    this.filterReport(true);
  }

  onToDateSelected(selectedDate: string) {
    this.selectedToDate = selectedDate;
    this.filterReport(true);
  }

  onOrderBySelected(selectedOrderBy: string) {
    this.selectedOrderBy = selectedOrderBy;
    this.filterReport(true);
  }

  //Filter reported houses
  onHouseOrderBySelected(selectedOrderBy: string) {
    this.selectedOrderByHouse = selectedOrderBy;
    this.filterReportedHouse(true);
  }

  onActiveStatusSelected(selectedStatus: string) {
    if (selectedStatus == "-1") {
      this.selectedActiveStatus = undefined;
    } else {
      this.selectedActiveStatus = selectedStatus;
    }
    this.filterReportedHouse(true);
  }

  //Search House by Name (contains)
  searchReportByContent(searchReportContent: string) {
    //not perform search for empty input
    if (!searchReportContent.trim()) {
      return;
    }
    // Call API (filter by name contains)
    this.searchName = searchReportContent;
    this.filterReport(true);
    console.log(this.searchName)
  }

  //[Paging] User click on a Page number -> Go to that page
  goToPage(pageNumber: number) {
    // Call API: go to Page Number
    this.reportPageNumber = pageNumber;
    this.filterReport(false);
  }

  goToHousePage(pageNumber: number) {
    // Call API: go to Page Number
    this.housePageNumber = pageNumber;
    this.filterReportedHouse(false);
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

    //Get data
    this.reportService
      .filterReport(this.reportPageSize, this.reportPageNumber, this.selectedFromDate, this.selectedToDate, this.selectedOrderBy, this.searchName)
      .subscribe((data) => {
        this.reports = data;
        this.scrollToTop();
      });

    //For Paging: count data
    this.reportService
      .filterReport(1000, 1, this.selectedFromDate, this.selectedToDate, this.selectedOrderBy, this.searchName)
      .subscribe((data) => {
        this.totalReport = data.length;

        // (Paging) Calculate number of pages
        this.reportPageCount = Math.ceil(this.totalReport / this.reportPageSize); //divide & round up

        // (Paging) Render pageList based on pageCount
        this.reportPageList = Array.from(
          { length: this.reportPageCount },
          (_, i) => i + 1
        );
        //pageList is now an array like {1, 2, 3, ..., n | n = pageCount}

      });
  }

  filterReportedHouse(resetPaging: boolean) {
    //if user filter -> reset Paging (back to page 1)
    if (resetPaging) {
      this.housePageNumber = 1;
    }

    this.houseService
      .filterReportedHouse(this.housePageSize, this.housePageNumber, this.selectedOrderByHouse, this.selectedActiveStatus)
      .subscribe((data) => {
        this.reportedHouses = data;
        this.scrollToTop();
      });
  }

  handleClear() {
    this.searchReportContent.nativeElement.value = ' ';
    this.searchName = undefined;
    this.filterReport(true);
  }
  handleClearHouse() {
    this.searchHouseName.nativeElement.value = ' ';
    this.searchHouse = undefined;
    this.filterReportedHouse(true);
  }

  onSelectReportStatus(selectedStatusId: string) {
    this.selectedStatusIdToUpdate = Number(selectedStatusId);
  }

  changeSelectedReport(reportId: number) {
    //Find the order which id == orderId
    var selectedReport = this.reports.find((report) => report.reportId == reportId);
    this.selectedReport = this.reports.find((report) => report.reportId == reportId);
    if (selectedReport != undefined) {
      this.selectedReportStudentName = selectedReport.student.displayName;
      this.selectedReportHouse = selectedReport.house.houseName;
      this.selectedReportContent = selectedReport.reportContent;
      if (selectedReport.solvedByNavigation != undefined) {
        this.selectedReportSolvedPerson = selectedReport.solvedByNavigation.displayName;
      }
      if (selectedReport.solvedByNavigation == undefined) {
        this.selectedReportSolvedPerson = "";
      }
      this.selectedReportDate = selectedReport.reportedDate;
      this.selectedReportSolvedDate = selectedReport.solvedDate;
      this.selectedStatusId = selectedReport.status.statusId;
      console.log(selectedReport.reportContent);
    }
  }

  updateReportStatus() {

  }
}
