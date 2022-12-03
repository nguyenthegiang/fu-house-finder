import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-staff-update-profile',
  templateUrl: './staff-update-profile.component.html',
  styleUrls: ['./staff-update-profile.component.scss']
})
export class StaffUpdateProfileComponent implements OnInit {
  staffDetail: User | undefined;

  @ViewChild('updateSuccessAlert') private updateSuccessAlert: SwalComponent | undefined;
  @ViewChild('updateErrorAlert') private updateErrorAlert: SwalComponent | undefined;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe(data => {
      this.staffDetail = data;
    });
  }
}
