import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { FileService } from 'src/app/services/file.service';
import { RoomService } from 'src/app/services/room.service';
import Swal from 'sweetalert2';

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
    var err: Array<string> = [];
    if (!this.image1.type.includes('image')) {
      err.push(this.image1.name);
    }
    else {
      this.fileService.uploadRoomImageFileWithRoomId(this.image1, roomId).subscribe(resp => {}, error => {});
    
    }
    if (!this.image2.type.includes('image')) {
      err.push(this.image2.name);
    }
    else {
      this.fileService.uploadRoomImageFileWithRoomId(this.image2, roomId).subscribe(resp => {}, error => {});
    }
    if (!this.image3.type.includes('image')) {
      err.push(this.image3.name);
    }
    else {
      this.fileService.uploadRoomImageFileWithRoomId(this.image3, roomId).subscribe(resp => {}, error => {});
    }
    if (err.length > 0){
      this.toast(true, 'error', true, 'Định dạng file ' + err.join(", ") +' không phải ảnh!')
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
}
