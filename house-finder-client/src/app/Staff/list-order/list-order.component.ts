import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/models/order';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-list-order',
  templateUrl: './list-order.component.html',
  styleUrls: ['./list-order.component.scss']
})
export class ListOrderComponent implements OnInit {
  //List of orders
  orders: Order[] = [];

  //(Paging)
  totalOrder = 0; //items count
  pageSize = 10;             //number of items per page
  pageNumber = 1;           //starts at page 1
  pageCount = 0;            //number of pages
  pageList: number[] = [];  //array to loop with *ngFor in HTML Template

  constructor(private orderService: OrderService,) {

  }

  ngOnInit(): void {
    this.filterHouse(false);

    // (Paging) Count available Houses for total number of pages
    this.orderService.countTotalOrder().subscribe(data => {
      this.totalOrder = data;
      console.log(data);

      // (Paging) Calculate number of pages
      this.pageCount = Math.ceil(this.totalOrder / this.pageSize);  //divide & round up

      // (Paging) Render pageList based on pageCount
      this.pageList = Array.from({ length: this.pageCount }, (_, i) => i + 1);
      //pageList is now an array like {1, 2, 3, ..., n | n = pageCount}
    });
  }

  //[Paging] User click on a Page number -> Go to that page
  goToPage(pageNumber: number) {
    // Call API: go to Page Number
    this.pageNumber = pageNumber;
    this.scrollToTop();
  }

  // Go to top of Page: used whenever user filter/paging data -> refresh list data
  scrollToTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth'
    });
  }

  // Call API to update list house with selected Filter value & Paging
  filterHouse(resetPaging: boolean) {
    //if user filter -> reset Paging (back to page 1)
    if (resetPaging) {
      this.pageNumber = 1;
    }
    this.orderService.getListOrderForPaging(
      this.pageSize,
      this.pageNumber,
    ).subscribe(data => {
      this.orders = data;
      this.scrollToTop();
    });
  }

}
