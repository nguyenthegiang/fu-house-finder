import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';
import { MatButtonModule } from '@angular/material/button';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-list-landlord-signup-request',
  templateUrl: './list-landlord-signup-request.component.html',
  styleUrls: ['./list-landlord-signup-request.component.scss'],
})
export class ListLandlordSignupRequestComponent implements OnInit {
  // Alerts
  @ViewChild('acceptLandlordAlert') private acceptLandlordAlert: SwalComponent | undefined;
  @ViewChild('denyLandlordStatusAlert') private denyLandlordStatusAlert: SwalComponent | undefined;

  //{Search} input value
  searchValue: string | undefined;

  landlordSignupRequest: User[] | undefined;
  selectedLandlord: User | undefined;

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

    this.reloadListRequest();
  }

  reloadListRequest() {
    this.userService.getLandlordSignUpRequest().subscribe((data) => {
      this.landlordSignupRequest = data;
    });
  }

  updateUserStatus(userId: string, statusId: number) {
    this.userService
      .updateUserStatus(userId, statusId)
      .subscribe(async (response) => {
        if (response.status == 200) {
          if (statusId == 1) this.acceptLandlordAlert?.fire();
          else this.denyLandlordStatusAlert?.fire();
          this.reloadListRequest();
        }
      });
  }

  changeSelectedLandlord(userId: string) {
    this.selectedLandlord = this.landlordSignupRequest!.find(
      (landlord) => landlord.userId == userId
    );
  }
}
