import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-staff-header',
  templateUrl: './staff-header.component.html',
  styleUrls: ['./staff-header.component.scss']
})
export class StaffHeaderComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  //Home button -> go back to /home
  backHome() {
    //use reload() so that when user is already in home -> they will get a reload of page
    window.location.href = "/home";
  }

  login(){
    window.location.href = "/login";
  }
}
