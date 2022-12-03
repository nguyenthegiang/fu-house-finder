import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffChangePasswordComponent } from './staff-change-password.component';

describe('StaffChangePasswordComponent', () => {
  let component: StaffChangePasswordComponent;
  let fixture: ComponentFixture<StaffChangePasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffChangePasswordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffChangePasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
