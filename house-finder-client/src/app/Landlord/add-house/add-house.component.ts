import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { Campus } from 'src/app/models/campus';
import { Commune } from 'src/app/models/commune';
import { District } from 'src/app/models/district';
import { Village } from 'src/app/models/village';
import { CampusService } from 'src/app/services/campus.service';
import { FileService } from 'src/app/services/file.service';
import { HouseService } from 'src/app/services/house.service';
declare const google: any;  //For Google Map

@Component({
  selector: 'app-add-house',
  templateUrl: './add-house.component.html',
  styleUrls: ['./add-house.component.scss']
})
export class AddHouseComponent implements OnInit {
  @ViewChild('serverErrorAlert') private serverErrorAlert: SwalComponent | undefined;
  @ViewChild('addRoom') private addRoom: SwalComponent | undefined;
  map: any;
  marker: any;
  distanceService: any;
  houseName = true;
  information = true;
  campus = true;
  district = true;
  commune = true;
  village = true;
  powerPrice = true;
  waterPrice = true;
  fingerprint = true;
  camera = true;
  parking = true;
  address = true;
  googleAddress = true;
  houseImg1 = true;
  houseImg2 = true;
  houseImg3 = true;
  img1: File | any;
  img2: File | any;
  img3: File | any;
  houseId: number | undefined;

  houseForm = this.formBuilder.group({
    houseName: ['', Validators.required],
    information: [''],
    campus: ['', Validators.required],
    district: ['', Validators.required],
    commune: ['', Validators.required],
    village: ['', Validators.required],
    powerPrice: ['', Validators.required],
    waterPrice: ['', Validators.required],
    fingerprint: [false,],
    camera: [false,],
    parking: [false,],
    address: ['', Validators.required],
    googleAddress: ['-25.363,105.527064', Validators.required],
    houseImg1: [, Validators.required],
    houseImg2: [, Validators.required],
    houseImg3: [, Validators.required],
  });

  campuses: Campus[] = [];                    //(Regions) All Campuses (and Districts, Communes, Villages)
  districtsOfSelectedCampus: District[] = []; //(Regions) all Districts of 1 selected Campus (only display after user has selected 1 Campus)
  communesOfSelectedDistrict: Commune[] = []; //(Regions) all Communes of 1 selected District (only display after user has selected 1 District)
  villagesOfSelectedCommune: Village[] = [];  //(Regions) all Villages of 1 selected Commune (only display after user has selected 1 Commune)

  selectedDistrictId: number | undefined;   //(filter by Region)
  selectedCommuneId: number | undefined;    //(filter by Region)
  selectedVillageId: number | undefined;    //(filter by Region)
  selectedCampusId: number | undefined;     //(filter by campus)

  constructor(
    private formBuilder: FormBuilder,
    private houseService: HouseService,
    private fileService: FileService,
    private router: Router,
    private campusService: CampusService,) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Staff
     */
    var userRole = localStorage.getItem("role");
    if (userRole != 'Staff') {
      window.location.href = '/home';
    }

    this.initMap();
  }

  initMap() {
    //initialize google map
    this.map = new google.maps.Map(document.getElementById("google-map") as HTMLElement, {
      zoom: 15,
      center: { lat: 21.0137883027051, lng: 105.52699965513666 },
    });

    // Configure the click listener
    this.map.addListener("click", (mapsMouseEvent: any) => {
      this.marker.setPosition(mapsMouseEvent.latLng);
      this.houseForm.controls['googleAddress'].setValue(`${mapsMouseEvent.latLng.toJSON()["lat"]},${mapsMouseEvent.latLng.toJSON()["lng"]}`);
    });

    this.marker = new google.maps.Marker({
      position: { lat: 21.0137883027051, lng: 105.52699965513666 },
      map: this.map
    });
    this.distanceService = new google.maps.DistanceMatrixService();

    this.campusService.getAllCampuses().subscribe(data => {
      this.campuses = data;
    });
  }

  async addHouse() {
    //validate form
    if (this.houseForm.controls['houseName'].errors?.['required']) {
      this.houseName = false;
    }
    else {
      this.houseName = true;
    }
    if (this.houseForm.controls['information'].errors?.['required']) {
      this.information = false;
    }
    else {
      this.information = true;
    }
    if (this.houseForm.controls['campus'].errors?.['required']) {
      this.campus = false;
    }
    else {
      this.campus = true;
    }
    if (this.houseForm.controls['district'].errors?.['required']) {
      this.district = false;
    }
    else {
      this.district = true;
    }
    if (this.houseForm.controls['commune'].errors?.['required']) {
      this.commune = false;
    }
    else
      if (this.houseForm.controls['village'].errors?.['required']) {
        this.village = false;
      }
      else {
        this.village = true;
      }
    if (this.houseForm.controls['powerPrice'].errors?.['required']) {
      this.powerPrice = false;
    }
    else {
      this.powerPrice = true;
    }
    if (this.houseForm.controls['waterPrice'].errors?.['required']) {
      this.waterPrice = false;
    }
    else {
      this.waterPrice = true;
    }
    if (this.houseForm.controls['address'].errors?.['required']) {
      this.address = false;
    }
    else {
      this.address = true;
    }
    if (this.houseForm.controls['googleAddress'].errors?.['required']) {
      this.googleAddress = false;
    }
    else {
      this.googleAddress = true;
    }
    if (this.houseForm.controls['houseImg1'].errors?.['required']) {
      this.houseImg1 = false;
    }
    else {
      this.houseImg1 = true;
    }
    if (this.houseForm.controls['houseImg2'].errors?.['required']) {
      this.houseImg2 = false;
    }
    else {
      this.houseImg2 = true;
    }
    if (this.houseForm.controls['houseImg3'].errors?.['required']) {
      this.houseImg3 = false;
    }
    else {
      this.houseImg3 = true;
    }
    if (!(this.houseName && this.information && this.campus && this.district && this.commune && this.village
      && this.powerPrice && this.waterPrice && this.address && this.googleAddress && this.houseImg1
      && this.houseImg2 && this.houseImg3)) {
      return;
    }
    var distance = 0;
    const origin = { lat: 21.0137883027051, lng: 105.52699965513666 };
    const destination = {
      lat: Number(this.houseForm.controls['googleAddress'].value.split(",")[0]),
      lng: Number(this.houseForm.controls['googleAddress'].value.split(",")[1])
    };

    const request = {
      origins: [origin],
      destinations: [destination],
      travelMode: google.maps.TravelMode.DRIVING,
      unitSystem: google.maps.UnitSystem.METRIC,
    };

    try{
      await this.distanceService.getDistanceMatrix(request).then((response: any) => {
        distance = response.rows[0].elements[0].distance.value;
      })
    }
    catch (Exception){
      distance = 0
    }
    this.houseService.createHouse(
      this.houseForm.controls['houseName'].value,
      this.houseForm.controls['information'].value,
      this.houseForm.controls['address'].value,
      this.houseForm.controls['googleAddress'].value,
      Number(this.houseForm.controls['village'].value),
      Number(this.houseForm.controls['campus'].value),
      distance,
      this.houseForm.controls['powerPrice'].value,
      this.houseForm.controls['waterPrice'].value,
      this.houseForm.controls['fingerprint'].value,
      this.houseForm.controls['camera'].value,
      this.houseForm.controls['parking'].value,
    ).subscribe(resp => {
      this.houseId = resp.houseId;
      this.fileService.uploadHouseImageFile(this.img1, this.img2, this.img3, resp.houseId).subscribe(
        resp => this.addRoom?.fire(),
        err => this.serverErrorAlert?.fire()
      )
    },
      error => {
        this.serverErrorAlert?.fire();
      });
  }

  navAddRoom() {
    if (this.houseId == undefined) {
      return;
    }
    this.router.navigate(['/Landlord/add-room'], { queryParams: { houseId: this.houseId } });
  }

  logout() {
    window.location.href = "/login";
  }
  loadImage(event: any, index: number) {
    if (event.target.files && event.target.files[0]) {
      if (index == 1) {
        this.img1 = event.target.files[0];
      }
      else if (index == 2) {
        this.img2 = event.target.files[0];
      }
      else if (index == 3) {
        this.img3 = event.target.files[0];
      }
    }
  }
  onCampusSelected(selectedCampusId: string) {
    // convert string to number
    var numberCampusId: number = +selectedCampusId;

    // find the selected campus
    this.campuses.forEach((campus) => {
      // assign the list of Commune as the communes of this District
      if (campus.campusId == numberCampusId) {
        this.districtsOfSelectedCampus = campus.districts;
        return;
      }
    });

    //Reset lower Region Filter
    this.selectedDistrictId = undefined;
    this.selectedCommuneId = undefined;
    this.selectedVillageId = undefined;
    this.communesOfSelectedDistrict = [];
    this.villagesOfSelectedCommune = [];

    // Call API: update list houses with the campus user chose
    this.selectedCampusId = numberCampusId;
  } onDistrictClicked() {
    // check if user has chosen Campus
    if (!this.selectedCampusId) {
      // if not => alert that they have to choose Campus before District
      alert('Vui lòng chọn Cơ sở bạn muốn tìm trước');
    }
  }

  //[Filter by Region] Filter by Commune
  //Change list of Communes after user selected District
  onDistrictSelected(stringSelectedDistrictId: string) {
    // convert string to number
    var numberDistrictId: number = +stringSelectedDistrictId;

    // find the selected district
    this.districtsOfSelectedCampus.forEach((district) => {
      // assign the list of Commune as the communes of this District
      if (district.districtId == numberDistrictId) {
        this.communesOfSelectedDistrict = district.communes;
        return;
      }
    });

    //Reset lower Region Filter
    this.villagesOfSelectedCommune = [];

    //no need for filtering by commune & village
    this.selectedCommuneId = undefined;
    this.selectedVillageId = undefined;
    // Call API to update list houses with the selected district
    this.selectedDistrictId = numberDistrictId;

  }

  //[Filter by Region] Filter by Commune
  //Change list of Villages after user selected Commune
  onCommuneSelected(stringSelectedCommuneId: string) {
    // convert string to number
    var numberCommuneId: number = +stringSelectedCommuneId;

    // find the selected commune
    this.communesOfSelectedDistrict.forEach((commune) => {
      // assign the list of Villages as the villages of this Commune
      if (commune.communeId == numberCommuneId) {
        this.villagesOfSelectedCommune = commune.villages;
        return;
      }
    });

    //no need for filtering by district & village
    this.selectedDistrictId = undefined;
    this.selectedVillageId = undefined;
    // Call API to update list houses with the selected commune
    this.selectedCommuneId = numberCommuneId;

  }

  //[Filter by Region] Filter by Village
  onVillageSelected(stringSelectedVillageId: string) {
    // convert string to number
    var numberVillageId: number = +stringSelectedVillageId;

    //no need for filtering by district & commune
    this.selectedDistrictId = undefined;
    this.selectedCommuneId = undefined;
    // Call API to update list houses with the selected village
    this.selectedVillageId = numberVillageId;

  }

}
