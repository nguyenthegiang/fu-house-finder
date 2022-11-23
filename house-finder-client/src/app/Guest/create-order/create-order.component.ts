import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/models/order';
import { OrderService } from 'src/app/services/order.service';
@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss']
})
export class CreateOrderComponent implements OnInit {
  orderDetail: Order = {
    orderId: 0,
    studentName: '',
    phoneNumber: '',
    email: '',
    orderContent: '',
    status: {
      statusId: 1,
      statusName: 'Unsolved'
    },
  }
  constructor(
    private orderService: OrderService
  ) {

  }

  ngOnInit(): void {

  }
  addOrder() {
    this.orderService.addOrder(this.orderDetail).subscribe(() => this.goBack());
  }
  goBack(): void {
    window.location.reload();
  }

}
