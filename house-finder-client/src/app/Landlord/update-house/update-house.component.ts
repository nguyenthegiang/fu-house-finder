import { HouseService } from 'src/app/services/house.service';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { House } from 'src/app/models/house';

@Component({
  selector: 'app-update-house',
  templateUrl: './update-house.component.html',
  styleUrls: ['./update-house.component.scss']
})
export class UpdateHouseComponent implements OnInit
{
  //Detail information of this House
  houseDetail: House | undefined;

  constructor(private houseService: HouseService,
    private route: ActivatedRoute,
    private router: Router)
  { }

  ngOnInit(): void
  {
    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.houseService.getHouseByHouseId(id).subscribe(data => {
      this.houseDetail = data;
    });
  }

  goBack(): void
  {
    window.location.reload();
  }
}
