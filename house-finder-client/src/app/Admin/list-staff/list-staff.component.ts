import { UserService } from './../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-list-staff',
  templateUrl: './list-staff.component.html',
  styleUrls: ['./list-staff.component.scss']
})

export class ListStaffComponent implements OnInit
{
  staffs: User[] = []

  constructor(private userService: UserService,
    private router: Router) { }

  ngOnInit(): void
  {
    //Get List of Staffs
    this.userService.getStaffs().subscribe(data => {
      this.staffs = data;
    });
  }

  addHouse()
  {
    this.router.navigate(['/Admin/create-account']);
  }

  updateStaff(id: string)
  {
    console.log(id);
    this.router.navigate(['/Admin/update-account/' + id]);
  }

  deleteStaff(id: string)
  {
    console.log(id);
    this.router.navigate(['/Admin/landlord-house-detail/' + id]);
  }
}
