import { Component, OnInit, ViewChild } from '@angular/core';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { User } from 'src/app/models/user';
import { FileService } from 'src/app/services/file.service';
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
  updatePhoneNumber: string = "";
  updateFacebookUrl: string = "";
  updateIdentityCardFrontSideImageLink: string = "";
  updateIdentityCardBackSideImageLink: string = "";

  frontImg: any;
  backImg: any;


  @ViewChild('updateSuccessAlert') private updateSuccessAlert: SwalComponent | undefined;
  @ViewChild('updateErrorAlert') private updateErrorAlert: SwalComponent | undefined;

  constructor(private userService: UserService, private fileService: FileService) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Landlord
     */
    var userRole = localStorage.getItem("role");
    if (userRole != 'Landlord') {
      window.location.href = '/home';
    }

    this.userService.getCurrentUser().subscribe(data => {
      this.landlordDetail = data;
      this.landlordId = this.landlordDetail.userId;
      this.updateName = this.landlordDetail.displayName;
      this.updatePhoneNumber = this.landlordDetail.phoneNumber;
      this.updateFacebookUrl = this.landlordDetail.facebookUrl;
      this.updateIdentityCardFrontSideImageLink = this.landlordDetail.identityCardFrontSideImageLink;
      this.updateIdentityCardBackSideImageLink = this.landlordDetail.identityCardBackSideImageLink;
    });
  }

  update() {
    //Check if user has logged in
    var user = null;
    user = localStorage.getItem("user");
    if (user === null) {
      //user not logged in => Alert
      this.updateErrorAlert?.fire();
    } else {
      //Check user logged in from Server => if not => alert
      this.userService.landlordUpdateProfile(this.landlordId, this.updateName, this.updatePhoneNumber, this.updateFacebookUrl).subscribe(
        data => {
          if (data.status == 403) {
            this.updateErrorAlert?.fire();
          } else if (data.status == 200) {
            localStorage.setItem('user', this.updateName);
            this.updateSuccessAlert?.fire();
          }
        },
        error => { }
      );
      this.fileService.uploadIDC(this.frontImg, this.backImg).subscribe(
        data => {
        },
        error => { }
      );
    }
  }

  displayImage(event: any, side: string): void {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];

      const reader = new FileReader();

      if (side == 'front') {
        reader.onload = e => this.updateIdentityCardFrontSideImageLink = reader.result!.toString();
        reader.readAsDataURL(file);
        this.frontImg = file;
      }
      else if (side == 'back') {
        reader.onload = e => this.updateIdentityCardBackSideImageLink = reader.result!.toString();
        reader.readAsDataURL(file);
        this.backImg = file;
      }
    }
  }

  goBack(): void {
    window.location.reload();
  }
}
