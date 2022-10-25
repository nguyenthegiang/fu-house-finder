import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-list-landlord-request',
  templateUrl: './list-landlord-request.component.html',
  styleUrls: ['./list-landlord-request.component.scss']
})

export class ListLandlordRequestComponent implements OnInit
{
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
