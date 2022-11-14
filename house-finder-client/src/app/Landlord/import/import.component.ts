import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
declare const google: any;  //For Google Map

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.scss']
})
export class ImportComponent implements OnInit, AfterViewInit {

  //Coorditate of current location of Client
  latitude: number = 0;
  longitude: number = 0;

  map: any;
  @ViewChild('mapElement') mapElement: any;

  constructor() { }

  ngOnInit(): void {
    this.getCurrentLocation();
  }

  ngAfterViewInit(): void {
    //initialize google map
    this.map = new google.maps.Map(this.mapElement.nativeElement, {
      center: { lat: this.latitude, lng: this.longitude },
      zoom: 14,
    });

    // Configure the click listener
    this.map.addListener("click", (mapsMouseEvent: any) => {
      console.log(mapsMouseEvent.latLng);
    });
  }

  //Get current location through Browser 
  getCurrentLocation() {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(position => {

        this.latitude = position.coords.latitude;
        this.longitude = position.coords.longitude;
      });
    }
    else {
      alert("Geolocation is not supported by this browser.");
    }
  }

}
