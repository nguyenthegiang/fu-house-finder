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
  staffId: string = "";
  oldPassword: string = "";
  newPassword: string = "";
  retypePassword: string = "";
  incorrectOldPassword = false;
  emptyNewPassword = false;
  EmptyRetypePassword = false;
  notMatchNewPassword = false;

  @ViewChild('updateSuccessAlert') private updateSuccessAlert: SwalComponent | undefined;
  @ViewChild('updateErrorAlert') private updateErrorAlert: SwalComponent | undefined;
  @ViewChild('notLoginErrorAlert') private notLoginErrorAlert: SwalComponent | undefined;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Staff
     */
    var userRole = localStorage.getItem("role");
    if (userRole == null || userRole!.indexOf('Department') < 0) {
      window.location.href = '/home';
    }
  }


  update() {
    //Check if user has logged in
    var user = null;
    user = localStorage.getItem("user");
    if (user === null) {
      //user not logged in => Alert
      this.notLoginErrorAlert?.fire();
    } else {
      // Check input
      if (this.newPassword == "") {
        this.emptyNewPassword = true;
      }
      else {
        this.emptyNewPassword = false;
      }
      if (this.retypePassword == "") {
        this.EmptyRetypePassword = true;
      }
      else {
        this.EmptyRetypePassword = false;
      }

      if (!this.emptyNewPassword && !this.EmptyRetypePassword && this.newPassword != this.retypePassword) {
        this.notMatchNewPassword = true;
      }
      else {
        this.notMatchNewPassword = false;
      }

      if (!this.emptyNewPassword && !this.EmptyRetypePassword && !this.notMatchNewPassword) {
        // change password
        this.userService.changePassword(this.oldPassword, this.newPassword).subscribe(
          data => {
            this.updateSuccessAlert?.fire();
            window.location.reload();
          },
          error => {
            if (error.status == 409) {
              this.incorrectOldPassword = true;
            }
            else if (error.status == 403) {
              this.notLoginErrorAlert?.fire();
            }
            else {
              this.updateErrorAlert?.fire();
            }
          }
        );
      }
    }
  }

  goBack(): void {
    window.location.reload();
  }
}
