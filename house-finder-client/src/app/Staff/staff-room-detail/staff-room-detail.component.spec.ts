import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffRoomDetailComponent } from './staff-room-detail.component';

describe('StaffRoomDetailComponent', () => {
  let component: StaffRoomDetailComponent;
  let fixture: ComponentFixture<StaffRoomDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffRoomDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffRoomDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
