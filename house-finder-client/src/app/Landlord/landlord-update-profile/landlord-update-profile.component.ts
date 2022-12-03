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
