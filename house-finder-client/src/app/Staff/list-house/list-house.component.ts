import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { House } from 'src/app/models/house';
import { HouseService } from 'src/app/services/house.service';
import { LandlordInformationService } from 'src/app/services/landlord-information.service';

@Component({
  selector: 'app-list-house',
  templateUrl: './list-house.component.html',
  styleUrls: ['./list-house.component.scss']
})
export class ListHouseComponent implements OnInit {
  //List of all houses
  houses: House[] = [];

  //{Search} input value
  houseCount: number = 0;
  roomCount: number = 0;
  roomAvailableCount: number = 0;
  searchValue: string | undefined;

  constructor(private houseService: HouseService,
    private router: Router)
    { }

  ngOnInit(): void {
    //Get List of all Houses
    this.houseService.getAvailableHouses().subscribe(data => {
      this.houses = data;
    });
  }

  viewHouse(id: number)
  {
    console.log(id);
    this.router.navigate(['/Staff/staff-house-detail/' + id]);
  }

  search(searchValue: string)
  {}

  changeBreadcrumbStatus()
  {
    localStorage.setItem('breadcrumb','false');
  }
}
