import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { RoleService } from 'src/app/services/role.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.scss']
})
export class CreateAccountComponent implements OnInit {
  @ViewChild('successAlert') private successAlert: SwalComponent | undefined;
  staffForm = this.formBuilder.group({
    name: [, Validators.required],
    email: [, Validators.required],
    role: [, Validators.required],
    password: [, [Validators.required, Validators.minLength(8)]],
  });

  roles: any;

  name = true;
  email = true;
  password = true;
  role = true;

  constructor(
    private formBuilder: FormBuilder, 
    private userService: UserService,
    private roleService: RoleService,
  ) { 
    roleService.getStaffRoles().subscribe(
      resp => {
        this.roles = resp;
      },
      error => { }
    );
  }

  ngOnInit(): void {
    
  }

  validate(){
    if (this.staffForm.controls['name'].errors?.['required']){
      this.name = false;
    }
    else {
      this.name = true;
    }
    if (this.staffForm.controls['email'].errors?.['required']){
      this.email = false;
    }
    else {
      this.email = true;
    }
    if (this.staffForm.controls['password'].errors?.['required']){
      this.password = false;
    }
    else {
      this.password = true;
    }
    if (this.staffForm.controls['role'].errors?.['required']){
      this.role = false;
    }
    else {
      this.role = true;
    }
  }

  addAccount(){
    this.validate();
    if (!(this.name && this.email && this.password && this.role)){
      return;
    }
    let data = {
      displayName: this.staffForm.controls['name'].value,
      email: this.staffForm.controls['email'].value,
      role: Number(this.staffForm.controls['role'].value),
      password: this.staffForm.controls['password'].value
    }
    this.userService.createStaff(data).subscribe(resp => {this.successAlert?.fire()}, error => {});
  }

}
