import { Observable } from 'rxjs';
import { User } from './../models/user';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
//environment variable for API URL
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  readonly APIUrl = `${environment.api_url}/User`;

  constructor(private http: HttpClient) { }

  //[House Detail] Get User by Id
  getUserByUserId(userId: string): Observable<User> {
    return this.http.get<User>(this.APIUrl + "/" + userId);
  }

  //[Staff][Dashboard] get list of landlords
  getLandlords(): Observable<User[]> {
    return this.http.get<User[]>(this.APIUrl + "/landlord");
  }

  //[Admin][List Staff] get list of staffs
  getStaffs(): Observable<User[]> {
    return this.http.get<User[]>(this.APIUrl + "/staff");
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

  registerStudentGoogle(googleIdToken: string){
    return this.http.post<User>(this.APIUrl + "/register",
    {
      "googleIdToken": googleIdToken,
      "roleId": 1,
    });
  }

  registerLandlordGoogle(
      googleIdToken: string,
      phonenumber: string,
      identityCardFrontSideImageLink: string,
      identityCardBackSideImageLink: string,
      facebookUrl: string
    ){
    return this.http.post<User>(
      this.APIUrl + "/register",
      {
        "googleIdToken": googleIdToken,
        "phonenumber": phonenumber,
        "identityCardFrontSideImageLink": identityCardFrontSideImageLink,
        "identityCardBackSideImageLink": identityCardBackSideImageLink,
        "facebookUrl": facebookUrl,
        "roleId": 2
      });
  }

  registerStudentFacebook(facebookId: string, name: string){
    return this.http.post<User>(this.APIUrl + "/register", {
      "facebookUserId": facebookId,
      "displayName": name,
      "roleId": 1
    });
  }

  registerLandlordFacebook(
    facebookId: string,
    name: string,
    phonenumber: string,
    identityCardFrontSideImageLink: string,
    identityCardBackSideImageLink: string,
    facebookUrl: string
  ){
  return this.http.post<User>(
    this.APIUrl + "/register",
    {
      "facebookUserId": facebookId,
      "displayName": name,
      "phonenumber": phonenumber,
      "identityCardFrontSideImageLink": identityCardFrontSideImageLink,
      "identityCardBackSideImageLink": identityCardBackSideImageLink,
      "facebookUrl": facebookUrl,
      "roleId": 2
    });
  }

  countTotalLandlords():Observable<number>{
    return this.http.get<number>(this.APIUrl + "/CountTotalLandlord");
  }
}
