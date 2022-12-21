import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { Rate } from 'src/app/models/rate';
import { RateService } from 'src/app/services/rate.service';

@Component({
  selector: 'app-rate-house',
  templateUrl: './rate-house.component.html',
  styleUrls: ['./rate-house.component.scss']
})
export class RateHouseComponent implements OnInit {
  replyOn = false;
  rates: Rate[] = [];
  rateDetail: Rate | undefined;
  reply: string = "";
  houseId: number = 0;
  isReply: boolean = false;
  indexReply: number = -1;
  rateId: number = -1;

  @ViewChild('rateSuccessAlert') private rateSuccessAlert: SwalComponent | undefined;
  @ViewChild('rateErrorAlert') private rateErrorAlert: SwalComponent | undefined;

  constructor(private route: ActivatedRoute,
    private rateService: RateService,
    private router: Router) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Landlord
     */
    var userRole = localStorage.getItem("role");
    if (userRole != 'Landlord') {
      window.location.href = '/home';
    }

    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.houseId = id;

    this.rateService.getListRatesByHouseId(this.houseId).subscribe(data => {
      this.rates = data;
    });
  }

  logout() {
    window.location.href = "/login";
  }

  openReply(index: number) {
    this.isReply = true;
    this.indexReply = index;
  }

  closeReply() {
    this.isReply = false;
    this.indexReply = -1;
  }

  addReply(rateId: number) {
    this.rateService.updateRate(rateId, this.reply).subscribe(() => this.goBack());
  }

  goBack(): void {
    window.location.reload();
  }
}
