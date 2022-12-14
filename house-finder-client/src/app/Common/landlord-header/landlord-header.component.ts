import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-landlord-header',
  templateUrl: './landlord-header.component.html',
  styleUrls: ['./landlord-header.component.scss']
})
export class LandlordHeaderComponent implements OnInit {
  //Display name of current user
  userDisplayName: string = "";

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    //Get displayName from localStorage
    this.userDisplayName = localStorage.getItem('user') || '{}';
  }

  //back to Landlord Dashboard
  backHome() {
    window.location.href = "/Landlord/dashboard";
  }

  //Log out of systen
  logout() {
    localStorage.removeItem("user");
    localStorage.removeItem("role");
    this.userService.logout().subscribe(resp => {
      window.location.href = "/login";
    },
    error => {
      window.location.href = "/login";
    });
  }
}
