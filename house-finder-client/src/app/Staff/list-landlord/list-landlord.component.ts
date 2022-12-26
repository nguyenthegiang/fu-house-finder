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
export class ListLandlordComponent implements OnInit {
  landlordId: string = '';
  houseCount: number = 0;
  roomCount: number = 0;
  roomAvailableCount: number = 0;

  //(Paging) for Landlords
  totalLandlord = 0; //items count
  landlordPageSize = 10; //number of items per page
  landlordPageNumber = 1; //starts at page 1
  landlordPageCount = 0; //number of pages
  landlordPageList: number[] = []; //array to loop with *ngFor in HTML Template

  //{Search} input value
  searchName: string | undefined;

  //List of landlords
  landlords: User[] | undefined;

  //Selected user status
  selectedStatusId: number | undefined;

  @ViewChild('searchValue') searchValue: any;


  constructor(
    private userService: UserService,
    private lanlord_informationService: LandlordInformationService,
    private router: Router
  ) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Staff
     */
    var userRole = localStorage.getItem("role");
    if (userRole == null || userRole!.indexOf('Department') < 0) {
      window.location.href = '/home';
    }

    this.filterLandlord(true);

    this.userService.getLandlords().subscribe(data => {
      this.landlords = data;
    });

    this.lanlord_informationService.getLandLordInfomation(this.landlordId).subscribe(data => {
      this.houseCount = data.houseCount;
      this.roomCount = data.roomCount;
      this.roomAvailableCount = data.roomAvailableCount;
    });
    console.log(this.searchName);
    this.reloadListLandlord();
  }

  viewHouse(id: string) {
    this.landlordId = id;
    console.log(id);
    this.router.navigate(['/Staff/staff-landlord-detail/' + id]);
  }

  search(searchValue: string) {
    this.searchName = searchValue;
  }

  handleClear() {
    this.searchValue.nativeElement.value = ' ';
    this.searchName = undefined;
    this.userService.getLandlords().subscribe(data => {
      this.landlords = data;
    });
  }

  reloadListLandlord() {
    this.userService.getLandlords().subscribe(data => {
      this.landlords = data;
    });

    this.lanlord_informationService.getLandLordInfomation(this.landlordId).subscribe(data => {
      this.houseCount = data.houseCount;
      this.roomCount = data.roomCount;
      this.roomAvailableCount = data.roomAvailableCount;
    });
  }

  updateUserStatus(event: any, userId: string) {
    //check if staff just checked or unchecked the checkbox
    const isChecked = (<HTMLInputElement>event.target).checked;

    if (isChecked) {
      this.selectedStatusId = 1;
    }
    else {
      this.selectedStatusId = 0;
    }
    this.userService.updateUserStatus(userId, this.selectedStatusId).subscribe((data) => {
      this.reloadListLandlord();
    });
  }

  // Go to top of Page: used whenever user filter/paging data -> refresh list data
  scrollToTop() {
    window.scroll({
      top: 0,
      left: 0,
      behavior: 'smooth',
    });
  }

  // Call API to update list landlord with selected Filter value & Paging
  filterLandlord(resetPaging: boolean) {
    //if user filter -> reset Paging (back to page 1)
    if (resetPaging) {
      this.landlordPageNumber = 1;
    }

    this.userService
      .filterUser(this.landlordPageSize, this.landlordPageNumber, this.searchName)
      .subscribe((data) => {
        this.landlords = data;
        this.scrollToTop();
      });
  }

  //[Paging] User click on a Page number -> Go to that page
  goToPage(pageNumber: number) {
    // Call API: go to Page Number
    this.landlordPageNumber = pageNumber;
    this.filterLandlord(false);
  }
}
