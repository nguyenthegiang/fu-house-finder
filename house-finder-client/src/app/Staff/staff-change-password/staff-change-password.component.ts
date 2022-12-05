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

  @ViewChild('updateSuccessAlert') private updateSuccessAlert: SwalComponent | undefined;
  @ViewChild('updateErrorAlert') private updateErrorAlert: SwalComponent | undefined;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe(data => {
      this.staffDetail = data;
      this.staffId = this.staffDetail.userId;
    });
    console.log(this.oldPassword);
  }

  inputPassword(value: string)
  {
    this.oldPassword = value;
  }

  update()
  {
    //Check if user has logged in
    var user = null;
    user = localStorage.getItem("user");
    if (user === null) {
      //user not logged in => Alert
      this.updateErrorAlert?.fire();
    } else {
      //Check user logged in from Server => if not => alert
      this.userService.changePassword(this.staffId, this.newPassword).subscribe(
        data => {
          if (data.status == 403) {
            this.updateErrorAlert?.fire();
          } else if (data.status == 200) {
            this.updateSuccessAlert?.fire();
            window.location.reload();
          }
        },
        error => { }
      );
    }
  }

  goBack(): void
  {
    window.location.reload();
  }
}
