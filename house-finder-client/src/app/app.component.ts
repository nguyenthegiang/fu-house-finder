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

  //to check whether to display footer or not
  public displayFooter: boolean = true;

  constructor(private router: Router) { }

  ngOnInit() {
    // Not display footer in Login page
    this.router.events.subscribe((val) => {
      if (this.router.url == '/login') {
        this.displayFooter = false;
      } else {
        this.displayFooter = true;
      }
    });
  }

}
