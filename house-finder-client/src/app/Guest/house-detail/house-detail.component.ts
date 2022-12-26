import { CampusService } from 'src/app/services/campus.service';
import { Campus } from 'src/app/models/campus';
import { RateService } from './../../services/rate.service';
import { Rate } from './../../models/rate';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { House } from '../../models/house';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HouseService } from '../../services/house.service';
import { RoomService } from '../../services/room.service';
import { Observable } from 'rxjs';
import { Room } from '../../models/room';
import { ReportService } from '../../services/report.service';
import { Report } from '../../models/report';
import { environment } from 'src/environments/environment'; //environment variable
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-house-detail',
  templateUrl: './house-detail.component.html',
  styleUrls: ['./house-detail.component.scss']
})

export class HouseDetailComponent implements OnInit {
  //(Environment) Google Map API Key, to display Google Map iframe embed
  mapAPIkey = environment.google_maps_api_key;
  //URL for src in <iframe> Google Map
  mapUrl: string | undefined;

  //Detail information of this House
  houseDetail: House | undefined;
  //Detail image of this House
  houseImage: string[] = [];
  //Landlord of this house
  landlordDetail: User | undefined;

  //List of available rooms
  availableRooms: Room[] = [];

  //For phone number
  phoneNumDisplay = false;

  // Statistics
  totallyAvailableRoom: number = 0;
  partiallyAvailableRoom: number = 0;
  availableSlot: number = 0;

  // Rates
  rates: Rate[] = [];

  // For creating Rates
  houseId: number = 0;
  star: number = 0;
  comment: string = "";

  // Reports
  inputReportContent: string = '';

  // For dislaying Regions
  campuses: Campus[] = [];
  houseVillage: string = "Chi Quan 1";
  houseCommune: string = "Thị trấn Liên Quan";
  houseDistrict: string = "Huyện Thạch Thất";
  houseCampus: string = "FU - Hòa Lạc";

  // Alert
  @ViewChild('reportSuccessAlert') private reportSuccessAlert: SwalComponent | undefined;
  @ViewChild('reportErrorAlert') private reportErrorAlert: SwalComponent | undefined;
  @ViewChild('rateSuccessAlert') private rateSuccessAlert: SwalComponent | undefined;
  @ViewChild('rateErrorAlert') private rateErrorAlert: SwalComponent | undefined;
  @ViewChild('rateRoleErrorAlert') private rateRoleErrorAlert: SwalComponent | undefined;

  constructor(
    private route: ActivatedRoute,
    private houseService: HouseService,
    private userService: UserService,
    private roomService: RoomService,
    private reportService: ReportService,
    private rateService: RateService,
    private router: Router,
    private campusService: CampusService
  ) { }

  ngOnInit(): void {
    //Get id of House from Route
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.houseId = id;

    //Call API: Increase view of this House by 1
    this.houseService.increaseView(id).subscribe();

    //Call API: get House Detail information
    this.houseService.getHouseByHouseId(id).subscribe(data => {
      this.houseDetail = data;

      //Set URL for <iframe> Google Map
      this.mapUrl = `https://www.google.com/maps/embed/v1/place?key=${this.mapAPIkey}&q=${this.houseDetail!.address.googleMapLocation}`;

      //Call API: get this House's Landlord detail information (after get house detail info)
      this.userService.getUserByUserId(this.houseDetail?.landlordId).subscribe(data => {
        this.landlordDetail = data;
      });

      //Call API: get available rooms of this house
      this.roomService.getAvailableRooms(id).subscribe(data => {
        this.availableRooms = data;
      });

      //Get region info
      this.getRegionOfHouse();
    });

    // Statistics
    this.roomService.countTotallyAvailableRoomByHouseId(id).subscribe(data => {
      this.totallyAvailableRoom = data;
    });

    this.roomService.countPartiallyAvailableRoomByHouseId(id).subscribe(data => {
      this.partiallyAvailableRoom = data;
    });

    this.roomService.countAvailableCapacityByHouseId(id).subscribe(data => {
      this.availableSlot = data;
    });

    // Rates
    this.rateService.getListRatesByHouseId(this.houseId).subscribe(data => {
      this.rates = data;
    });

    // Get Regions from localStorage (if has)
    let campus_data = localStorage.getItem("campuses");
    if (campus_data) {
      this.campuses = JSON.parse(campus_data);
    } else {
      this.campusService.getAllCampuses().subscribe(data => {
        this.campuses = data;
        localStorage.campuses = JSON.stringify(data);
      });
    }
  }

  // get data of region of this house 
  // (based on list campuses from localStorage and ids of regions of house)
  getRegionOfHouse() {
    //Get Campus
    this.campuses.forEach((campus) => {
      if (campus.campusId == this.houseDetail?.campusId) {
        this.houseCampus = campus.campusName;

        //Get District
        campus.districts.forEach((district) => {
          if (district.districtId == this.houseDetail?.districtId) {
            this.houseDistrict = district.districtName;

            //Get Commune
            district.communes.forEach((commune) => {
              if (commune.communeId == this.houseDetail?.communeId) {
                this.houseCommune = commune.communeName;

                //Get Village
                commune.villages.forEach((village) => {
                  if (village.villageId == this.houseDetail?.villageId) {
                    this.houseVillage = village.villageName;
                  }
                });
              }
            });
          }
        });
        return;
      }
    });
  }

  //Send the Report for House
  sendReport() {
    var user = null;
    var role = null;

    // get data from localStorage
    user = localStorage.getItem("user");
    role = localStorage.getItem("role");

    if (user === null) {
      //not logged in
      this.reportErrorAlert?.fire();
      return;
    } else if (role === "Student") {
      this.inputReportContent = this.inputReportContent.trim();

      //Create Report
      const report: Report = {
        houseId: this.houseDetail!.houseId,
        reportContent: this.inputReportContent,
        statusId: 1,
        deleted: false,
        reportedDate: new Date()
      };

      this.reportService.addReport(report).subscribe(
        data => {
          if (data.status == 403) {
            //not logged in
            this.reportErrorAlert?.fire();
          } else if (data.status == 200) {
            //success
            this.reportSuccessAlert?.fire();
          }
        },
        error => {
        }
      );
    } else if (role !== "Student") {
      //wrong role
      this.rateRoleErrorAlert?.fire();
    }
  }

  // Go to RoomDetail
  viewRoom(id: number) {
    this.router.navigate(['/room-detail/' + id]);
  }

  //for displaying 'Amount of People'
  peopleCounter(i: number) {
    return new Array(i);
  }

  // For Rate
  star1() {
    this.star = 1;
  }
  star2() {
    this.star = 2;
  }
  star3() {
    this.star = 3;
  }
  star4() {
    this.star = 4;
  }
  star5() {
    this.star = 5;
  }

  // Create Rate
  addRate() {
    //Check if user has logged in
    var user = null;
    user = localStorage.getItem("user");

    if (user === null) {
      //user not logged in => Alert
      this.rateErrorAlert?.fire();
    } else {
      //Check user logged in from Server => if not => alert
      this.rateService.addRate(this.houseId, this.star, this.comment).subscribe(
        data => {
          if (data.status == 403) {
            this.rateErrorAlert?.fire();
          } else if (data.status == 200) {
            this.rateSuccessAlert?.fire();

            // Refresh list Rate
            this.rateService.getListRatesByHouseId(this.houseId).subscribe(data => {
              this.rates = data;
            });
          }
        },
        error => { }
      );
    }
  }
}
