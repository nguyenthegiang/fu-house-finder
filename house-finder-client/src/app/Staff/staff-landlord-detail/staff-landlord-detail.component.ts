import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { House } from 'src/app/models/house';
import { User } from 'src/app/models/user';
import { HouseService } from 'src/app/services/house.service';
import { LandlordInformationService } from 'src/app/services/landlord-information.service';
import { RoomService } from 'src/app/services/room.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-staff-landlord-detail',
  templateUrl: './staff-landlord-detail.component.html',
  styleUrls: ['./staff-landlord-detail.component.scss']
})
export class StaffLandlordDetailComponent implements OnInit {
  //List of all houses
  houses: House[] = [];
  //(Environment) Google Map API Key, to display Google Map iframe embed
  mapAPIkey = environment.google_maps_api_key;
  //Detail information of this House
  houseDetail: House | undefined;
  //URL for src in <iframe> Google Map
  mapUrl: string | undefined;
  //Detail image of this House
  houseImage: string[] = [];
  //Landlord of this house
  landlordDetail: User | undefined;

  //{Search} input value
  totallyAvailableRoomByHouseId: number = 0;
  totallyPartiallyRoomByHouseId: number = 0;
  totallyAvailableCapacityByHouseId: number = 0;
  houseCount: number = 0;
  roomCount: number = 0;
  roomAvailableCount: number = 0;

  //{Search} input value
  searchValue: string | undefined;

  constructor(private houseService: HouseService,
    private lanlord_informationService: LandlordInformationService,
    private router: Router,
    private route: ActivatedRoute,
    private roomService: RoomService,
    private userService: UserService)
    { }

  ngOnInit(): void {
    //Get id of House from Route
    const id = String(this.route.snapshot.paramMap.get('id'));

    //Call API: get this House's Landlord detail information (after get house detail info)
    this.userService.getUserByUserId(id).subscribe(data => {
      this.landlordDetail = data;
    });
    //Get List of all Houses
    this.houseService.getListHousesByLandlordId(id).subscribe(data => {
      this.houses = data;
    });

    //Get Data for Statistics
    this.lanlord_informationService.getLandLordInfomation(id).subscribe(data => {
      this.houseCount = data.houseCount;
      this.roomCount = data.roomCount;
      this.roomAvailableCount = data.roomAvailableCount;
    });

    this.roomService.countTotallyAvailableRoomByHouseId(1).subscribe(data => {
      this.totallyAvailableRoomByHouseId = data;
    });

    this.roomService.countPartiallyAvailableRoomByHouseId(1).subscribe(data => {
      this.totallyPartiallyRoomByHouseId = data;
    });

    this.roomService.countAvailableCapacityByHouseId(1).subscribe(data => {
      this.totallyAvailableCapacityByHouseId = data;
    });
  }

  viewRoom(id: number)
  {
    console.log(id);
    this.router.navigate(['/Staff/staff-house-detail/' + id]);
  }

  search(searchValue: string)
  {}
}
