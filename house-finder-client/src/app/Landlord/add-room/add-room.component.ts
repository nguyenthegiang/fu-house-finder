import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { MultipleComponent } from './multiple/multiple.component';
import { SingleComponent } from './single/single.component';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.scss']
})
export class AddRoomComponent implements OnInit {
  @ViewChild(MultipleComponent) childMultiple: MultipleComponent | any;
  @ViewChild(SingleComponent) childSingle: SingleComponent | any;
  selected: string = "multiple";
  houseId: any;

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
    private formBuilder: FormBuilder,
    private route: ActivatedRoute) { }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Landlord
     */
    var userRole = localStorage.getItem("role");
    if (userRole != 'Landlord') {
      window.location.href = '/home';
    }

    if (this.route.snapshot.queryParamMap.get('houseId') == undefined) {
      window.location.replace('/Landlord/dashboard');
    }
    this.houseId = Number(this.route.snapshot.queryParamMap.get('houseId'));
  }

  updateForm(tab: string) {
    this.selected = tab;
  }

  async submitForm() {
    if (this.selected === "single") {
      this.childSingle.addRoom(this.houseId);
    }
    else if (this.selected === "multiple") {
      await this.childMultiple.uploadDataFile(this.houseId);
      this.childMultiple.uploadImageFiles(this.houseId);
    }
  }
}
