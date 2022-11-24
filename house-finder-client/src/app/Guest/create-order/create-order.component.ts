import { Component, OnInit } from '@angular/core';
import { CreateOrder } from 'src/app/models/createOrder';
import { OrderService } from 'src/app/services/order.service';
@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss']
})
export class CreateOrderComponent implements OnInit {
  orderDetail: CreateOrder = {
    orderId: 0,
    studentName: '',
    phoneNumber: '',
    email: '',
    orderContent: '',
    statusId: 1
  }
  constructor(
    private orderService: OrderService
  ) {

  }

  ngOnInit(): void {

  }
  addOrder() {
    //Check if user has logged in
    var user = null;
    user = localStorage.getItem("user");
    if (user === null) {
      alert("Đăng nhập để xử dụng tính năng này!")
    } else {
    this.orderService.addOrder(this.orderDetail).subscribe(() => this.goBack());
    }
  }
  goBack(): void {
    window.location.reload();
  }

}
