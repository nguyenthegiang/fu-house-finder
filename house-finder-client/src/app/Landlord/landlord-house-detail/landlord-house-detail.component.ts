import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { House } from 'src/app/models/house';
import { Room } from 'src/app/models/room';
import { User } from 'src/app/models/user';
import { HouseService } from 'src/app/services/house.service';
import { RoomService } from 'src/app/services/room.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-landlord-house-detail',
  templateUrl: './landlord-house-detail.component.html',
  styleUrls: ['./landlord-house-detail.component.scss']
})
export class LandlordHouseDetailComponent implements OnInit {
  //List of available rooms
  rooms: Room[] = [];
  isOn = false;
  replyOn = false;

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomService,
    private router: Router
  ) { }

  ngOnInit(): void {
    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));

    //Call API: get available rooms of this house
    this.roomService.getRooms(id).subscribe(data => {
      this.rooms = data;
    });
  }

  viewRoom(id: number)
  {
    this.router.navigate(['/room-detail/' + id]);
  }

  //for displaying 'Amount of People'
  peopleCounter(i: number) {
    return new Array(i);
  }
}
