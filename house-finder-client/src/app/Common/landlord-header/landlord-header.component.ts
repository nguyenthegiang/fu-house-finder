import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-landlord-header',
  templateUrl: './landlord-header.component.html',
  styleUrls: ['./landlord-header.component.scss']
})
export class LandlordHeaderComponent implements OnInit {
  username: string = "";
  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.username = localStorage.getItem('user') || '{}';
  }

  backHome() {
    //use reload() so that when user is already in home -> they will get a reload of page
    window.location.href = "/Landlord/dashboard";
  }

  logout(){
    this.userService.logout().subscribe(resp => {
      localStorage.clear();
      sessionStorage.clear();
      window.location.href = "/login";
    });
  }
}
