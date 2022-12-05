import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffUpdateProfileComponent } from './staff-update-profile.component';

describe('StaffUpdateProfileComponent', () => {
  let component: StaffUpdateProfileComponent;
  let fixture: ComponentFixture<StaffUpdateProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffUpdateProfileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffUpdateProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
