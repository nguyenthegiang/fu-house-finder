import { ɵparseCookieValue } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-admin-header',
  templateUrl: './admin-header.component.html',
  styleUrls: ['./admin-header.component.scss']
})
export class AdminHeaderComponent implements OnInit {

  constructor(private userService: UserService) { }

  ngOnInit(): void {
  }

  //back to Admin Dashboard
  backHome() {
    window.location.href = "/Admin/list-staff";
  }

  //Log out of system
  logout() {
    localStorage.clear();
    sessionStorage.clear();
    this.userService.logout().subscribe(resp => {
      window.location.href = "/login";
    },
    error => {
      window.location.href = "/login";
    });
  }
}
