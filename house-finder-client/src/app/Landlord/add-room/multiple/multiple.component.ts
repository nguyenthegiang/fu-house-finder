import { Component, OnInit } from '@angular/core';
import { FileService } from 'src/app/services/file.service';

@Component({
  selector: 'app-multiple',
  templateUrl: './multiple.component.html',
  styleUrls: ['./multiple.component.scss']
})
export class MultipleComponent implements OnInit {

  constructor(private fileService: FileService) { }

  ngOnInit(): void {
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

}
