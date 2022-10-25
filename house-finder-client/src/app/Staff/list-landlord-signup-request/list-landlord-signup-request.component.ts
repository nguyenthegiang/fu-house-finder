import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-list-landlord-signup-request',
  templateUrl: './list-landlord-signup-request.component.html',
  styleUrls: ['./list-landlord-signup-request.component.scss']
})
export class ListLandlordSignupRequestComponent implements OnInit {

  //{Search} input value
  searchLandlordName: any;

  constructor()
  { }

  ngOnInit(): void
  {
  }

  searchLandlordByName()
  {}
}
