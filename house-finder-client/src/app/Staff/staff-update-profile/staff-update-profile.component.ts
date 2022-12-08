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
  staffId: string = "";
  updateName: string = "";
  updateEmail: string = "";

  @ViewChild('updateSuccessAlert') private updateSuccessAlert: SwalComponent | undefined;
  @ViewChild('updateErrorAlert') private updateErrorAlert: SwalComponent | undefined;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe(data => {
      this.staffDetail = data;
      this.staffId = this.staffDetail.userId;
      this.updateName = this.staffDetail.displayName;
      this.updateEmail = this.staffDetail.email;
    });
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
      this.userService.updateProfile(this.staffId, this.updateName, this.updateEmail).subscribe(
        data => {
          if (data.status == 403) {
            this.updateErrorAlert?.fire();
          } else if (data.status == 200) {
            this.updateSuccessAlert?.fire();
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
