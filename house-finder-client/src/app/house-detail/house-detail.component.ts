import { User } from './../models/user';
import { CampusService } from './../services/campus.service';
import { UserService } from './../services/user.service';
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
  //Detail information of this House
  houseDetail: House | undefined;
  //Landlord of this house
  landlordDetail: User | undefined;

  constructor(
    private route: ActivatedRoute,
    private houseService: HouseService,
    private userService: UserService,
  ) { }

  ngOnInit(): void {
    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));
    //Call API: get House Detail information
    this.houseService.getHouseByHouseId(id).subscribe(data => {
      this.houseDetail = data;

      //Call API: get this House's Landlord detail information (after get house detail info)
      this.userService.getUserByUserId(this.houseDetail?.landlordId).subscribe(data => {
          this.landlordDetail = data;
      });
    });
  }

}
