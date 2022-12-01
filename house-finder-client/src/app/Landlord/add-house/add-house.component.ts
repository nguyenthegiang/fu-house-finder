import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
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
    fingerprint: [false, ],
    camera: [false, ],
    parking: [false, ],
    address: ['', Validators.required],
    googleAddress: ['-25.363,105.527064', Validators.required],
    houseImg1: [, Validators.required],
    houseImg2: [, Validators.required],
    houseImg3: [, Validators.required],
  }); 

  constructor(
    private formBuilder: FormBuilder, 
    private houseService: HouseService,
    private fileService: FileService,
    private router: Router,) { }

  ngOnInit(): void {
    this.initMap();
  }

  initMap(){
    //initialize google map
    this.map = new google.maps.Map(document.getElementById("google-map") as HTMLElement, {
      zoom: 15,
      center: { lat: -25.363, lng: 105.527064 },
    });
  
    // Configure the click listener
    this.map.addListener("click", (mapsMouseEvent: any) => {
      this.marker.setPosition(mapsMouseEvent.latLng);
      this.houseForm.controls['googleAddress'].setValue(`${mapsMouseEvent.latLng.lat},${mapsMouseEvent.latLng.lng}`)
    });
  
    this.marker = new google.maps.Marker({
      position: { lat: -25.363, lng: 105.527064 },
      map: this.map
    });
    this.distanceService = new google.maps.DistanceMatrixService();
  }

  async addHouse(){
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
    else  {
      this.campus = true;
    }
    if (this.houseForm.controls['district'].errors?.['required']) {
      this.district = false;
    }
    else  {
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
      && this.houseImg2 && this.houseImg3)){
        return;
      }

    var distance = 0;
    const origin = { lat: -25.363, lng: 105.527064 };
    const destination = { lat: 50.087, lng: 14.421 };
  
    const request = {
      origins: [origin],
      destinations: [destination],
      travelMode: google.maps.TravelMode.DRIVING,
      unitSystem: google.maps.UnitSystem.METRIC,
    };

    // await this.distanceService.getDistanceMatrix(request).then((response: any) => {
    //   distance = response.rows[0].elements[0].distance.value;
    // })
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

  navAddRoom(){
    if (this.houseId == undefined){
      return;
    }
    this.router.navigate(['/Landlord/add-room', {queryParams: {houseId: this.houseId}}]);
  }

  logout()
  {
    window.location.href = "/login";
  }
  loadImage(event: any, index: number){
    if (event.target.files && event.target.files[0]) {
      if (index == 1){
        this.img1 = event.target.files[0];
      }
      else if (index == 2){
        this.img2 = event.target.files[0];
      }
      else if (index == 3){
        this.img3 = event.target.files[0];
      }
    }
  }
}
