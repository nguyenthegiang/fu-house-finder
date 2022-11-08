import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { CampusService } from './services/campus.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'FU House Finder';
  
  constructor() { }

  ngOnInit() { }

  //Home button -> go back to /home
  backHome() {
    //use reload() so that when user is already in home -> they will get a reload of page
    window.location.href = "/home";
  }

  login(){
    window.location.href = "/login";
  }
}
