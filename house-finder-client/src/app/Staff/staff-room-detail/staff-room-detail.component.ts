import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { House } from 'src/app/models/house';
import { Room } from 'src/app/models/room';
import { User } from 'src/app/models/user';
import { HouseService } from 'src/app/services/house.service';
import { RoomService } from 'src/app/services/room.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-staff-room-detail',
  templateUrl: './staff-room-detail.component.html',
  styleUrls: ['./staff-room-detail.component.scss']
})
export class StaffRoomDetailComponent implements OnInit {
  //Detail information of this House
  houseDetail: House | undefined;
  //Detail information of this Room
  roomDetail: Room | undefined;
  //Landlord of this house
  landlordDetail: User | undefined;

  constructor(private route: ActivatedRoute,
    private houseService: HouseService,
    private roomService: RoomService,
    private userService: UserService) { }

  ngOnInit(): void {
    //Get id of Room from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));

    //Call API: get Room Detail information
    this.roomService.getRoomByRoomId(id).subscribe(data => {
      this.roomDetail = data;

      //Call API: get House Detail information (after get room detail info)
      this.houseService.getHouseByHouseId(this.roomDetail?.houseId).subscribe(data => {
        this.houseDetail = data;

        //Call API: get this House's Landlord detail information (after get house detail info)
        this.userService.getUserByUserId(this.houseDetail?.landlordId).subscribe(data => {
          this.landlordDetail = data;
        });
      });
    });
  }
}
