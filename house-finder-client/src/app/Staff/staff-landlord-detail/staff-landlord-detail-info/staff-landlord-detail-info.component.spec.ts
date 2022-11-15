import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffLandlordDetailInfoComponent } from './staff-landlord-detail-info.component';

describe('StaffLandlordDetailInfoComponent', () => {
  let component: StaffLandlordDetailInfoComponent;
  let fixture: ComponentFixture<StaffLandlordDetailInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffLandlordDetailInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffLandlordDetailInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
