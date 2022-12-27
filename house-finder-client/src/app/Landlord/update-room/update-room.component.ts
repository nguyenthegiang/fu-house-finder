import { Component, Input, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Room } from 'src/app/models/room';
import { RoomService } from 'src/app/services/room.service';
import { Location } from '@angular/common';
import { RoomStatus } from 'src/app/models/roomStatus';
import RoomStatusService from 'src/app/services/roomStatus.service';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import Swal from 'sweetalert2';
import { Validators, FormBuilder } from '@angular/forms';
import { FileService } from 'src/app/services/file.service';



@Component({
  selector: 'app-update-room',
  templateUrl: './update-room.component.html',
  styleUrls: ['./update-room.component.scss']
})
export class UpdateRoomComponent implements OnInit {
  
  @ViewChild('serverErrorAlert') private serverErrorAlert: SwalComponent | undefined;
  @ViewChild('addRoomAlert') private addRoomAlert: SwalComponent | undefined;
  @ViewChild('forbiddenAlert') private forbiddenAlert: SwalComponent | undefined;
  
  roomId: number = 0;
  houseId: number = 0;
  listStatus: RoomStatus[] = [];
  selectedRoomStatusId: number | undefined;

  roomForm = this.formBuilder.group({
    building: ['', Validators.required],
    floor: ['', Validators.required],
    roomName: ['', Validators.required],
    price: ['', Validators.required],
    info: ['', ],
    area: ['', Validators.required],
    maxPeople: ['', Validators.required],
    currentPeople: ['', Validators.required],
    roomType: ['', Validators.required],
    fridge: [false, ],
    kitchen: [false, ],
    washingMachine: [false, ],
    table: [false, ],
    bed: [false, ],
    withoutHost: [false, ],
    closedToilet: [false, ],
    image1: [, Validators.required],
    image2: [, Validators.required],
    image3: [, Validators.required],
    statusId: [, Validators.required],
    houseId: [,],
    deleted: [false,],
    createdDate: [,],
    createdBy: [,]
  });

  image1: File | any;
  image2: File | any;
  image3: File | any;

  image1_ID: number = 0;
  image2_ID: number = -1;
  image3_ID: number = -2;

  fileToUpload: { [imageId: string]: any} = {};

  building = true;
  floor = true;
  roomName = true;
  price = true;
  area = true;
  maxPeople = true;
  currentPeople = true;
  image1Validate = true;

  constructor(
    private roomService: RoomService, 
    private fileService: FileService,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private roomStatusService: RoomStatusService
  ){ 
    try{
      this.roomId = Number(this.route.snapshot.paramMap.get('id'));
    }
    catch {
      this.navDashboard();
    }
  }

  ngOnInit(): void {
    this.getRoomByRoomId();
  }

  validate(){
    if (this.roomForm.controls['building'].errors?.['required']){
      this.building = false;
    }
    else {
      this.building = true;
    }
    if (this.roomForm.controls['floor'].errors?.['required']){
      this.floor = false;
    }
    else {
      this.floor = true;
    }
    if (this.roomForm.controls['roomName'].errors?.['required']){
      this.roomName = false;
    }
    else {
      this.roomName = true;
    }
    if (this.roomForm.controls['price'].errors?.['required']){
      this.price = false;
    }
    else {
      this.price = true;
    }
    if (this.roomForm.controls['area'].errors?.['required']){
      this.area = false;
    }
    else {
      this.area = true;
    }
    if (this.roomForm.controls['maxPeople'].errors?.['required']){
      this.maxPeople = false;
    }
    else {
      this.maxPeople = true;
    }
    if (this.roomForm.controls['currentPeople'].errors?.['required']){
      this.currentPeople = false;
    }
    else {
      this.currentPeople = true;
    }
    var err: Array<string> = [];
    for (let key in this.fileToUpload){
      if (!this.fileToUpload[key].type.includes('image')){
        this.image1Validate = false;
        err.push(this.fileToUpload[key].name);
        break;
      }
    }

    if (err.length > 0){
      this.toast(true, 'error', true, 'Định dạng file ' + err.join(", ") +' không phải ảnh!')
    }
  }

  onImage1Change(event: any){
    const reader = new FileReader();
    
    reader.onload = (e) => {
      this.roomForm.controls['image1'].setValue(reader.result!.toString());
      this.fileToUpload[`${this.image1_ID}`] = event.target.files[0];
    }
    reader.readAsDataURL(event.target.files[0]);
  }

  onImage2Change(event: any){
    const reader = new FileReader();
    
    reader.onload = (e) => {
      this.roomForm.controls['image2'].setValue(reader.result!.toString());
      this.fileToUpload[`${this.image2_ID}`] = event.target.files[0];
    }
    reader.readAsDataURL(event.target.files[0]);
  }

  onImage3Change(event: any){
    const reader = new FileReader();
    
    reader.onload = (e) => {
      this.roomForm.controls['image3'].setValue(reader.result!.toString());
      this.fileToUpload[`${this.image3_ID}`] = event.target.files[0];
    }
    reader.readAsDataURL(event.target.files[0]);
  }

  addRoom(){
    this.validate();
    if (!(this.building && this.floor && this.roomName && this.price 
      && this.area && this.maxPeople && this.currentPeople
      && this.image1Validate)) {
        return;
      }
    let data = {
      buildingNumber: this.roomForm.controls['building'].value,
      floorNumber: this.roomForm.controls['floor'].value,
      roomName: this.roomForm.controls['roomName'].value,
      pricePerMonth: this.roomForm.controls['price'].value,
      information: this.roomForm.controls['info'].value,
      areaByMeters: this.roomForm.controls['area'].value,
      maxAmountOfPeople: this.roomForm.controls['maxPeople'].value,
      currentAmountOfPeople: this.roomForm.controls['currentPeople'].value,
      roomTypeId: this.roomForm.controls['roomType'].value,
      fridge: this.roomForm.controls['fridge'].value,
      kitchen: this.roomForm.controls['kitchen'].value,
      washingMachine: this.roomForm.controls['washingMachine'].value,
      desk: this.roomForm.controls['table'].value,
      bed: this.roomForm.controls['bed'].value,
      noLiveWithHost: this.roomForm.controls['withoutHost'].value,
      closedToilet: this.roomForm.controls['closedToilet'].value,
      roomId: this.roomId,
      houseId: this.houseId,
      statusId: this.roomForm.controls['statusId'].value,
      deleted: this.roomForm.controls['deleted'].value,
      createdDate: this.roomForm.controls['createdDate'].value,
      createdBy: this.roomForm.controls['createdBy'].value,
    }
    this.roomService.updateRoom(data).subscribe(
      resp => {
        this.submitImage(this.roomId);
        this.addRoomAlert?.fire();
      },
      error => {
        if (error.status == 401 || error.status == 403) {
          this.forbiddenAlert?.fire();
        }
        else {
          this.serverErrorAlert?.fire();
        }
      }
    )
  }

  submitImage(roomId: Number){
    for (let key in this.fileToUpload){
      this.fileService.updateRoomImageFile(this.fileToUpload[key], roomId, Number(key)).subscribe(
        resp => {},
        err => {}
      );
    }
  }
  async toast(toast: boolean = false, typeIcon: any = 'error', 
      timerProgressBar: boolean = true, title: any = '', 
      text: any = '', position: any = 'top-end',
      confirmButton: boolean = true) {
    await Swal.fire({
      toast: toast,
      position: position,
      showConfirmButton: confirmButton,
      icon: typeIcon,
      timerProgressBar,
      timer: 3000,
      title: title,
      text: text
    })
  }
  navDashboard() {
    this.router.navigate(['/Landlord/landlord-house-detail/' + this.houseId]);
  }
  getRoomByRoomId() {
    //Not calling API on first time run
    if (this.roomId == 0) {
      return;
    }
    else {
      this.roomStatusService.getAllStatus().subscribe(data => {
        this.listStatus = data;
      });
      this.roomService.getRoomByRoomId(this.roomId).subscribe(data => {
        this.roomForm.controls["building"].setValue(data.buildingNumber);
        this.roomForm.controls["floor"].setValue(data.floorNumber);
        this.roomForm.controls["roomName"].setValue(data.roomName);
        this.roomForm.controls["roomType"].setValue(data.roomType.roomTypeId);
        this.roomForm.controls["price"].setValue(data.pricePerMonth);
        this.roomForm.controls["area"].setValue(data.areaByMeters);
        this.roomForm.controls["maxPeople"].setValue(data.maxAmountOfPeople);
        this.roomForm.controls["currentPeople"].setValue(data.currentAmountOfPeople);
        this.roomForm.controls["fridge"].setValue(data.fridge);
        this.roomForm.controls["kitchen"].setValue(data.kitchen);
        this.roomForm.controls["washingMachine"].setValue(data.washingMachine);
        this.roomForm.controls["table"].setValue(data.desk);
        this.roomForm.controls["bed"].setValue(data.bed);
        this.roomForm.controls["withoutHost"].setValue(data.noLiveWithHost);
        this.roomForm.controls["closedToilet"].setValue(data.closedToilet);
        this.roomForm.controls["info"].setValue(data.information);
        this.roomForm.controls["statusId"].setValue(data.statusId);
        this.roomForm.controls["houseId"].setValue(data.houseId);
        this.houseId = data.houseId;
        this.roomForm.controls["deleted"].setValue(data.deleted);
        this.roomForm.controls["createdDate"].setValue(data.createdDate);
        this.roomForm.controls["createdBy"].setValue(data.createdBy);
        try {
          this.roomForm.controls["image1"].setValue(data.imagesOfRooms[0].imageLink);
          this.image1_ID = data.imagesOfRooms[0].imageId;
        }
        catch {
          this.image1_ID = 0;
        }

        try {
          this.roomForm.controls["image2"].setValue(data.imagesOfRooms[1].imageLink);
          this.image2_ID = data.imagesOfRooms[1].imageId;
        }
        catch {
          this.image2_ID = -1;
        }

        try {
          this.roomForm.controls["image3"].setValue(data.imagesOfRooms[2].imageLink);
          this.image3_ID = data.imagesOfRooms[2].imageId;
        }
        catch {
          this.image3_ID = -2;
        }
      });
    }
  }
}
