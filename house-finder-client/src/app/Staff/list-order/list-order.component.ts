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

  constructor(private orderService: OrderService,) {

  }

  ngOnInit(): void {
    //Call API: get all orders
    this.orderService.getAllOrders().subscribe(data => {
      this.orders = data;
    });
  }

}
