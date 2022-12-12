import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.scss']
})
export class CreateAccountComponent implements OnInit {

  staffForm = this.formBuilder.group({
    name: ['', Validators.required],
    email: ['', Validators.required],
    role: ['', ],
    password: ['', Validators.required, Validators.minLength(8)],
  });

  roles: Array<any>;

  constructor(private formBuilder: FormBuilder, private userService: UserService) { 
    this.roles = [
      {roleId: 1, roleName: 'Staff'},
    ]
  }

  ngOnInit(): void {
    
  }

  getStaffRoles(){
    
  }

  validate(){

  }

  addAccount(){
    let data = {
      displayName: this.staffForm.controls['name'].value,
      email: this.staffForm.controls['email'].value,
      role: this.staffForm.controls['role'].value,
      password: this.staffForm.controls['password'].value
    }
    this.userService.createStaff(data).subscribe(resp => {}, error => {});
  }

}
