import { Component, OnInit } from '@angular/core';
import { ImagesOfRoomUploadData, ImagesOfRoomUploadFileData } from 'src/app/models/imagesOfRoom';
import { FileService } from 'src/app/services/file.service';

@Component({
  selector: 'app-multiple',
  templateUrl: './multiple.component.html',
  styleUrls: ['./multiple.component.scss']
})
export class MultipleComponent implements OnInit {

  file: File | any; 
  images: File[] | any;
  houseId: number | any;

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
    this.images.forEach((image: File) => {
      var name = image.name;
      var roomName = name.split("-")[0];
      try {
        var building = Number(name.split("-")[1]);
        var floor = Number(name.split("-")[2]);
        var imageIndex = Number(name.split("-")[3]);
        uploadData.push({
          image: image,
          data: {
            building: building,
            roomName: roomName,
            floor: floor,
            imageIndex: imageIndex,
            houseId: this.houseId
          }
        });
      } catch (error) {
        console.log("invalid file name");
      }
    });
  }

}
