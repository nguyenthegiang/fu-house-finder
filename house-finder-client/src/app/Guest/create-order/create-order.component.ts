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
    studentId: 'HE153046',
    studentName: 'HE153046',
    phoneNumber: '0987206969',
    email: 'thongdz@gmail.com',
    orderContent: 'em uoc mo duoc lam sieu nhan',
    status: {
      statusId: 1,
      statusName: 'Unsolved'
    },
    orderedDate: new Date(),
    solvedDate: new Date()
  }
  constructor(
    private orderService: OrderService
  ) {

  }

  ngOnInit(): void {

  }


}
