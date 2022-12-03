import { Component, OnInit, ViewChild } from '@angular/core';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-staff-change-password',
  templateUrl: './staff-change-password.component.html',
  styleUrls: ['./staff-change-password.component.scss']
})
export class StaffChangePasswordComponent implements OnInit {
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
