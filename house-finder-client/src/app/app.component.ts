import { Component, OnInit } from '@angular/core';
import { CampusService } from './services/campus.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'FU House Finder';
  campuses: any = [];

  constructor(private campusService: CampusService) {}

  ngOnInit(): void {
    this.campusService.getAllCampuses().subscribe(data => {
      this.campuses = data;
    });
  }
}
