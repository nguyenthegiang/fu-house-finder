import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  logged_in = false;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.checkLoggedIn();
  }
  //Home button -> go back to /home
  backHome() {
    //use reload() so that when user is already in home -> they will get a reload of page
    window.location.href = "/home";
  }

  login(){
    window.location.href = "/login";
  }

  logout(){
    this.userService.logout().subscribe(resp => {
      localStorage.clear();
      sessionStorage.clear();
      window.location.reload();
    });
  }

  checkLoggedIn(){
    if (localStorage.getItem("user") != null){
      this.logged_in = true;
    }
  }

}
