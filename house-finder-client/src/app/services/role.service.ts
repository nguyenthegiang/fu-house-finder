import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RoleService {
  readonly APIUrl = `${environment.api_url}/roles`;
  constructor(private http: HttpClient) { 

  }
  getStaffRoles(): Observable<Array<any>>{
    return this.http.get<Array<any>>(this.APIUrl + '/staff', {withCredentials: true});
  }
}
