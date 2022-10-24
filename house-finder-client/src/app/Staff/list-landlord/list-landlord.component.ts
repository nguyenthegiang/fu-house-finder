import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-list-landlord',
  templateUrl: './list-landlord.component.html',
  styleUrls: ['./list-landlord.component.scss']
})
export class ListLandlordComponent implements OnInit
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
