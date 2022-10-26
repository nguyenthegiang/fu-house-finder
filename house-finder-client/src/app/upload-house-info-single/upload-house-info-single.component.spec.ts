import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadHouseInfoSingleComponent } from './upload-house-info-single.component';

describe('UploadHouseInfoSingleComponent', () => {
  let component: UploadHouseInfoSingleComponent;
  let fixture: ComponentFixture<UploadHouseInfoSingleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UploadHouseInfoSingleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadHouseInfoSingleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
