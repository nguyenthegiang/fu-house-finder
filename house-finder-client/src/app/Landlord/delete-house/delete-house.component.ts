import { Component, Input, OnInit } from '@angular/core';
import { HouseService } from 'src/app/services/house.service';

@Component({
  selector: 'app-delete-house',
  templateUrl: './delete-house.component.html',
  styleUrls: ['./delete-house.component.scss']
})
export class DeleteHouseComponent implements OnInit {
  @Input() houseId!: number;

  constructor(private houseService: HouseService) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Landlord
     */
    var userRole = localStorage.getItem("role");
    if (userRole != 'Landlord') {
      window.location.href = '/home';
    }
  }

  goBack(): void {
    window.location.reload();
  }

  deleteHouse() {
    this.houseService.deleteHouse(this.houseId).subscribe(() => this.goBack());
  }
}
