import { HouseService } from 'src/app/services/house.service';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-update-house',
  templateUrl: './update-house.component.html',
  styleUrls: ['./update-house.component.scss']
})
export class UpdateHouseComponent implements OnInit
{
  @Input() houseId!: number;

  constructor(private houseService: HouseService)
  { }

  ngOnInit(): void
  {

  }

  goBack(): void
  {
    window.location.reload();
  }

  deleteHouse()
  {
    console.log(this.houseId);
    this.houseService.deleteHouse(this.houseId).subscribe(() => this.goBack());
  }
}
