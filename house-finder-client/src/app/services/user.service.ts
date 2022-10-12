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
  getUserByUserId(userId: number): Observable<User> {
    return this.http.get<User>(this.APIUrl + "/" + userId);
  }
}
