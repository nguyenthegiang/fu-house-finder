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

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    withCredentials: true
  };

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
    facebookUrl: string,
    address: string
  ): Observable<any> {
    return this.http.post<any>(
      this.APIUrl + "/register",
      {
        "googleIdToken": googleIdToken,
        "phonenumber": phonenumber,
        "facebookUrl": facebookUrl,
        "address": address,
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
    facebookUrl: string,
    address: string
  ): Observable<any> {
    return this.http.post<any>(
      this.APIUrl + "/register",
      {
        "facebookUserId": facebookId,
        "displayName": name,
        "phonenumber": phonenumber,
        "facebookUrl": facebookUrl,
        "address": address,
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

  getLandlordSignUpRequest(): Observable<User[]> {
    return this.http.get<User[]>(this.APIUrl + "/LandlordSignupRequest");
  }

  getRejectedLandlord(): Observable<User[]> {
    return this.http.get<User[]>(this.APIUrl + "/RejectedLandlord");
  }

  updateUserStatus(userId: string, statusId: number): Observable<any> {
    return this.http.put<any>(this.APIUrl + "/" + userId + "/" + statusId, this.httpOptions, { withCredentials: true });
  }

  getCurrentUser(): Observable<User> {
    return this.http.get<User>(this.APIUrl + "/currentUser", { withCredentials: true });
  }

  updateProfile(userId: string, name: string, email: string): Observable<any> {
    return this.http.put<any>(this.APIUrl + "/updateProfile?userId=" + userId + "&name=" + name + "&email=" + email, this.httpOptions);
  }

  landlordUpdateProfile(userId: string, name: string, phoneNumber: string, facebookUrl: string): Observable<any> {
    return this.http.put<any>(this.APIUrl + "/landLordUpdateProfile?userId=" + userId + "&name=" + name + "&phoneNumber=" + phoneNumber + "&facebookUrl=" + facebookUrl, this.httpOptions);
  }

  changePassword(oldPassword: string, newPassword: string): Observable<any> {
    return this.http.put<any>(this.APIUrl + "/change_password", { 'oldPassword': oldPassword, 'newPassword': newPassword }, this.httpOptions);
  }

  createStaff(staff: any): Observable<any> {
    return this.http.post<any>(this.APIUrl + "/staff/create", staff, this.httpOptions);
  }
  updateStaff(staff: any): Observable<any> {
    return this.http.put<any>(this.APIUrl + "/staff/update", staff, this.httpOptions);
  }
  deleteStaff(staff: any): Observable<any> {
    return this.http.delete<any>(this.APIUrl + "/staff/delete?staff=" + staff, this.httpOptions);
  }
  //[Staff/list-landlord] Filter landlords
  filterUser(
    pageSize: number,
    pageNumber: number,
    searchName?: string,
  ): Observable<User[]> {
    //[Paging] count Skip and Top from pageSize & pageNumber
    const skip = pageSize * (pageNumber - 1);
    const top = pageSize;

    //define API here to append query options into it later
    var filterAPIUrl = this.APIUrl;
    filterAPIUrl += `?$skip=${skip}&$top=${top}`;

    //[Filter] flag to check if that filter is the first filter (if is first -> not have 'and')
    var checkFirstFilter = true;

    //[Filter] add filter by name if has (contains name)
    if (searchName != undefined) {
      //if is not the first filter -> need to add 'and' to API URL
      if (!checkFirstFilter) {
        filterAPIUrl += ` and `;
      } else {
        //if this one is the first filter -> mark it so others won't add 'and'
        checkFirstFilter = false;
      }

      filterAPIUrl += `contains(ReportContent, '${searchName}')`;
    }

    console.log(filterAPIUrl);
    return this.http.get<User[]>(filterAPIUrl);
  }
}
