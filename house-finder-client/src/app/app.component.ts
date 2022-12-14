import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'FU House Finder';
  
  public displayFooter: boolean = true;

  constructor() { }

  ngOnInit() {
    if (window.location.pathname == '/login') {
      this.displayFooter = false;
    }
  }

}
