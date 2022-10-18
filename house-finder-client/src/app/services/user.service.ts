import { Observable } from 'rxjs';
import { User } from './../models/user';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  readonly APIUrl = "https://localhost:5001/api/User";

  constructor(private http: HttpClient) { }

  //[House Detail] Get User by Id
  getUserByUserId(userId: string): Observable<User> {
    return this.http.get<User>(this.APIUrl + "/" + userId);
  }

  loginFacebook(facebookUserId: string): Observable<User> {
    return this.http.post<User>(this.APIUrl + "/login", {"facebookUserId": facebookUserId});
  }

  loginGoogle(googleUserId: string): Observable<User>{
    console.log("api called");
    return this.http.post<User>(this.APIUrl + "/login", {"googleUserId": googleUserId});
  }

  loginEmailPassword(email: string, password: string): Observable<User>{
    return this.http.post<User>(this.APIUrl + "/login", {"email": email, "password": password});
  }
}
