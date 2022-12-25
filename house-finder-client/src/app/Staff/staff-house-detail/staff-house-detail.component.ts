import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { House } from 'src/app/models/house';
import { Room } from 'src/app/models/room';
import { User } from 'src/app/models/user';
import { HouseService } from 'src/app/services/house.service';
import { LandlordInformationService } from 'src/app/services/landlord-information.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-staff-house-detail',
  templateUrl: './staff-house-detail.component.html',
  styleUrls: ['./staff-house-detail.component.scss']
})
export class StaffHouseDetailComponent implements OnInit {
  //Landlord of this house
  landlordId: string = '';
  //Detail information of this House
  houseDetail: House | undefined;
  //Detail image of this House
  houseImage: string[] = [];
  //List of rooms
  rooms: Room[] = [];
  isOn = false;
  replyOn = false;
  partiallyAvailableRoom: number = 0;
  totallyAvailableRoom: number = 0;
  availableRoom: number = 0;
  availableSlot: number = 0;

  //[Update] roomId to pass into <update-room>
  updateRoomId: number = 0;

  //{Search} input value
  searchValue: string | undefined;

  checkBreadcrumb: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomService,
    private landlordInformationService: LandlordInformationService,
    private router: Router,
    private houseService: HouseService
  ) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Staff
     */
    var userRole = localStorage.getItem("role");
    if (userRole == null || userRole!.indexOf('Department') < 0) {
      window.location.href = '/home';
    }

    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));
    //Call API: get House Detail information
    this.houseService.getHouseByHouseId(id).subscribe(data => {
      this.houseDetail = data;
      this.landlordId = data.landlordId;
    });
    this.getListRoom();

    this.roomService.countPartiallyAvailableRoomByHouseId(id).subscribe(data => {
      this.partiallyAvailableRoom = data;

      this.roomService.countTotallyAvailableRoomByHouseId(id).subscribe(data => {
        this.totallyAvailableRoom = data;
        this.availableRoom = this.partiallyAvailableRoom + this.totallyAvailableRoom;
      });
    });

    this.roomService.countAvailableCapacityByHouseId(id).subscribe(data => {
      this.availableSlot = data;
    });

    if (localStorage.getItem('breadcrumb') == 'true') {
      this.checkBreadcrumb = true;
    }

    if (localStorage.getItem('breadcrumb') == 'false') {
      this.checkBreadcrumb = false;
    }
  }

  getListRoom() {
    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));

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

  viewRoomDetail(id: number) {
    this.router.navigate(['/Staff/staff-room-detail/' + id]);
  }

  search(searchValue: string) { }
}
