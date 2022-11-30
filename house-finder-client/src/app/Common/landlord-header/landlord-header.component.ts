import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-landlord-header',
  templateUrl: './landlord-header.component.html',
  styleUrls: ['./landlord-header.component.scss']
})
export class LandlordHeaderComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  backHome() {
    //use reload() so that when user is already in home -> they will get a reload of page
    window.location.href = "/dashboard";
  }

  logout(){
    window.location.href = "/login";
  }
}
