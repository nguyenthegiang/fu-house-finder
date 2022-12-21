import { UserService } from './../../services/user.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-list-staff',
  templateUrl: './list-staff.component.html',
  styleUrls: ['./list-staff.component.scss']
})

export class ListStaffComponent implements OnInit {
  @ViewChild('deleteStaffAlert') private deleteStaffAlert: SwalComponent | undefined;
  staffs: User[] = []

  selectedId: any;

  constructor(private userService: UserService,
    private router: Router) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Admin
     */
    var userRole = localStorage.getItem("role");
    if (userRole != 'Admin') {
      window.location.href = '/home';
    }

    //Get List of Staffs
    this.userService.getStaffs().subscribe(data => {
      this.staffs = data;
    });
  }

  addStaff() {
    this.router.navigate(['/Admin/create-account']);
  }

  updateStaff(id: string) {
    this.router.navigate(['/Admin/update-account/' + id]);
  }

  async confirmDelete(id: string) {
    this.selectedId = id;
    this.deleteStaffAlert?.fire();
  }

  deleteStaff() {
    this.userService.deleteStaff(this.selectedId).subscribe(resp => { window.location.reload() });
  }
}
