import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardStaffComponent } from './dashboard.component';

describe('DashboardComponent', () => {
  let component: DashboardStaffComponent;
  let fixture: ComponentFixture<DashboardStaffComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DashboardStaffComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardStaffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
