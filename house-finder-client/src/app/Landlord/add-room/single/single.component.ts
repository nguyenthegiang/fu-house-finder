import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
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

  constructor(
    private roomService: RoomService, 
    private formBuilder: FormBuilder,
  ){ 
    
  }



  ngOnInit(): void {
  }

  addRoom(){

  }
}
