import { Component, ElementRef, NgZone, OnInit, Renderer2, ViewChild } from '@angular/core';
import { SocialAuthService, SocialUser } from "angularx-social-login";
import { FacebookLoginProvider, GoogleLoginProvider } from "angularx-social-login";
import { User } from '../models/user';
import { UserService } from '../services/user.service';
import { CredentialResponse } from 'google-one-tap';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  @ViewChild('ggDiv') ggDiv: ElementRef | undefined;
  @ViewChild('registerModal') registerModal: ElementRef | undefined;
  @ViewChild('roleModal') roleModal: ElementRef | undefined;

  socialUser: SocialUser | undefined;
  user: User | undefined;
  googleIdToken: string | any;
  registerForm = this.formBuilder.group({
    phonenumber: "", 
    identityCardFrontSideImageLink: "", 
    identityCardBackSideImageLink: "", 
    facebookUrl: ""
  });

  constructor(
    private authService: SocialAuthService, 
    private userService: UserService,
    private elementRef: ElementRef,
    private router: Router,
    private ngZone: NgZone,
    private formBuilder: FormBuilder,
    private renderer: Renderer2
    ) { }

  ngOnInit(): void {
    var s = document.createElement("script");
    s.src = "https://accounts.google.com/gsi/client";
    s.async = true;
    s.defer = true;
    this.elementRef.nativeElement.appendChild(s);
    (window as any).onGoogleLibraryLoad = () => {
      console.log('Google\'s One-tap sign in script loaded!');
    
      // @ts-ignore
      google.accounts.id.initialize({
        // Ref: https://developers.google.com/identity/gsi/web/reference/js-reference#IdConfiguration
        client_id: '919349682446-etrauq4d5cluclesaifkcr4bnh4gru2j.apps.googleusercontent.com',
        callback: this.handleCredentialResponse.bind(this), // Whatever function you want to trigger...
        auto_select: false,
        cancel_on_tap_outside: true,
      });

      // @ts-ignore
      google.accounts.id.renderButton(
        parent=this.ggDiv?.nativeElement,
        {
          type: 'standard',
          theme: 'outline',
          size: 'large',
          text: 'signin_with',
          shape: 'rectangular',
          logo_alignment: 'left'
        }
      );
    };
    
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
        response => {
          this.ngZone.run(()=>{this.router.navigate(['/home']);});
        },
        error => {
          this.googleIdToken = response?.credential;
          this.triggerRoleModal();
        }
      );

    }
    
  signInWithFB(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID);
    this.authService.authState.subscribe((user) => {
      this.socialUser = user;
      console.log(user);
      this.userService.loginFacebook(user.id).subscribe(data => {
        this.user = data;
      });
    });
  }

  signOut(): void {
    this.authService.signOut();
  }

  refreshToken(): void {
    this.authService.refreshAuthToken(GoogleLoginProvider.PROVIDER_ID);
  }

  onSubmit(): void {
    this.registerLandlord(
      this.registerForm.controls['phonenumber'].value,
      this.registerForm.controls['identityCardFrontSideImageLink'].value,
      this.registerForm.controls['identityCardBackSideImageLink'].value,
      this.registerForm.controls['facebookUrl'].value
    );
  }


  registerStudent(): void {
    this.userService.registerStudentGoogle(this.googleIdToken).subscribe(data => {
      this.user = data;
      this.dismissRoleModal();
      this.router.navigate(['/home']);
    });
  }

  registerLandlord(
    phonenumber: string, 
    identityCardFrontSideImageLink: string, 
    identityCardBackSideImageLink: string, 
    facebookUrl: string
    ): void {
    this.userService.registerLandlordGoogle(this.googleIdToken, phonenumber, identityCardFrontSideImageLink, identityCardBackSideImageLink, facebookUrl).subscribe(data => {
      this.user = data;
      this.dismissRegisterModal();
      this.router.navigate(['/home']);
    });
  }
  
  triggerRegisterModal(): void {
    this.dismissRegisterModal();
    this.renderer.setStyle(this.registerModal?.nativeElement, "display", "block")
    this.registerModal?.nativeElement.classList.add('show');
  }

  triggerRoleModal(): void {
    this.renderer.setStyle(this.roleModal?.nativeElement, "display", "block");
    this.roleModal?.nativeElement.classList.add('show');
  }

  dismissRegisterModal(): void {
    this.renderer.setStyle(this.registerModal?.nativeElement, "display", "none")
    this.registerModal?.nativeElement.classList.remove('show');
  }

  dismissRoleModal(): void {
    this.renderer.setStyle(this.roleModal?.nativeElement, "display", "none");
    this.roleModal?.nativeElement.classList.remove('show');
  }
}
