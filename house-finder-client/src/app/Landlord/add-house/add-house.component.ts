import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-house',
  templateUrl: './add-house.component.html',
  styleUrls: ['./add-house.component.scss']
})
export class AddHouseComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  logout()
  {
    window.location.href = "/login";
  }
}
