import { Component, OnInit, ViewChild } from '@angular/core';
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
  selected: string = "single";

  constructor() { }

  ngOnInit(): void {
  }

  updateForm(tab: string){
    this.selected = tab;
    console.log(this.selected);
  }

  submitForm(){
    if (this.selected === "single"){

    }
    else if (this.selected === "multiple"){
      this.childMultiple.uploadDataFile();
    }
  }

}
