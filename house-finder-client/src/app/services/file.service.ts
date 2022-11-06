import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  readonly APIUrl = "https://localhost:5001/api/File";

  constructor(private http: HttpClient) { }
  
  downloadTemplateFile(): Observable<HttpResponse<Blob>>{
    return this.http.get<Blob>(this.APIUrl + '/download',{ observe: 'response', responseType: 'blob' as 'json'});
  }

  uploadFile(){
    
  }
}
