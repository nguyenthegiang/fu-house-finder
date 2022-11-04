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
  countAvailableHouses = 0; //items count
  pageSize = 9;             //number of items per page
  pageNumber = 1;           //starts at page 1
  pageCount = 0;            //number of pages
  pageList: number[] = [];  //array to loop with *ngFor in HTML Template

  constructor(private orderService: OrderService,) {

  }

  ngOnInit(): void {
    //Call API: get all orders
    this.orderService.getAllOrders().subscribe(data => {
      this.orders = data;
    });
  }

}
