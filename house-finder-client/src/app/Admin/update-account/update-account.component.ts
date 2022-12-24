import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { RoleService } from 'src/app/services/role.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-update-account',
  templateUrl: './update-account.component.html',
  styleUrls: ['./update-account.component.scss']
})
export class UpdateAccountComponent implements OnInit {
  @ViewChild('successAlert') private successAlert: SwalComponent | undefined;
  staffForm = this.formBuilder.group({
    name: [, Validators.required],
    email: [, Validators.required],
    role: [, Validators.required],
  });
  roles: any;
  staffId: any;

  name = true;
  email = true;
  role = true;

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService,
    private roleService: RoleService,
    private route: ActivatedRoute
  ) {
    this.roleService.getStaffRoles().subscribe(
      resp => {
        this.roles = resp;
      },
      error => { }
    );
  }

  ngOnInit(): void {
    /**
     * [Authorization]
     * Role: Admin
     */
    var userRole = localStorage.getItem("role");
    if (userRole != 'Admin') {
      window.location.href = '/home';
    }

    this.route.params.subscribe((params: Params) => {
      this.staffId = params['id'];
      if (this.staffId == undefined) {
        window.location.replace('/Admin/list-staff');
      }
      else {
        this.userService.getUserByUserId(this.staffId).subscribe(
          resp => {
            this.staffForm.controls['name'].setValue(resp.displayName);
            this.staffForm.controls['email'].setValue(resp.email);
            this.staffForm.controls['role'].setValue(resp.roleId);
          }
        )
      }
    });
  }

  validate() {
    if (this.staffForm.controls['name'].errors?.['required']) {
      this.name = false;
    }
    else {
      this.name = true;
    }
    if (this.staffForm.controls['email'].errors?.['required']) {
      this.email = false;
    }
    else {
      this.email = true;
    }
    if (this.staffForm.controls['role'].errors?.['required']) {
      this.role = false;
    }
    else {
      this.role = true;
    }
  }
  updateAccount() {
    this.validate();
    if (!(this.name && this.email && this.role)) {
      return;
    }
    let data = {
      uid: this.staffId,
      displayName: this.staffForm.controls['name'].value,
      email: this.staffForm.controls['email'].value,
      role: this.staffForm.controls['role'].value
    }
    this.userService.updateStaff(data).subscribe(resp => { this.successAlert?.fire() }, error => { });
  }

}
