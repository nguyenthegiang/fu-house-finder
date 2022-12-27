import { Order } from './../../models/order';
import { OrderService } from 'src/app/services/order.service';
import { Component, OnInit, ViewChild, TemplateRef, ElementRef, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-confirm-order',
  templateUrl: './confirm-order.component.html',
  styleUrls: ['./confirm-order.component.scss']
})
export class ConfirmOrderComponent implements OnInit, AfterViewInit {
  // button trigger modal
  @ViewChild('modalButton') modalButton: ElementRef<HTMLElement> | undefined;

  //List of Order that is not confirmed of this student
  unconfirmedOrders: Order[] | undefined;

  constructor(private orderService: OrderService) { }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    //Get list of un-confirmed orders of this student
    this.orderService.getListOrderNotConfirm().subscribe(
      data => {
        if (data.status == 403) {
          //User not logged in: do nothing
          return;
        } else {
          //assign list of unconfirmed orders
          this.unconfirmedOrders = data;

          if (this.unconfirmedOrders && this.unconfirmedOrders.length > 0) {
            //Open modal for Confirm Orders
            this.openModal();
          } else {
            //User has no order: do nothing
            return;
          }
        }
      },
    );
  }

  openModal() {
    let el: HTMLElement = this.modalButton!.nativeElement;
    el.click();
  }

  // user click "Đã giải quyết"
  confirmSolvedOrder(orderId: number) {
    this.orderService.updateOrderStatus(orderId, 3).subscribe(data => {
      //Get list of un-confirmed orders of this student, to check if it still has orders
      this.orderService.getListOrderNotConfirm().subscribe(
        data => {
          if (data.status == 403) {
            //User not logged in: do nothing
            return;
          } else {
            //assign list of unconfirmed orders
            this.unconfirmedOrders = data;

            if (this.unconfirmedOrders && this.unconfirmedOrders.length > 0) {
              //user still has orders left
            } else {
              //User has no order: close modal
              this.openModal();
              return;
            }
          }
        },
      );
    });
  }
}
