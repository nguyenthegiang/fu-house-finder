import { Observable } from 'rxjs';
import { User } from './../models/user';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  /**
   * [Login] Login with Facebook
   */
  loginFacebook(facebookUserId: string): Observable<any> {
    return this.http.post<any>(this.APIUrl + "/login", { "facebookUserId": facebookUserId }, { withCredentials: true });
  }

  /**
   * [Login] Login with Google
   */
  loginGoogle(googleUserId: string): Observable<any> {
    return this.http.post<any>(this.APIUrl + "/login", { "googleUserId": googleUserId }, { withCredentials: true });
  }

  /**
   * [Login] Login with Email & Password
   */
  loginEmailPassword(email: string, password: string): Observable<any> {
    return this.http.post<any>(this.APIUrl + "/login", { "email": email, "password": password }, { withCredentials: true });
  }

  /**
   * [Login] Register with Google for Student
   */
  registerStudentGoogle(googleIdToken: string): Observable<any> {
    return this.http.post<any>(this.APIUrl + "/register",
      {
        "googleIdToken": googleIdToken,
        "roleName": "student",
      },
      { withCredentials: true });
  }

  /**
   * [Login] Register with Google for Landlord
   */
  registerLandlordGoogle(
    googleIdToken: string,
    phonenumber: string,
    facebookUrl: string
  ): Observable<any> {
    return this.http.post<any>(
      this.APIUrl + "/register",
      {
        "googleIdToken": googleIdToken,
        "phonenumber": phonenumber,
        "facebookUrl": facebookUrl,
        "roleName": "landlord"
      },
      { withCredentials: true });
  }

  /**
   * [Login] Register with Facebook for Student
   */
  registerStudentFacebook(facebookId: string, name: string): Observable<any> {
    return this.http.post<User>(this.APIUrl + "/register", {
      "facebookUserId": facebookId,
      "displayName": name,
      "roleName": "student"
    },
      { withCredentials: true });
  }

  /**
   * [Login] Register with Facebook for Landlord
   */
  registerLandlordFacebook(
    facebookId: string,
    name: string,
    phonenumber: string,
    facebookUrl: string
  ): Observable<any> {
    return this.http.post<any>(
      this.APIUrl + "/register",
      {
        "facebookUserId": facebookId,
        "displayName": name,
        "phonenumber": phonenumber,
        "facebookUrl": facebookUrl,
        "roleName": "landlord"
      },
      { withCredentials: true });
  }

  /**
   * [Login] Logout
   */
  logout(): Observable<any> {
    return this.http.get<any>(this.APIUrl + "/logout", { withCredentials: true });
  }

  countTotalLandlords(): Observable<number> {
    return this.http.get<number>(this.APIUrl + "/CountTotalLandlord");
  }

  countActiveLandlords(): Observable<number> {
    return this.http.get<number>(this.APIUrl + "/CountActiveLandlord");
  }

  countInactiveLandlords(): Observable<number> {
    return this.http.get<number>(this.APIUrl + "/CountInactiveLandlord");
  }
}
