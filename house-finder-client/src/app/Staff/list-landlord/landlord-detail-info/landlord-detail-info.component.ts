import { LandlordInformationService } from './../../../services/landlord-information.service';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-landlord-detail-info',
  templateUrl: './landlord-detail-info.component.html',
  styleUrls: ['./landlord-detail-info.component.scss']
})
export class LandlordDetailInfoComponent implements OnInit {
  //receive Id of Landlord through parameter of Component
  @Input() landlordId!: string;

  //statistic data
  houseCount: number = 0;
  roomCount: number = 0;
  roomAvailableCount: number = 0;

  constructor(private lanlord_informationService: LandlordInformationService) { }

  ngOnInit(): void {
    this.lanlord_informationService.getLandLordInfomation(this.landlordId).subscribe(data => {
      this.houseCount = data.houseCount;
      this.roomCount = data.roomCount;
      this.roomAvailableCount = data.roomAvailableCount;
    });
  }

}
