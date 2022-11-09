import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffListRoomComponent } from './staff-list-room.component';

describe('StaffListRoomComponent', () => {
  let component: StaffListRoomComponent;
  let fixture: ComponentFixture<StaffListRoomComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StaffListRoomComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StaffListRoomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
