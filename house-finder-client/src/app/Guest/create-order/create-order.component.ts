import { Component, OnInit, ViewChild } from '@angular/core';
import { CreateOrder } from 'src/app/models/createOrder';
import { OrderService } from 'src/app/services/order.service';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
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
  @ViewChild('orderSuccessAlert') private orderSuccessAlert: SwalComponent | undefined;
  @ViewChild('orderErrorAlert') private orderErrorAlert: SwalComponent | undefined;
  @ViewChild('orderRoleErrorAlert') private orderRoleErrorAlert: SwalComponent | undefined;
  constructor(
    private orderService: OrderService
  ) { }

  ngOnInit(): void { }

  addOrder() {
    //Check if user has logged in
    var user = null;
    var role = null;
    user = localStorage.getItem("user");
    role = localStorage.getItem("role");
    if (user === null) {
      //user not logged in => Alert
      this.orderErrorAlert?.fire();
      return;
    } else if (role === "Student") {
      //Check user logged in from Server => if not => alert
      this.orderService.addOrder(this.orderDetail).subscribe(
        data => {
          if (data.status == 403) {
            this.orderErrorAlert?.fire();
          } else if (data.status == 200) {
            this.orderSuccessAlert?.fire();
          }
        },
        error => { }
      );
    } else if (role !== "Student") {
      this.orderRoleErrorAlert?.fire();
    }
  }
  
  // goBack(): void {
  //   window.location.reload();
  // }
}
