import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Room } from 'src/app/models/room';
import { LandlordInformationService } from 'src/app/services/landlord-information.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-staff-house-detail',
  templateUrl: './staff-house-detail.component.html',
  styleUrls: ['./staff-house-detail.component.scss']
})
export class StaffHouseDetailComponent implements OnInit {
  //List of available rooms
  rooms: Room[] = [];
  isOn = false;
  replyOn = false;
  availableRoom: number = 0;
  availableSlot: number = 0;

  //[Update] roomId to pass into <update-room>
  updateRoomId: number = 0;

  //{Search} input value
  searchValue: string | undefined;

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomService,
    private landlordInformationService: LandlordInformationService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.getListRoom();
  }

  getListRoom() {
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
      const id = Number(this.route.snapshot.paramMap.get('id'));
      this.roomService.getRooms(id).subscribe(data => {
        this.rooms = data;
      });
    } else {
      this.roomService.updateStatusRoom(2, roomId).subscribe();
      const id = Number(this.route.snapshot.paramMap.get('id'));
      this.roomService.getRooms(id).subscribe(data => {
        this.rooms = data;
      });
    }
  }

  goBack(): void {
    window.location.reload();
  }

  //for displaying 'Amount of People'
  peopleCounter(i: number) {
    return new Array(i);
  }

  viewRoomDetail(id: number)
  {
    this.router.navigate(['/Staff/staff-room-detail/' + id]);
  }

  search(searchValue: string)
  {}
}
