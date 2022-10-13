import { User } from './../models/user';
import { CampusService } from './../services/campus.service';
import { UserService } from './../services/user.service';
import { House } from './../models/house';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HouseService } from '../services/house.service';
import { RoomService } from '../services/room.service';
import { Observable } from 'rxjs';
import { Room } from '../models/room';

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
  //List of available rooms
  availableRooms: Room[] = [];

  constructor(
    private route: ActivatedRoute,
    private houseService: HouseService,
    private userService: UserService,
    private roomService: RoomService,
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

      //Call API: get available rooms of this house
      this.roomService.getAvailableRooms(id).subscribe(data => {
        this.availableRooms = data;
      });
    });
  }

}
