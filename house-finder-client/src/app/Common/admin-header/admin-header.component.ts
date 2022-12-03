import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.scss']
})
export class AdminHeaderComponent implements OnInit {

  constructor(private userService: UserService,
    private router: Router) { }

  ngOnInit(): void {
  }

  backHome() {
    //use reload() so that when user is already in home -> they will get a reload of page
    window.location.href = "/Admin/list-staff";
  }

  logout(){
    this.userService.logout().subscribe(resp => {
      localStorage.clear();
      sessionStorage.clear();
      window.location.href = "/login";
    });
  }

  isListStaffRoute()
  {
    return this.router.url === "/Admin/list-staff";
  }

  isChangePasswordRoute()
  {
    return this.router.url === "/Admin/change-password";
  }
}
