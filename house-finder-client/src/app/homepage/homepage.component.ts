import { HouseService } from './../services/house.service';
import { Component, OnInit } from '@angular/core';
import { House } from '../models/house';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']
})
export class HomepageComponent implements OnInit {
  //List of all houses
  houses: House[] = [];

  constructor(private houseService: HouseService) { }

  ngOnInit(): void {
    this.houseService.getAllHouses().subscribe(data => {
      this.houses = data;
    });
  }
}
