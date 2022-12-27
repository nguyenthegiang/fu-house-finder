import { Component, OnInit, ViewChild, TemplateRef, ElementRef, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-confirm-order',
  templateUrl: './confirm-order.component.html',
  styleUrls: ['./confirm-order.component.scss']
})
export class ConfirmOrderComponent implements OnInit, AfterViewInit {
  @ViewChild('modalButton') modalButton: ElementRef<HTMLElement> | undefined;

  constructor() { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    let el: HTMLElement = this.modalButton!.nativeElement;
    el.click();
  }

}
