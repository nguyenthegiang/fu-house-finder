import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { RoleService } from 'src/app/services/role.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-update-account',
  templateUrl: './update-account.component.html',
  styleUrls: ['./update-account.component.scss']
})
export class UpdateAccountComponent implements OnInit {
  staffForm = this.formBuilder.group({
    name: [, Validators.required],
    email: [, Validators.required],
    role: [, Validators.required],
  });
  constructor(
    private formBuilder: FormBuilder, 
    private userService: UserService,
    private roleService: RoleService,) { }

  ngOnInit(): void {
  }

}
