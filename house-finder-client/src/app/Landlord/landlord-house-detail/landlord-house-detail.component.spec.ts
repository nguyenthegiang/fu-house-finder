import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LandlordHouseDetailComponent } from './landlord-house-detail.component';

describe('LandlordHouseDetailComponent', () => {
  let component: LandlordHouseDetailComponent;
  let fixture: ComponentFixture<LandlordHouseDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LandlordHouseDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LandlordHouseDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
