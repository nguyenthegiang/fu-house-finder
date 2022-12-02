import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { MultipleComponent } from './multiple/multiple.component';
import { SingleComponent } from './single/single.component';
declare const google: any;  //For Google Map

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.scss']
})
export class AddRoomComponent implements OnInit {
  @ViewChild(MultipleComponent) childMultiple: MultipleComponent | any;
  @ViewChild(SingleComponent) childSingle: SingleComponent | any;
  selected: string = "single";
  map: any;
  marker: any;
  houseForm = this.formBuilder.group({
    houseName: ['', Validators.required],
    information: [''],
    campus: ['', Validators.required],
    village: ['', Validators.required],
    powerPrice: ['', Validators.required],
    waterPrice: ['', Validators.required],
    fingerprint: [false],
    camera: [false],
    parking: [false],
    address: ['', Validators.required],
    googleAddress: ['', Validators.required],
  });

  constructor(
    private elementRef: ElementRef,
    private formBuilder: FormBuilder,) { }

  ngOnInit(): void {
    //this.initMap();
    //Set URL for <iframe> Google Map
    //this.mapUrl = `https://www.google.com/maps/embed/v1/view?key=${environment.google_maps_api_key}&center=21.01325,105.527064&zoom=15`;
  }

  updateForm(tab: string){
    this.selected = tab;
    console.log(this.selected);
  }

  async submitForm(){
    if (this.selected === "single"){

    }
    else if (this.selected === "multiple"){
      this.childMultiple.uploadDataFile();
      this.childMultiple.uploadImageFiles();
    }
  }

  submitHouse(){

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
    });

    this.marker = new google.maps.Marker({
      position: { lat: -25.363, lng: 105.527064 },
      map: this.map
    });
  }
}
