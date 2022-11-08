import { LandlordInformationService } from './../../services/landlord-information.service';
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
  availableRoom: number = 0;
  availableSlot: number = 0;

  //[Update] roomId to pass into <update-room>
  updateRoomId: number = 0;

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomService,
    private landlordInformationService: LandlordInformationService,
    private router: Router
  ) { }

  ngOnInit(): void {
    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));

    //Call API: get rooms of this house
    this.landlordInformationService.getLandLordInfomation("LA000003").subscribe(data => {
      this.availableRoom = data.roomAvailableCount;
    });

    //Call API: get rooms of this house
    this.roomService.getRooms(id).subscribe(data => {
      this.rooms = data;
    });
  }

  //[Filter] Filter by Room Type
  onStatusRoomSelected(event: any, roomId: number) {
    //see if user just checked or unchecked the checkbox
    const isChecked = (<HTMLInputElement>event.target).checked;

    //if user check -> add roomTypeId to the list
    if (isChecked) {
      this.roomService.updateStatusRoom(1, roomId).subscribe();
    } else {
      this.roomService.updateStatusRoom(2, roomId).subscribe();
    }
  }

  updateRoom(id: number) {
    this.router.navigate(['/Landlord/update-room/' + id]);
  }

  //[Update] pass roomId and status check to <update-room> component to call API
  editRoom(roomId: number) {
    this.updateRoomId = roomId;
  }

  deleteRoom(id: number) {

  }

  //for displaying 'Amount of People'
  peopleCounter(i: number) {
    return new Array(i);
  }
}
