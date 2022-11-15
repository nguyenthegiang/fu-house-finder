import { Component, Input, OnInit } from '@angular/core';
import { HouseService } from 'src/app/services/house.service';

@Component({
  selector: 'app-dashboard-info',
  templateUrl: './dashboard-info.component.html',
  styleUrls: ['./dashboard-info.component.scss']
})
export class DashboardInfoComponent implements OnInit
{
  @Input() houseId!: number;
  moneyForNotRentedRooms: number = 0;

  constructor(private houseService: HouseService)
  { }

  ngOnInit(): void
  {
    this.houseService.getMoneyForNotRentedRooms(this.houseId).subscribe(data => {
      this.moneyForNotRentedRooms = data;
    });
  }
}
