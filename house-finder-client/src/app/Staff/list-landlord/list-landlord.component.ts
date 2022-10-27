import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-list-landlord',
  templateUrl: './list-landlord.component.html',
  styleUrls: ['./list-landlord.component.scss']
})
export class ListLandlordComponent implements OnInit
{
  //{Search} input value
  searchLandlordName: any;
  //List of landlords
  landlords: User[] = [];

  constructor(
    private userService: UserService,
  )
  { }

  ngOnInit(): void
  {
    //Call API: get available rooms of this house
    this.userService.getLandlords().subscribe(data => {
      this.landlords = data;
    });
  }

  searchLandlordByName()
  {}
}
