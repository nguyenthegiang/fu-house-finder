import { Component, OnInit, ViewChild } from '@angular/core';
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
  houses: House[] | undefined;

  //{Search} input value
  houseCount: number = 0;
  roomCount: number = 0;
  roomAvailableCount: number = 0;
  searchName: string | undefined;

  @ViewChild('searchValue') searchValue: any;

  constructor(private houseService: HouseService,
    private router: Router) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Staff
     */
    var userRole = localStorage.getItem("role");
    if (userRole == null || userRole!.indexOf('Department') < 0) {
      window.location.href = '/home';
    }

    //Get List of all Houses
    this.houseService.getAllHouses().subscribe(data => {
      this.houses = data;
    });
  }

  viewHouse(id: number) {
    console.log(id);
    this.router.navigate(['/Staff/staff-house-detail/' + id]);
  }

  search(searchValue: string) {
    this.searchName = searchValue;
  }

  changeBreadcrumbStatus() {
    localStorage.setItem('breadcrumb', 'false');
  }

  handleClear() {
    this.searchValue.nativeElement.value = ' ';
    this.searchName = undefined;
    this.houseService.getAvailableHouses().subscribe(data => {
      this.houses = data;
    });
  }
}
