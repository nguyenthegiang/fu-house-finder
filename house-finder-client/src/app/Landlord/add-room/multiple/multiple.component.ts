import { Component, OnInit } from '@angular/core';
import { FileService } from 'src/app/services/file.service';

@Component({
  selector: 'app-multiple',
  templateUrl: './multiple.component.html',
  styleUrls: ['./multiple.component.scss']
})
export class MultipleComponent implements OnInit {

  file: File | any; 
  images: File[] | any;

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
    this.fileService.uploadFile(this.file).subscribe(()=>{});
  }

  uploadImageFiles(){
    
  }

}
