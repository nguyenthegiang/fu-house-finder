import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LandlordDetailInfoComponent } from './landlord-detail-info.component';

describe('LandlordDetailInfoComponent', () => {
  let component: LandlordDetailInfoComponent;
  let fixture: ComponentFixture<LandlordDetailInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LandlordDetailInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LandlordDetailInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
