import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LandlordChangePasswordComponent } from './landlord-change-password.component';

describe('LandlordChangePasswordComponent', () => {
  let component: LandlordChangePasswordComponent;
  let fixture: ComponentFixture<LandlordChangePasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LandlordChangePasswordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LandlordChangePasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
