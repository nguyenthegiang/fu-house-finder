import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user';
import { LandlordInformationService } from 'src/app/services/landlord-information.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-list-landlord',
  templateUrl: './list-landlord.component.html',
  styleUrls: ['./list-landlord.component.scss']
})
export class ListLandlordComponent implements OnInit
{
  landlordId: string = '';
  houseCount: number = 0;
  roomCount: number = 0;
  roomAvailableCount: number = 0;

  //{Search} input value
  searchName: string | undefined;

  //List of landlords
  landlords: User[] = [];

  @ViewChild('searchValue') searchValue: any;


  constructor(
    private userService: UserService,
    private lanlord_informationService: LandlordInformationService,
    private router: Router
  )
  { }

  ngOnInit(): void
  {
    this.userService.getLandlords().subscribe(data => {
      this.landlords = data;
    });

    this.lanlord_informationService.getLandLordInfomation(this.landlordId).subscribe(data => {
      this.houseCount = data.houseCount;
      this.roomCount = data.roomCount;
      this.roomAvailableCount = data.roomAvailableCount;
    });
    console.log(this.searchName);
  }


  viewHouse(id: string)
  {
    this.landlordId = id;
    console.log(id);
    this.router.navigate(['/Staff/staff-landlord-detail/' + id]);
  }

  search(searchValue: string)
  {
    this.searchName = searchValue;
  }

  handleClear(){
    this.searchValue.nativeElement.value = ' ';
    this.searchName = undefined;
    this.userService.getLandlords().subscribe(data => {
      this.landlords = data;
    });
  }
}
