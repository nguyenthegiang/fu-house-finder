import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  //Flag to check if user has logged in
  logged_in = false;

  //Display name of current user
  userDisplayName: string = "";

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.checkLoggedIn();

    //Get displayName from localStorage
    this.userDisplayName = localStorage.getItem('user') || '{}';
  }

  //Home button -> go back to /home
  backHome() {
    //use reload() so that when user is already in home -> they will get a reload of page
    window.location.href = "/home";
  }

  //go to Log in screen
  login() {
    window.location.href = "/login";
  }

  //Log out of systen
  logout() {
    localStorage.removeItem("user");
    localStorage.removeItem("role");
    this.userService.logout().subscribe(resp => {
      window.location.reload();
    },
    error => {
      window.location.reload();
    });
  }

  //Check if user has logged in
  checkLoggedIn() {
    if (localStorage.getItem("user") != null) {
      this.logged_in = true;
    }
  }
}
