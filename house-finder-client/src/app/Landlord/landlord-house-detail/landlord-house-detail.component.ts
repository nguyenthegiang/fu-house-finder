import { LandlordInformationService } from './../../services/landlord-information.service';
import { Component, OnInit, SimpleChanges } from '@angular/core';
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
  rooms: Room[] | undefined;
  isOn = false;
  replyOn = false;
  houseId = 0;

  totallyAvailableRoom: number = 0;
  partiallyAvailableRoom: number = 0;
  availableSlot: number = 0;
  moneyForNotRentedRooms: number = 0;

  //[Update] roomId to pass into <update-room>
  updateRoomId: number = 0;

  //Detail information of this House - to get HouseName to display
  houseDetail: House | undefined;

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomService,
    private houseService: HouseService,
    private router: Router
  ) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Landlord
     */
    var userRole = localStorage.getItem("role");
    if (userRole != 'Landlord') {
      window.location.href = '/home';
    }
    
    this.getListRoom();
  }

  getListRoom() {
    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.houseId = id;

    //Call API: get rooms of this house
    this.roomService.getRooms(id).subscribe(data => {
      this.rooms = data;
    });

    this.roomService.countTotallyAvailableRoomByHouseId(id).subscribe(data => {
      this.totallyAvailableRoom = data;
    });

    this.roomService.countPartiallyAvailableRoomByHouseId(id).subscribe(data => {
      this.partiallyAvailableRoom = data;
    });

    this.roomService.countAvailableCapacityByHouseId(id).subscribe(data => {
      this.availableSlot = data;
    });

    this.houseService.getMoneyForNotRentedRooms(id).subscribe(data => {
      this.moneyForNotRentedRooms = data;
    });

    //Get houseDetail to take HouseName
    this.houseService.getHouseByHouseId(id).subscribe(resp => {
      this.houseDetail = resp;
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

  updateRoom(id: number) {
    this.router.navigate(['/Landlord/update-room/' + id]);
  }

  //[Update] pass roomId and status check to <update-room> component to call API
  editRoom(roomId: number) {
    this.updateRoomId = roomId;
  }

  addRoom() {
    window.location.href = `/Landlord/add-room?houseId=${this.houseId}`;
  }

  deleteRoom(id: number) {
    this.roomService.deleteRoom(id).subscribe(resp => {window.location.reload();}, err => {});
  }

  //for displaying 'Amount of People'
  peopleCounter(i: number) {
    return new Array(i);
  }

  search(searchValue: string) { }

  logout() {
    window.location.href = "/login";
  }
}
