import { HouseService } from './../services/house.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']
})
export class HomepageComponent implements OnInit {
  //List of all houses
  houses: any = [];

  constructor(private houseService: HouseService) { }

  ngOnInit(): void {
    this.houseService.getAllHouses().subscribe(data => {
      this.houses = data;
    });
  }
}
