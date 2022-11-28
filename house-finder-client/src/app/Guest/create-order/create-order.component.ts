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
  ) { }

  ngOnInit(): void { }

  addOrder() {
    //Check if user has logged in
    var user = null;
    user = localStorage.getItem("user");
    if (user === null) {
      //user not logged in => Alert
      this.alertUserNotLoggedIn();
    } else {
      //Check user logged in from Server => if not => alert
      this.orderService.addOrder(this.orderDetail).subscribe(
        data => {
          if (data.status == 403) {
            this.alertUserNotLoggedIn();
          }
        },
        error => {}
      );
    }
  }

  //Alert when create order failed
  alertUserNotLoggedIn() {
    alert("Vui lòng đăng nhập để xử dụng tính năng này!");
  }

  goBack(): void {
    window.location.reload();
  }
}
