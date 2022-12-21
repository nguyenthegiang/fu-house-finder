import { Component, OnInit, ViewChild } from '@angular/core';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-list-rejected-landlord',
  templateUrl: './list-rejected-landlord.component.html',
  styleUrls: ['./list-rejected-landlord.component.scss']
})
export class ListRejectedLandlordComponent implements OnInit {
  @ViewChild('acceptLandlordAlert') private acceptLandlordAlert:
    | SwalComponent
    | undefined;
  @ViewChild('denyLandlordStatusAlert') private denyLandlordStatusAlert:
    | SwalComponent
    | undefined;
  @ViewChild('updateLandlordStatusFailAlert')
  private updateLandlordStatusFailAlert: SwalComponent | undefined;

  rejectedLandlords: User[] = [];
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
    this.userService.getRejectedLandlord().subscribe((data) => {
      this.rejectedLandlords = data;
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
    this.selectedLandlord = this.rejectedLandlords.find(
      (landlord) => landlord.userId == userId
    );
  }

}
