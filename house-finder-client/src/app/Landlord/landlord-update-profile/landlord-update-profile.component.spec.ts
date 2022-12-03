import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LandlordUpdateProfileComponent } from './landlord-update-profile.component';

describe('LandlordUpdateProfileComponent', () => {
  let component: LandlordUpdateProfileComponent;
  let fixture: ComponentFixture<LandlordUpdateProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LandlordUpdateProfileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LandlordUpdateProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
