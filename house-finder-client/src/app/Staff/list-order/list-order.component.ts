import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Order } from 'src/app/models/order';
import { OrderStatus } from 'src/app/models/orderStatus';
import { OrderService } from 'src/app/services/order.service';
import { OrderStatusService } from 'src/app/services/orderStatus.service';

@Component({
  selector: 'app-list-order',
  templateUrl: './list-order.component.html',
  styleUrls: ['./list-order.component.scss'],
})
export class ListOrderComponent implements OnInit {
  //List of orders
  orders: Order[] = [];

  //List of orders' statuses
  statuses: OrderStatus[] = [];

  //(Paging)
  totalOrder = 0; //items count
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

  constructor(
    private orderService: OrderService,
    private orderStatusService: OrderStatusService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.selectedOrderBy = 'desc';
    this.filterOrder(true);
    //this.filterOrder(false);

    // (Paging) Count available Houses for total number of pages
    this.orderService.countTotalOrder().subscribe((data) => {
      this.totalOrder = data;
      console.log(data);

      // (Paging) Calculate number of pages
      this.pageCount = Math.ceil(this.totalOrder / this.pageSize); //divide & round up

      // (Paging) Render pageList based on pageCount
      this.pageList = Array.from({ length: this.pageCount }, (_, i) => i + 1);
      //pageList is now an array like {1, 2, 3, ..., n | n = pageCount}
    });

    //Call API: get list of orders' statuses
    this.orderStatusService.getAllStatus().subscribe((data) => {
      this.statuses = data;
    });
  }

  //[Paging] User click on a Page number -> Go to that page
  goToPage(pageNumber: number) {
    // Call API: go to Page Number
    this.pageNumber = pageNumber;
    this.filterOrder(false);
    //this.scrollToTop();
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
  }

  //[Filter] Filter by Campus
  onStatusSelected(selectedStatusId: string) {
    // convert string to number
    var numberCampusId: number = +selectedStatusId;
    console.log(numberCampusId);

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

  onOrderBySelected(selectedOrderBy: string){
    this.selectedOrderBy = selectedOrderBy;
    this.filterOrder(true);
  }

  viewOrder(id: number) {
    this.router.navigate(['/Staff/staff-landlord-detail/' + id]);
  }

  search(searchValue: string) {}

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
      console.log('Status id: ' + this.selectedStatusId);
    }
  }

  onSelectOrderStatus(selectedStatusId: string){
    this.selectedStatusIdToUpdate = Number(selectedStatusId);
    this.filterOrder(true);
  }

  updateOrderStatus() {
    var orderId = this.selectedOrder?.orderId;
    console.log("OrderId: " + this.selectedOrder?.orderId + "; statusId: " + this.selectedStatusIdToUpdate);
    if (orderId != undefined && this.selectedStatusIdToUpdate != undefined) {
      //Call API:
      this.orderService.updateOrderStatus(orderId, this.selectedStatusIdToUpdate).subscribe();
      window.location.reload();
    }
  }
}
