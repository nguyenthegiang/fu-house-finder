import { OrderService } from 'src/app/services/order.service';
import { Component, OnInit, ViewChild, TemplateRef, ElementRef, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-confirm-order',
  templateUrl: './confirm-order.component.html',
  styleUrls: ['./confirm-order.component.scss']
})
export class ConfirmOrderComponent implements OnInit, AfterViewInit {
  @ViewChild('modalButton') modalButton: ElementRef<HTMLElement> | undefined;

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
          //Open modal for Confirm Orders
          this.openModal();
        }
      },
    );
  }

  openModal() {
    let el: HTMLElement = this.modalButton!.nativeElement;
    el.click();
  }
}
