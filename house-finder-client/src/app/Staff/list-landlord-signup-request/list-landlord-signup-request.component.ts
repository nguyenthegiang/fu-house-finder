import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-list-landlord-signup-request',
  templateUrl: './list-landlord-signup-request.component.html',
  styleUrls: ['./list-landlord-signup-request.component.scss']
})
export class ListLandlordSignupRequestComponent implements OnInit {

  //{Search} input value
  searchValue: string | undefined;

  landlordSignupRequest: User[] = [];

  constructor(private userService: UserService,
    )
  { }

  ngOnInit(): void
  {
    this.reloadListRequest();
  }

  reloadListRequest(){
    this.userService.getLandlordSignUpRequest().subscribe((data) =>{
      this.landlordSignupRequest = data
    }
    )
  }

  search(searchValue: string)
  {}

  updateUserStatus(userId: string, statusId: number){
    this.userService.updateUserStatus(userId, statusId).subscribe();
    this.reloadListRequest();
  }
}
