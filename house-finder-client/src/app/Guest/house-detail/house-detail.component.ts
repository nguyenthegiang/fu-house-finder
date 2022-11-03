import { User } from '../../models/user';
import { CampusService } from '../../services/campus.service';
import { UserService } from '../../services/user.service';
import { House } from '../../models/house';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HouseService } from '../../services/house.service';
import { RoomService } from '../../services/room.service';
import { Observable } from 'rxjs';
import { Room } from '../../models/room';
import { ReportService } from '../../services/report.service';
import { Report } from '../../models/report';

@Component({
  selector: 'app-house-detail',
  templateUrl: './house-detail.component.html',
  styleUrls: ['./house-detail.component.scss']
})

export class HouseDetailComponent implements OnInit {
  //Detail information of this House
  houseDetail: House | undefined;
  //Detail image of this House
  houseImage: string[] = [];
  //Landlord of this house
  landlordDetail: User | undefined;
  //List of available rooms
  availableRooms: Room[] = [];
  //(Paging)
  countAvailableHouses = 0; //items count
  pageSize = 9; //number of items per page
  pageNumber = 1; //starts at page 1
  isOn = false;

  constructor(
    private route: ActivatedRoute,
    private houseService: HouseService,
    private userService: UserService,
    private roomService: RoomService,
    private reportService: ReportService,
    private router: Router
  ) { }

  ngOnInit(): void {
    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));
    //Call API: get House Detail information
    this.houseService.getHouseByHouseId(id).subscribe(data => {
      this.houseDetail = data;

      //Call API: get this House's Landlord detail information (after get house detail info)
      this.userService.getUserByUserId(this.houseDetail?.landlordId).subscribe(data => {
        this.landlordDetail = data;
      });

      //Call API: get available rooms of this house
      this.roomService.getAvailableRooms(id).subscribe(data => {
        this.availableRooms = data;
      });
    });

    //(Paging) Count available Houses for total number of pages
    this.houseService.countTotalAvailableHouse().subscribe(data => {
      this.countAvailableHouses = data;
    });
  }

  //Send the Report for House
  sendReport(inputReportContent: string) {
    inputReportContent = inputReportContent.trim();

    //Create Report
    const report: Report = {
      studentId: 'HE153046',   //Fake data, fix later when have Login
      houseId: this.houseDetail!.houseId,
      reportContent: inputReportContent,
      createdBy: 'HE153046',
      lastModifiedBy: 'HE153046'
    };

    this.reportService.addReport(report).subscribe();
  }

  viewRoom(id: number)
  {
    this.router.navigate(['/room-detail/' + id]);
  }

  //for displaying 'Amount of People'
  peopleCounter(i: number) {
    return new Array(i);
  }
}
