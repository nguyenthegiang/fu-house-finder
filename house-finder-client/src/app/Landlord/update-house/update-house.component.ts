import { HouseService } from 'src/app/services/house.service';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { House } from 'src/app/models/house';
import { Campus } from 'src/app/models/campus';
import { CampusService } from 'src/app/services/campus.service';
import { Commune } from 'src/app/models/commune';
import { Village } from 'src/app/models/village';
import { District } from 'src/app/models/district';
import { ImagesOfHouse } from 'src/app/models/imagesOfHouse';
import { FileService } from 'src/app/services/file.service';
import { FormBuilder, Validators } from '@angular/forms';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-update-house',
  templateUrl: './update-house.component.html',
  styleUrls: ['./update-house.component.scss']
})
export class UpdateHouseComponent implements OnInit {
  @ViewChild('successAlert') private successAlert: SwalComponent | undefined;
  @ViewChild('serverErrorAlert') private serverErrorAlert: SwalComponent | undefined;
  @ViewChild('forbiddenAlert') private forbiddenAlert: SwalComponent | undefined;
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

  fileToUpload: { [imageId: string]: any} = {};
  fileIndex: number = 0;
  listImage: ImagesOfHouse[] | undefined;
  imageLink: string = '';
  imageChangedId: Number[] = []; 

  houseForm = this.formBuilder.group({
    houseName: ['', Validators.required],
    campus: [, Validators.required],
    district: [, Validators.required],
    commune: [, Validators.required],
    village: [, Validators.required],
    address: ['', Validators.required],
    powerPrice: [, Validators.required],
    waterPrice: [, Validators.required],
    fingerprint: [false],
    camera: [false],
    parking: [false],
    info: [, Validators.required],
  });

  constructor(private houseService: HouseService,
    private campusService: CampusService,
    private fileService: FileService,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder) { }

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

    this.houseService.getHouseByHouseId(id).subscribe(resp => {
      this.houseDetail = resp;
      this.houseForm.controls["houseName"].setValue(resp.houseName);
      this.houseForm.controls["address"].setValue(resp.address.addresses);
      this.houseForm.controls["powerPrice"].setValue(resp.powerPrice);
      this.houseForm.controls["waterPrice"].setValue(resp.waterPrice);
      this.houseForm.controls["fingerprint"].setValue(resp.fingerprintLock);
      this.houseForm.controls["camera"].setValue(resp.camera);
      this.houseForm.controls["parking"].setValue(resp.parking);
      this.houseForm.controls["info"].setValue(resp.information);
      this.listImage = resp.imagesOfHouses;

      //(Filter) Get all Campuses (with their Districts, Communes, Villages)
      let campus_data = localStorage.getItem("campuses");
      if (campus_data) {
        this.campuses = JSON.parse(campus_data);
        this.loadVillage();
      }
      else {
        this.campusService.getAllCampuses().subscribe(data => {
          this.campuses = data;
          localStorage.campuses = JSON.stringify(data);
          this.loadVillage();
        });
      }
    });
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

    this.districtsOfSelectedCampus.forEach((district) => {
      // assign the list of Commune as the communes of this District
      if (district.districtId == this.districtsOfSelectedCampus[0].districtId) {
        this.communesOfSelectedDistrict = district.communes;
        this.houseForm.controls["district"].setValue(this.districtsOfSelectedCampus[0].districtId);

        // find the selected commune
        this.communesOfSelectedDistrict.forEach((commune) => {
          // assign the list of Villages as the villages of this Commune
          if (commune.communeId == this.communesOfSelectedDistrict[0].communeId) {
            this.villagesOfSelectedCommune = commune.villages;
            this.houseForm.controls["commune"].setValue(this.communesOfSelectedDistrict[0].communeId);
            this.houseForm.controls["village"].setValue(this.villagesOfSelectedCommune[0].villageId);
            return;
          }
        });
        return;
      }
    });

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

    this.communesOfSelectedDistrict.forEach((commune) => {
      // assign the list of Villages as the villages of this Commune
      if (commune.communeId == this.communesOfSelectedDistrict[0].communeId) {
        this.villagesOfSelectedCommune = commune.villages;
        this.houseForm.controls["commune"].setValue(this.communesOfSelectedDistrict[0].communeId);
        this.houseForm.controls["village"].setValue(this.villagesOfSelectedCommune[0].villageId);
        return;
      }
    });

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
        this.houseForm.controls["village"].setValue(this.villagesOfSelectedCommune[0].villageId);
        return;
      }
    });
  }

  loadVillage(){
    // find the campus
    this.campuses.forEach((campus) => {
      // assign the list of Commune as the communes of this District
      if (campus.campusId == this.houseDetail?.campusId) {
        this.districtsOfSelectedCampus = campus.districts;
        this.houseForm.controls["campus"].setValue(this.houseDetail?.campusId);

        this.districtsOfSelectedCampus.forEach((district) => {
          // assign the list of Commune as the communes of this District
          if (district.districtId == this.houseDetail?.districtId) {
            this.communesOfSelectedDistrict = district.communes;
            this.houseForm.controls["district"].setValue(this.houseDetail?.districtId);
    
            // find the selected commune
            this.communesOfSelectedDistrict.forEach((commune) => {
              // assign the list of Villages as the villages of this Commune
              if (commune.communeId == this.houseDetail?.communeId) {
                this.villagesOfSelectedCommune = commune.villages;
                this.houseForm.controls["commune"].setValue(this.houseDetail?.communeId);
                this.houseForm.controls["village"].setValue(this.houseDetail?.villageId);
                return;
              }
            });
            return;
          }
        });
        return;
      }
    });
  }

  getImageId(index: number) {
    this.fileIndex = index;

    if (this.houseDetail?.imagesOfHouses) {
      this.imageLink = this.houseDetail.imagesOfHouses[this.fileIndex].imageLink;
    }
  }

  onFilechange(event: any) {
    //console.log(event.target.files[0].name);

    const reader = new FileReader();
    
    reader.onload = (e) => {
      if (this.listImage) {
        let img = this.listImage[this.fileIndex];
        img.imageLink = reader.result!.toString();
        
        if (this.imageChangedId.find(i => i == img.imageId) === undefined){
          this.imageChangedId.push(img.imageId);
        }

        this.fileToUpload[`${img.imageId}`] = event.target.files[0];
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

  updateHouse() {
    let houseId = Number(this.route.snapshot.paramMap.get('id'))
    this.houseService.updateHouse(
      houseId, 
      this.houseForm.controls["houseName"].value,
      this.houseForm.controls["info"].value,
      this.houseForm.controls["address"].value,
      this.houseForm.controls["village"].value,
      this.houseForm.controls["campus"].value,
      this.houseForm.controls["powerPrice"].value,
      this.houseForm.controls["waterPrice"].value,
      this.houseForm.controls["fingerprint"].value,
      this.houseForm.controls["camera"].value,
      this.houseForm.controls["parking"].value
    ).subscribe(
      resp => {
        let count = 0;
        if (Object.keys(this.fileToUpload).length == 0) {
          this.successAlert?.fire();
        }
        for (let key in this.fileToUpload){
          this.fileService.updateHouseImageFile(this.fileToUpload[key], houseId, Number(key)).subscribe(
            resp => {
              count++;
              if (count == Object.keys(this.fileToUpload).length){
                this.successAlert?.fire();
              }
            },
            err => {
              count++;
              if (count == Object.keys(this.fileToUpload).length){
                this.serverErrorAlert?.fire();
              }
            }
          );
        }
      }, 
      err => {
        if (err.status == 401 || err.status == 403){
          this.forbiddenAlert?.fire();
        }
        else {
          this.serverErrorAlert?.fire();
        }
      }
    )
  }
  
  backDashboard(){
    this.router.navigate(["/Landlord/dashboard"]);
  }
}
