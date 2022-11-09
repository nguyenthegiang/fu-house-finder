import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffHouseDetailComponent } from './staff-house-detail.component';

describe('StaffHouseDetailComponent', () => {
  let component: StaffHouseDetailComponent;
  let fixture: ComponentFixture<StaffHouseDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffHouseDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffHouseDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
