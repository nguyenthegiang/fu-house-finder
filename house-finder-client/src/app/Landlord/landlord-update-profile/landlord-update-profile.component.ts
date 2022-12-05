import { Component, OnInit, ViewChild } from '@angular/core';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-landlord-update-profile',
  templateUrl: './landlord-update-profile.component.html',
  styleUrls: ['./landlord-update-profile.component.scss']
})
export class LandlordUpdateProfileComponent implements OnInit {
  landlordDetail: User | undefined;
  landlordId: string = "";
  updateName: string = "";
  updateEmail: string = "";

  @ViewChild('updateSuccessAlert') private updateSuccessAlert: SwalComponent | undefined;
  @ViewChild('updateErrorAlert') private updateErrorAlert: SwalComponent | undefined;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getCurrentUser().subscribe(data => {
      this.landlordDetail = data;
      this.landlordId = this.landlordDetail.userId;
      this.updateName = this.landlordDetail.displayName;
      this.updateEmail = this.landlordDetail.email;
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
      this.userService.updateProfile(this.landlordId, this.updateName, this.updateEmail).subscribe(
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