import { HouseService } from 'src/app/services/house.service';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { House } from 'src/app/models/house';
import { Campus } from 'src/app/models/campus';
import { CampusService } from 'src/app/services/campus.service';
import { Commune } from 'src/app/models/commune';
import { Village } from 'src/app/models/village';
import { District } from 'src/app/models/district';
import { ImagesOfHouse } from 'src/app/models/imagesOfHouse';

@Component({
  selector: 'app-update-house',
  templateUrl: './update-house.component.html',
  styleUrls: ['./update-house.component.scss']
})
export class UpdateHouseComponent implements OnInit {
  //Detail information of this House
  houseDetail: House | undefined;

  campuses: Campus[] = [];                    //(Regions) All Campuses (and Districts, Communes, Villages)
  districtsOfSelectedCampus: District[] = []; //(Regions) all Districts of 1 selected Campus (only display after user has selected 1 Campus)
  communesOfSelectedDistrict: Commune[] = []; //(Regions) all Communes of 1 selected District (only display after user has selected 1 District)
  villagesOfSelectedCommune: Village[] = [];  //(Regions) all Villages of 1 selected Commune (only display after user has selected 1 Commune)

  selectedCampusId: number | undefined;     //(filter by campus)
  selectedDistrictId: number | undefined;   //(filter by Region)
  selectedCommuneId: number | undefined;    //(filter by Region)
  selectedVillageId: number | undefined;    //(filter by Region)

  fileToUpload: File | null = null;
  fileIndex: number = 0;
  listImage: ImagesOfHouse[] | undefined;
  imageLink: string = '';

  constructor(private houseService: HouseService,
    private campusService: CampusService,
    private route: ActivatedRoute,
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

    this.houseService.getHouseByHouseId(id).subscribe(data => {
      this.houseDetail = data;
      this.listImage = data.imagesOfHouses;

      //(Filter) Get all Campuses (with their Districts, Communes, Villages)
      this.campusService.getAllCampuses().subscribe(data => {
        this.campuses = data;

        // find the campus
        this.campuses.forEach((campus) => {
          // assign the list of Commune as the communes of this District
          if (campus.campusId == this.houseDetail?.campusId) {
            this.districtsOfSelectedCampus = campus.districts;

            // find the district
            this.districtsOfSelectedCampus.forEach((district) => {
              console.log(this.houseDetail?.districtId);
              // assign the list of Commune as the communes of this District
              if (district.districtId == this.houseDetail?.districtId) {
                this.communesOfSelectedDistrict = district.communes;

                // find the selected commune
                this.communesOfSelectedDistrict.forEach((commune) => {
                  // assign the list of Villages as the villages of this Commune
                  if (commune.communeId == this.houseDetail?.communeId) {
                    this.villagesOfSelectedCommune = commune.villages;
                  }
                });
              }
            });
          }
        });
      });
    });
  }

  //[Filter] Filter by Campus
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

    // Call API: update list houses with the campus user chose
    this.selectedCampusId = numberCampusId;
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

  getImageId(index: number) {
    this.fileIndex = index;

    if (this.houseDetail?.imagesOfHouses) {
      this.imageLink = this.houseDetail.imagesOfHouses[this.fileIndex].imageLink;
    }
  }

  onFilechange(event: any) {
    //console.log(event.target.files[0].name);
    this.fileToUpload = event.target.files[0];

    const reader = new FileReader();

    
    reader.onload = (e) => {
      if (this.listImage) {
        this.listImage[this.fileIndex].imageLink = reader.result!.toString();
      }
    }
    reader.readAsDataURL(event.target.files[0]);
    
  }

  cancelChange(index: number) {
    if (this.listImage && this.houseDetail?.imagesOfHouses) {
      this.listImage[index].imageLink = this.imageLink;
    }
  }

  goBack(): void {
    window.location.reload();
  }

  logout() {
    window.location.href = "/login";
  }
}
