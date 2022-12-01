import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
//environment variable for API URL
import { environment } from 'src/environments/environment'; 
import { ImagesOfRoomUploadData } from '../models/imagesOfRoom';


@Injectable({
  providedIn: 'root'
})
export class FileService {
  readonly APIUrl = `${environment.api_url}/File`;

  constructor(private http: HttpClient) { }
  
  downloadTemplateFile(): Observable<HttpResponse<Blob>>{
    return this.http.get<Blob>(this.APIUrl + '/download',{ observe: 'response', responseType: 'blob' as 'json'});
  }

  uploadDataFile(file: File){
    const formData = new FormData(); 
        
    // Store form name as "file" with file data
    formData.append("file", file, file.name);
      
    // Make http post request over api
    // with formData as req
    return this.http.post(this.APIUrl + '/upload', formData);
  }

  uploadRoomImageFile(data: ImagesOfRoomUploadData, file: File){
    const formData = new FormData();

    formData.append("file", file, file.name);
    formData.append("room", JSON.stringify(data));

    return this.http.post(this.APIUrl + "/room/image", formData, {withCredentials: true})
  }

  uploadIDC(frontFile: File, backFile: File){
    const formData = new FormData(); 

    // Store form name as "file" with file data
    formData.append("files", frontFile, frontFile.name);
    formData.append("files", backFile, backFile.name);
      
    // Make http post request over api
    // with formData as req

    return this.http.post(this.APIUrl + '/idc/upload', formData, {withCredentials: true});
  }
  
  uploadHouseImageFile(file1: File, file2: File, file3: File, houseId: number){
    const formData = new FormData();

    // Store form name as "file" with file data
    formData.append("files", file1, file1.name);
    formData.append("files", file2, file2.name);
    formData.append("files", file3, file3.name);

    return this.http.post(this.APIUrl + `/house/image/${houseId}`, formData, {withCredentials: true})
  }
}
