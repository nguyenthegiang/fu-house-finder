import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatProgressBar } from '@angular/material/progress-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { ImagesOfRoomUploadData, ImagesOfRoomUploadFileData } from 'src/app/models/imagesOfRoom';
import { FileService } from 'src/app/services/file.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-multiple',
  templateUrl: './multiple.component.html',
  styleUrls: ['./multiple.component.scss']
})
export class MultipleComponent implements OnInit {

  file: File | any = { name: "" };
  images: any;
  houseId: number | any;
  matProgressBarValue = 0;
  progressDisplay = false;
  imageNames: any;

  // Alerts
  // @ViewChild('serverErrorAlert') private serverErrorAlert: SwalComponent | undefined;
  // @ViewChild('dataAndImageNotFound') private dataAndImageNotFound: SwalComponent | undefined;
  @ViewChild('uploadSuccess') private uploadSuccess: SwalComponent | undefined;

  constructor(private fileService: FileService,
    private router: Router,
    private route: ActivatedRoute,) { }

  ngOnInit(): void {
  }

  onTemplateChange(event: any) {
    if (event.target.files[0].type == 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet') {
      this.file = event.target.files[0];
    }
    else {
      this.toast(true, 'error', true, 'Lỗi dữ liệu', 'Không đúng định dạng file');
    }
  }

  onImagesChange(event: any) {
    this.images = event.target.files;
    this.imageNames = [];
    for (var i = 0; i < event.target.files.length; i++) {
      if (!this.images[i].type.includes('image')) {
        continue;
      }
      try {
        var name = event.target.files[i].name;
        var roomName = name.split("-")[0];
        var building = Number(name.split("-")[1]);
        var floor = Number(name.split("-")[2]);
        var imageIndex = Number(name.split("-")[3].split(".")[0]);

        if (isNaN(building) || isNaN(floor) || isNaN(imageIndex)) {
          continue;
        }
        else {
          this.imageNames.push(event.target.files[i].name);
        }
      } catch (error) {
        continue;
      }
    };
  }

  downloadTemplate() {
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

  async uploadDataFile(houseId: number) {
    if (this.file instanceof File) {
      await this.fileService.uploadDataFile(this.file, houseId).toPromise()
        .then(resp => { })
        .catch(err => { });
    }
    else {
      await this.toast(true, 'error', true, 'Lỗi dữ liệu', 'Chưa chọn file data');
    }
  }

  uploadImageFiles(houseId: number) {
    this.progressDisplay = true;
    var uploadData: Array<ImagesOfRoomUploadFileData> = [];

    if (this.images == undefined) {
      this.toast(true, 'error', true, 'Lỗi dữ liệu', 'Chưa chọn ảnh');
      return;
    }

    for (var i = 0; i < this.images.length; i++) {
      if (!this.images[i].type.includes('image')) {
        continue;
      }
      try {
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
            houseId: houseId
          }
        });
      } catch (error) {
        continue;
      }
    };
    var error: Array<string> = [];
    var serverErr = false;
    var progress = 0;
    uploadData.forEach((data: ImagesOfRoomUploadFileData) => {
      this.fileService.uploadRoomImageFile(data.data, data.image).subscribe(async resp => {
        progress += 1;
        this.matProgressBarValue = progress / uploadData.length * 100;
        if (progress == uploadData.length) {
          this.progressDisplay = false;
          if (serverErr) {
            await this.toast(true, 'error', true, 'Lỗi server');
          }
          else if (error.length > 0) {
            let errLog = error.join(', ');
            this.toast(true, 'error', true, 'Lỗi dữ liệu', 'Không tìm thấy thông tin phòng cho ảnh ' + errLog);
          }
          else {
            // this.toast(false, 'success', false, 'Thông tin trọ', 'Thông tin phòng của nhà trọ đã được upload', '');
            this.uploadSuccess?.fire();
          }
        }
      },
        async err => {
          if (err.status == 404) {
            error.push(data.image.name);
          }
          else {
            serverErr = true;
          }
          progress += 1;
          this.matProgressBarValue = progress / uploadData.length * 100;
          if (progress == uploadData.length) {
            this.progressDisplay = false;
            if (serverErr) {
              await this.toast(true, 'error', true, 'Lỗi server');
            }
            if (error.length > 0) {
              let errLog = error.join(', ');
              this.toast(true, 'error', true, 'Lỗi dữ liệu', 'Không tìm thấy thông tin phòng cho ảnh ' + errLog);
            }
          }
        });
    });
    if (progress == uploadData.length) this.progressDisplay = false;
  }

  // Create customized Alerts
  async toast(toast: boolean = false, typeIcon: any = 'error',
    timerProgressBar: boolean = true, title: any = '',
    text: any = '', position: any = 'top-end',
    confirmButton: boolean = true) {
    await Swal.fire({
      toast: toast,
      position: position,
      showConfirmButton: confirmButton,
      icon: typeIcon,
      timerProgressBar,
      timer: 3000,
      title: title,
      text: text
    })
  }

  // Back to previous Page (House detail), used when upload successfully
  backToPreviousPage() {
    var houseId = Number(this.route.snapshot.queryParamMap.get('houseId'));
    this.router.navigate(['/Landlord/landlord-house-detail/' + houseId]);
  }
}
