import { Component, Inject, NgZone, OnInit } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';


@Component({
  selector: 'app-role-modal',
  templateUrl: './role-modal.component.html',
  styleUrls: ['./role-modal.component.scss']
})
export class RoleModalComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<RoleModalComponent>,
    private ngZone: NgZone
    ) { }

  ngOnInit(): void {
  }

  studentChoose(){
    this.ngZone.run(() => {
      this.dialogRef.close('student');
    });
  }

  landlordChoose(){
    this.ngZone.run(() => {
      this.dialogRef.close('landlord');
    });
  }

}
