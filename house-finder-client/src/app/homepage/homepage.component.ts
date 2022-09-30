import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']
})
export class HomepageComponent implements OnInit {
  List: any = [1, 2, 3];

  constructor() { }

  ngOnInit(): void
  {
  }
}
