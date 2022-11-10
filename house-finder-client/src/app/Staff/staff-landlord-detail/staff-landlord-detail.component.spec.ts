import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffLandlordDetailComponent } from './staff-landlord-detail.component';

describe('StaffLandlordDetailComponent', () => {
  let component: StaffLandlordDetailComponent;
  let fixture: ComponentFixture<StaffLandlordDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffLandlordDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffLandlordDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
