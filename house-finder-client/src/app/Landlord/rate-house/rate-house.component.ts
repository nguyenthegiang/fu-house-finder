import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-rate-house',
  templateUrl: './rate-house.component.html',
  styleUrls: ['./rate-house.component.scss']
})
export class RateHouseComponent implements OnInit {
  replyOn = false;

  constructor() { }

  ngOnInit(): void {
  }
}
