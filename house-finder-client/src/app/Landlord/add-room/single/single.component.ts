import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { FileService } from 'src/app/services/file.service';
import { RoomService } from 'src/app/services/room.service';

@Component({
  selector: 'app-single',
  templateUrl: './single.component.html',
  styleUrls: ['./single.component.scss']
})
export class SingleComponent implements OnInit {

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
    image3: [, Validators.required]
  });

  image1: File | any;
  image2: File | any;
  image3: File | any;

  building = true;
  floor = true;
  roomName = true;
  price = true;
  area = true;
  maxPeople = true;
  currentPeople = true;
  image1Validate = true;
  image2Validate = true;
  image3Validate = true;

  constructor(
    private roomService: RoomService, 
    private fileService: FileService,
    private formBuilder: FormBuilder,
  ){ 
    
  }

  ngOnInit(): void {
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
    if (this.roomForm.controls['image1'].errors?.['required']){
      this.image1Validate = false
    }
    else {
      this.image1Validate = true;
    }
    if (this.roomForm.controls['image2'].errors?.['required']){
      this.image2Validate = false;
    }
    else {
      this.image2Validate = true;
    }
    if (this.roomForm.controls['image3'].errors?.['required']){
      this.image3Validate = false;
    }
    else {
      this.image3Validate = true;
    }
  }

  onImage1Change(event: any){
    this.image1 = event.target.files[0];
  }
  onImage2Change(event: any){
    this.image2 = event.target.files[0];
  }
  onImage3Change(event: any){
    this.image3 = event.target.files[0];
  }

  addRoom(HouseId: Number){
    this.validate();
    if (!(this.building && this.floor && this.roomName && this.price 
      && this.area && this.maxPeople && this.currentPeople
      && this.image1Validate && this.image2Validate && this.image3Validate)) {
        return;
      }
    
    let data = {
      buildingNumber: this.roomForm.controls['building'].value,
      floorNumber: this.roomForm.controls['building'].value,
      roomName: this.roomForm.controls['building'].value,
      pricePerMonth: this.roomForm.controls['building'].value,
      information: this.roomForm.controls['building'].value,
      areaByMeters: this.roomForm.controls['building'].value,
      maxAmountOfPeople: this.roomForm.controls['building'].value,
      currentAmountOfPeople: this.roomForm.controls['building'].value,
      roomTypeId: this.roomForm.controls['building'].value,
      fridge: this.roomForm.controls['building'].value,
      kitchen: this.roomForm.controls['building'].value,
      washingMachine: this.roomForm.controls['building'].value,
      desk: this.roomForm.controls['building'].value,
      bed: this.roomForm.controls['building'].value,
      noLiveWithHost: this.roomForm.controls['building'].value,
      closedToilet: this.roomForm.controls['building'].value,
      houseId: HouseId
    }
    this.roomService.createRoom(data).subscribe(
      resp => {
        this.submitImage(resp.roomId);
      },
      error => {}
    )
  }

  submitImage(roomId: Number){
    this.fileService.uploadRoomImageFileWithRoomId(this.image1, roomId).subscribe(resp => {}, error => {});
    this.fileService.uploadRoomImageFileWithRoomId(this.image2, roomId).subscribe(resp => {}, error => {});
    this.fileService.uploadRoomImageFileWithRoomId(this.image3, roomId).subscribe(resp => {}, error => {});
  }
}
