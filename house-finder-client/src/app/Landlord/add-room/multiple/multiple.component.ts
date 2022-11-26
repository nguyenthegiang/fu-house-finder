import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatProgressBar } from '@angular/material/progress-bar';
import { ImagesOfRoomUploadData, ImagesOfRoomUploadFileData } from 'src/app/models/imagesOfRoom';
import { FileService } from 'src/app/services/file.service';

@Component({
  selector: 'app-multiple',
  templateUrl: './multiple.component.html',
  styleUrls: ['./multiple.component.scss']
})
export class MultipleComponent implements OnInit {

  file: File | any; 
  images: any;
  houseId: number | any;
  matProgressBarValue = 0;

  constructor(private fileService: FileService) { }

  ngOnInit(): void {
  }

  onTemplateChange(event: any){
    this.file = event.target.files[0];
  }
  
  onImagesChange(event: any){
    this.images = event.target.files;
  }

  downloadTemplate(){
    this.fileService.downloadTemplateFile()
      .subscribe((response) => {
        const downloadLink = document.createElement('a');
        downloadLink.href = URL.createObjectURL(new Blob([response.body as BlobPart], { type: response.body?.type }));

        const contentDisposition = response.headers.get('content-disposition');
        const fileName = contentDisposition?.split(';')[1].split('filename')[1].split('=')[1].trim() ?? "house-template.xlsx";
        downloadLink.download = fileName;
        downloadLink.click();
      });
  }

  uploadDataFile(){
    this.fileService.uploadDataFile(this.file).subscribe((resp)=>{});
  }

  uploadImageFiles(){
    var uploadData: Array<ImagesOfRoomUploadFileData> = [];
    
    for (var i=0; i<this.images.length; i++) {
      var name = this.images[i].name;
      var roomName = name.split("-")[0];
      var building = Number(name.split("-")[1]);
      var floor = Number(name.split("-")[2]);
      var imageIndex = Number(name.split("-")[3].split(".")[0]);

      if (isNaN(building) || isNaN(floor) || isNaN(imageIndex)) {
        console.log("invalid file name");
        continue;
      }
      uploadData.push({
        image: this.images[i],
        data: {
          buildingNumber: building,
          roomName: roomName,
          floorNumber: floor,
          houseId: 1
        }
      });
    };

    var progress = 0;
    uploadData.forEach((data: ImagesOfRoomUploadFileData) =>{
      this.fileService.uploadRoomImageFile(data.data, data.image).subscribe(resp => {
        progress += 1;
        this.matProgressBarValue = progress / uploadData.length * 100;
      },
      err => {
        progress += 1;
        this.matProgressBarValue = progress / uploadData.length * 100;
      });
    });
  }

}
