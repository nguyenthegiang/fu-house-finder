import { Component, Input, OnInit } from '@angular/core';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-staff-landlord-detail-info',
  templateUrl: './staff-landlord-detail-info.component.html',
  styleUrls: ['./staff-landlord-detail-info.component.scss']
})
export class StaffLandlordDetailInfoComponent implements OnInit
{
  @Input() houseId!: number;

  totallyAvailableRoom: number = 0;
  partiallyAvailableRoom: number = 0;
  availableCapacity: number = 0;

  constructor(private roomService: RoomService)
  { }

  ngOnInit(): void
  {
    this.roomService.countTotallyAvailableRoomByHouseId(this.houseId).subscribe(data => {
      this.totallyAvailableRoom = data;
    });

    this.roomService.countPartiallyAvailableRoomByHouseId(this.houseId).subscribe(data => {
      this.partiallyAvailableRoom = data;
    });

    this.roomService.countAvailableCapacityByHouseId(this.houseId).subscribe(data => {
      this.availableCapacity = data;
    });
  }
}
