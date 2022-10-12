import { House } from './../models/house';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HouseService } from '../services/house.service';

@Component({
  selector: 'app-house-detail',
  templateUrl: './house-detail.component.html',
  styleUrls: ['./house-detail.component.scss']
})
export class HouseDetailComponent implements OnInit {
  houseDetail: House | undefined;

  constructor(private route: ActivatedRoute, private houseService: HouseService) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.houseService.getHouseByHouseId(id).subscribe(data => {
      this.houseDetail = data;
      alert(this.houseDetail?.houseName);
    });

    
  }

}
