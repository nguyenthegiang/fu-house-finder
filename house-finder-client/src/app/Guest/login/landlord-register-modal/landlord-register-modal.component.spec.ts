import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LandlordRegisterModalComponent } from './landlord-register-modal.component';

describe('LandlordRegisterModalComponent', () => {
  let component: LandlordRegisterModalComponent;
  let fixture: ComponentFixture<LandlordRegisterModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LandlordRegisterModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LandlordRegisterModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
