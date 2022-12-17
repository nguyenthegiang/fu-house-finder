import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Chart } from 'chart.js';
import { Order } from 'src/app/models/order';
import { OrderStatus } from 'src/app/models/orderStatus';
import { OrderService } from 'src/app/services/order.service';
import { OrderStatusService } from 'src/app/services/orderStatus.service';

@Component({
  selector: 'app-list-order',
  templateUrl: './list-order.component.html',
  styleUrls: ['./list-order.component.scss'],
  providers: [DatePipe],
})
export class ListOrderComponent implements OnInit {
  //List of orders
  orders: Order[] = [];

  //List of orders' statuses
  statuses: OrderStatus[] = [];

  //(Paging)
  pageSize = 10; //number of items per page
  pageNumber = 1; //starts at page 1
  pageCount = 0; //number of pages
  pageList: number[] = []; //array to loop with *ngFor in HTML Template

  //Filter
  selectedStatusId: number | undefined;
  selectedFromDate: string | undefined;
  selectedToDate: string | undefined;
  selectedOrderBy: string | undefined;
  searchValue: string | undefined;

  //Modal
  selectedOrderName: string | undefined;
  selectedOrderPhone: string | undefined;
  selectedOrderEmail: string | undefined;
  selectedOrderCreatedDate: Date | undefined;
  selectedOrderContent: string | undefined;
  selectedOrderStatus: number | undefined;

  selectedOrder: Order | undefined;
  selectedStatusIdToUpdate: number | undefined;

  totalOfSolvedOrder: number | undefined;

  //Bar chart
  totalofSolvedOrderInYear: number[] | undefined;
  totalofSolvedOrderInDay: number = 0;
  totalofSolvedOrderInMonth: number = 0;
  totalofSolvedOrderInCurrentYear: number = 0;

  //Statistic
  //Get current year
  currentYear: number = new Date().getFullYear();
  //Get current month
  currentMonth: number = new Date().getMonth();

  constructor(
    private orderService: OrderService,
    private orderStatusService: OrderStatusService,
    private router: Router,
    private datePipe: DatePipe,
  ) {

  }

  ngOnInit(): void {
    this.selectedOrderBy = 'desc';
    this.filterOrder(true);

    let currentDate = this.datePipe.transform((new Date), 'yyyy-MM-dd') + "";

    //let yesterday = new Date()
    // yesterday.setDate(yesterday.getDate() - 1)
    // let previousDate = yesterday.getFullYear() + "-" + (yesterday.getMonth() + 1) + "-" + yesterday.getDate();

    this.orderService.countOrderSolvedByStaffInADay(currentDate).subscribe((data) => {
      this.totalofSolvedOrderInDay = data;
    })

    //Call API: get list of orders' statuses
    this.orderStatusService.getAllStatus().subscribe((data) => {
      this.statuses = data;
    });

    this.orderService.countTotalOrderSolvedByAccount().subscribe((data) => {
      this.totalOfSolvedOrder = data;
    });

    //Call API: create a bar chart show number of solved orders by this staff in a year
    this.orderService.countSolvedOrderByStaffInAYear().subscribe((data) => {
      this.totalofSolvedOrderInYear = data;

      this.totalofSolvedOrderInMonth = this.totalofSolvedOrderInYear[this.currentMonth];

      var num = 0;
      this.totalofSolvedOrderInYear.forEach(function (value) {
        num += value;
      })
      this.totalofSolvedOrderInCurrentYear = num;

      //Create order chart
      var solvedOrderChart = new Chart('solvedOrderChart', {
        type: 'bar',
        data: {
          labels: [
            'Tháng 1',
            'Tháng 2',
            'Tháng 3',
            'Tháng 4',
            'Tháng 5',
            'Tháng 6',
            'Tháng 7',
            'Tháng 8',
            'Tháng 9',
            'Tháng 10',
            'Tháng 11',
            'Tháng 12',
          ],
          datasets: [
            {
              label: 'Số đơn đã được giải quyết',
              data: this.totalofSolvedOrderInYear,
              backgroundColor: ['#069bff'],
              borderColor: ['#069bff'],
              borderWidth: 1,
            },
          ],
        },
        options: {
          scales: {
            y: {
              beginAtZero: true,
            },
          },
          plugins: {
            title: {
              display: true,
              text:
                'Thống kê số lượng nguyện vọng bạn đã giải quyết trong năm ' + this.currentYear,
            },
          },
        },
      });
    }
    )

  }

  //[Paging] User click on a Page number -> Go to that page
  goToPage(pageNumber: number) {
    // Call API: go to Page Number
    this.pageNumber = pageNumber;
    this.filterOrder(false);
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
  filterOrder(resetPaging: boolean) {
    //if user filter -> reset Paging (back to page 1)
    if (resetPaging) {
      this.pageNumber = 1;
    }

    //Get data
    this.orderService
      .filterOrder(
        this.pageSize,
        this.pageNumber,
        this.selectedStatusId,
        this.selectedFromDate,
        this.selectedToDate,
        this.selectedOrderBy
      )
      .subscribe((data) => {
        this.orders = data;
        this.scrollToTop();
      });

    //For Paging: Count Order
    this.orderService
      .filterOrder(
        1000,
        1,
        this.selectedStatusId,
        this.selectedFromDate,
        this.selectedToDate,
        this.selectedOrderBy
      )
      .subscribe((data) => {
        //(Paging) calculate number of pages
        this.pageCount = Math.ceil(data.length / this.pageSize);  //divide & round up

        // (Paging) Render pageList based on pageCount
        this.pageList = Array.from({ length: this.pageCount }, (_, i) => i + 1);
        //pageList is now an array like {1, 2, 3, ..., n | n = pageCount}
      });
  }

  //[Filter] Filter by Campus
  onStatusSelected(selectedStatusId: string) {
    // convert string to number
    var numberCampusId: number = +selectedStatusId;

    // Call API: update list houses with the campus user chose
    this.selectedStatusId = numberCampusId;
    this.filterOrder(true);
  }

  onFromDateSelected(selectedDate: string) {
    this.selectedFromDate = selectedDate;
    this.filterOrder(true);
  }

  onToDateSelected(selectedDate: string) {
    this.selectedToDate = selectedDate;
    this.filterOrder(true);
  }

  onOrderBySelected(selectedOrderBy: string) {
    this.selectedOrderBy = selectedOrderBy;
    this.filterOrder(true);
  }

  viewOrder(id: number) {
    this.router.navigate(['/Staff/staff-landlord-detail/' + id]);
  }

  search(searchValue: string) { }

  //To show detail in modal
  changeSelectedOrder(orderId: number) {
    //Find the order which id == orderId
    var selectedOrder = this.orders.find((order) => order.orderId == orderId);
    this.selectedOrder = this.orders.find((order) => order.orderId == orderId);
    if (selectedOrder != undefined) {
      this.selectedOrderName = selectedOrder.studentName;
      this.selectedOrderPhone = selectedOrder.phoneNumber;
      this.selectedOrderEmail = selectedOrder.email;
      this.selectedOrderCreatedDate = selectedOrder.orderedDate;
      this.selectedOrderContent = selectedOrder.orderContent;
      this.selectedStatusId = selectedOrder.status.statusId;
    }
  }

  onSelectOrderStatus(selectedStatusId: string) {
    this.selectedStatusIdToUpdate = Number(selectedStatusId);
  }

  updateOrderStatus() {
    var orderId = this.selectedOrder?.orderId;
    if (orderId != undefined && this.selectedStatusIdToUpdate != undefined) {
      //Call API:
      this.orderService.updateOrderStatus(orderId, this.selectedStatusIdToUpdate).subscribe();
      window.location.reload();
    }
  }
}
