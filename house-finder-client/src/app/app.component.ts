import { Component } from '@angular/core';
import { CampusService } from './services/campus.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'FU House Finder';
  campuses: any = [];

  constructor(private campusService: CampusService) { }

  ngOnInit(): void {
    this.campusService.getAllCampuses().subscribe(data => {
      this.campuses = data;
    });
  }
}
