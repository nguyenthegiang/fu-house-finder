import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListLandlordSignupRequestComponent } from './list-landlord-signup-request.component';

describe('ListLandlordSignupRequestComponent', () => {
  let component: ListLandlordSignupRequestComponent;
  let fixture: ComponentFixture<ListLandlordSignupRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListLandlordSignupRequestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListLandlordSignupRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
