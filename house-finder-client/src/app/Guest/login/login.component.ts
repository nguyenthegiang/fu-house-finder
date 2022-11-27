import { Component, ElementRef, NgZone, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { UserService } from '../../services/user.service';
import { CredentialResponse } from 'google-one-tap';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { environment } from 'src/environments/environment';
import { MatDialog } from '@angular/material/dialog';
import { RoleModalComponent } from './role-modal/role-modal.component';
import { FileService } from 'src/app/services/file.service';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  @ViewChild('ggDiv') ggDiv: ElementRef | undefined;
  @ViewChild('registerModal') registerModal: ElementRef | undefined;
  @ViewChild('roleModal') roleModal: ElementRef | undefined;
  @ViewChild('registerTemplate') registerTemplate: TemplateRef<any> | any;
  @ViewChild('loginTemplate') loginTemplate: TemplateRef<any> | any;
  @ViewChild('landLordAccountLockedAlert') private landLordAccountLockedAlert: SwalComponent | undefined;
  @ViewChild('serverErrorAlert') private serverErrorAlert: SwalComponent | undefined;
  @ViewChild('invalidEmailPasswordAlert') private invalidEmailPasswordAlert: SwalComponent | undefined;

  login = true;
  frontImgSrc = '';
  backImgSrc = '';

  user: any;
  googleIdToken: string | undefined;
  facebookId: string | undefined;
  name: string | undefined;
  role: string | undefined;
  phonenumber: string | undefined;
  facebookUrl: string | undefined;
  identityCardFrontSideImageLink: string | undefined;
  identityCardBackSideImageLink: string | undefined;
  frontImg: any;
  backImg: any;

  contactForm = this.formBuilder.group({
    phonenumber: ['', Validators.required],
    facebookUrl: ['', Validators.required],
  });
  imageForm = this.formBuilder.group({
    identityCardFrontSideImageLink: [, Validators.required],
    identityCardBackSideImageLink: [, Validators.required]
  })
  loginForm = this.formBuilder.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor(
    private userService: UserService,
    private elementRef: ElementRef,
    private router: Router,
    private ngZone: NgZone,
    private formBuilder: FormBuilder,
    public dialog: MatDialog,
    private fileService: FileService,
  ) {
  }

  ngOnInit(): void {
    var gg_s = document.createElement("script");
    gg_s.src = "https://accounts.google.com/gsi/client";
    gg_s.async = true;
    gg_s.defer = true;
    this.elementRef.nativeElement.appendChild(gg_s);

    var fb_s = document.createElement("script");
    fb_s.src = "https://connect.facebook.net/en_US/sdk.js";
    fb_s.async = true;
    fb_s.defer = true;
    fb_s.crossOrigin = 'anonymous';
    this.elementRef.nativeElement.appendChild(fb_s);

    (window as any).onGoogleLibraryLoad = () => {

      // @ts-ignore
      google.accounts.id.initialize({
        // Ref: https://developers.google.com/identity/gsi/web/reference/js-reference#IdConfiguration
        client_id: environment.gg_client,
        callback: this.handleCredentialResponse.bind(this), // Whatever function you want to trigger...
        auto_select: false,
        cancel_on_tap_outside: true,
      });

      // @ts-ignore
      google.accounts.id.renderButton(
        parent = this.ggDiv?.nativeElement,
        {
          type: 'standard',
          theme: 'outline',
          size: 'large',
          text: 'signin_with',
          shape: 'pill',
          logo_alignment: 'left',
          width: 320
        }
      );
    };

    (window as any).fbAsyncInit = function () {
      FB.init({
        appId: environment.fb_app_id,
        cookie: true,                     // Enable cookies to allow the server to access the session.
        xfbml: true,                     // Parse social plugins on this webpage.
        version: 'v15.0',                  // Use this Graph API version for this call.
      });

      FB.Event.subscribe(
        'auth.statusChange',
        signInWithFB
      );
    }

    const signInWithFB = () => {
      console.log('login fb called');
      FB.getLoginStatus((resp) => {   // Called after the JS SDK has been initialized.
        if (resp.status === 'connected') {  // The current login status of the person.
          // Logged into your webpage and Facebook.
          FB.api('/me', (response: any) => {
            this.userService.loginFacebook(response.id).subscribe(async resp => {
              if (resp.status == 200){
                localStorage.setItem('user', resp.user.displayName);
                localStorage.setItem("role", resp.user.roleName);
                this.ngZone.run(() => { this.router.navigate(['/home']); });
              }
              else if (resp.status == 403){
                this.landLordAccountLockedAlert?.fire();
              }
              else if (resp.status == 404){
                this.facebookId = response.id;
                this.name = response.name;
                await this.triggerRoleModal();
                if (this.role == 'landlord') {
                  this.ngZone.run(() => { this.triggerRegister() });
                }
                else if (this.role == 'student'){
                  this.ngZone.run(() => { this.registerStudent() });
                }
              }
              else if (resp.status == 500){
                this.serverErrorAlert?.fire();
              }
            });
          });
        } else {                                 // Not logged into your webpage or we are unable to tell.
        
        }
      });
    }

  }

  handleCredentialResponse(response: CredentialResponse) {
    // Decoding  JWT token...
    let decodedToken: any | null = null;
    try {
      decodedToken = JSON.parse(atob(response?.credential.split('.')[1]));
    } catch (e) {
      console.error('Error while trying to decode token', e);
    }
    let user = this.userService.loginGoogle(response?.credential).subscribe(
      async resp => {
        if (resp.status == 200){
          localStorage.setItem('user', resp.user.displayName);
          localStorage.setItem("role", resp.user.roleName);
          this.ngZone.run(() => { this.router.navigate(['/home']); });
        }
        else if (resp.status == 403){
          this.landLordAccountLockedAlert?.fire();
        }
        else if (resp.status == 404){
          this.googleIdToken = response?.credential;
          await this.triggerRoleModal();
          if (this.role == 'landlord') {
            this.ngZone.run(() => { this.triggerRegister() });
          }
          else if (this.role == 'student'){
            this.ngZone.run(() => { this.registerStudent() });
          }
        }
        else if (resp.status == 500){
          this.serverErrorAlert?.fire();
        }
      }
    );
  }

  loginUsernamePassword(): void {
    this.userService.loginEmailPassword(
      this.loginForm.controls['username'].value,
      this.loginForm.controls['password'].value
    ).subscribe(
      resp => {
        if (resp.status == 200){
          localStorage.setItem('user', resp.user.displayName);
          localStorage.setItem("role", resp.user.roleName);
          this.ngZone.run(() => { this.router.navigate(['/Admin/list-staff']); });
        }
        else if (resp.status == 403){
          this.landLordAccountLockedAlert?.fire();
        }
        else if (resp.status == 404){
          this.invalidEmailPasswordAlert?.fire();
        }
        else if (resp.status == 500){
          this.serverErrorAlert?.fire();
        }
      }
    )
  }

  onSubmit(): void {
    this.registerLandlord(
      this.contactForm.controls['phonenumber'].value,
      this.contactForm.controls['facebookUrl'].value
    );
  }


  registerStudent(): void {
    if (this.googleIdToken != undefined) {
      this.userService.registerStudentGoogle(this.googleIdToken).subscribe(resp => {
        if (resp.status == 200){
          localStorage.setItem('user', resp.user.displayName);
          localStorage.setItem("role", resp.user.roleName);
          this.user = resp;
          this.router.navigate(['/home']);
        }
        else if (resp.status == 500){
          this.serverErrorAlert?.fire();
        }
      });
    }
    else if (this.facebookId != undefined && this.name != undefined) {
      this.userService.registerStudentFacebook(this.facebookId, this.name).subscribe(resp => {
        if (resp.status == 200){
          localStorage.setItem('user', resp.user.displayName);
          localStorage.setItem("role", resp.user.roleName);
          this.user = resp;
          this.router.navigate(['/home']);
        }
        else if (resp.status == 500){
          this.serverErrorAlert?.fire();
        }
      });
    }
  }

  registerLandlord(
    phonenumber: string,
    facebookUrl: string
  ): void {
    if (this.googleIdToken != undefined) {
      this.userService.registerLandlordGoogle(
        this.googleIdToken,
        phonenumber,
        facebookUrl
      ).subscribe(resp => {
        if (resp.status == 200){
          this.fileService.uploadIDC(this.frontImg, this.backImg).subscribe(resp => {});
          localStorage.setItem('user', resp.user.displayName);
          localStorage.setItem("role", resp.user.roleName);
          this.router.navigate(['/home']);
        }
        else if (resp.status == 500){
          this.serverErrorAlert?.fire();
        }
      });
    }
    else if (this.facebookId != undefined && this.name != undefined) {
      this.userService.registerLandlordFacebook(
        this.facebookId,
        this.name,
        phonenumber,
        facebookUrl
      ).subscribe(resp => {
        if (resp.status == 200){
          this.fileService.uploadIDC(this.frontImg, this.backImg).subscribe(resp => {});
          localStorage.setItem('user', resp.user.displayName);
          localStorage.setItem("role", resp.user.roleName);
          this.router.navigate(['/home']);
        }
        else if (resp.status == 500){
          this.serverErrorAlert?.fire();
        }
      });
    }
  }

  async triggerRoleModal(): Promise<Boolean> {
    const dialogRef = this.dialog.open(RoleModalComponent, {
      width: '500px',
      height: '200px'
    });
    return dialogRef.afterClosed().toPromise().then(
      result => {
        this.role = result;
        return Promise.resolve(result);
      });
  }

  triggerRegister() {
    this.login = false;
  }

  displayImage(event: any, side: string): void{
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];

      const reader = new FileReader();

      if (side == 'front'){
        reader.onload = e => this.frontImgSrc = reader.result!.toString();
        reader.readAsDataURL(file);
        this.frontImg = file;
      }
      else if (side == 'back'){
        reader.onload = e => this.backImgSrc = reader.result!.toString();
        reader.readAsDataURL(file);
        this.backImg = file;
      }
    }
  }

}

