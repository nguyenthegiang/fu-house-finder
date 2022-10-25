import { Component, ElementRef, NgZone, OnInit, ViewChild } from '@angular/core';
import { SocialAuthService, SocialUser } from "angularx-social-login";
import { FacebookLoginProvider, GoogleLoginProvider } from "angularx-social-login";
import { User } from '../models/user';
import { UserService } from '../services/user.service';
import { CredentialResponse } from 'google-one-tap';
import { Router } from '@angular/router';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  @ViewChild('ggDiv') ggDiv: ElementRef | undefined;
  @ViewChild('registerModal') registerModal: any;
  @ViewChild('roleModal') roleModal: any;

  socialUser: SocialUser | undefined;
  user: User | undefined;
  constructor(
    private authService: SocialAuthService, 
    private userService: UserService,
    private elementRef: ElementRef,
    private router: Router,
    private ngZone: NgZone
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
    console.log(response?.credential);
    // Decoding  JWT token...
      let decodedToken: any | null = null;
      try {
        decodedToken = JSON.parse(atob(response?.credential.split('.')[1]));
        console.log(decodedToken.sub);
      } catch (e) {
        console.error('Error while trying to decode token', e);
      }
      let user = this.userService.loginGoogle(response?.credential).subscribe(
        response => {
          this.ngZone.run(()=>{this.router.navigate(['/home']);});
        },
        error => {
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

  registerStudent(): void {

  }

  registerLandlord(): void {

  }
  
  triggerRegisterModal(): void {
    this.registerModal.open();
  }

  triggerRoleModal(): void {
    this.roleModal.open();
  }
}
