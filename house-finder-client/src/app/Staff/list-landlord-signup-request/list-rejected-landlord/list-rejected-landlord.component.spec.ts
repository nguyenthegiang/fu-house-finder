import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListRejectedLandlordComponent } from './list-rejected-landlord.component';

describe('ListRejectedLandlordComponent', () => {
  let component: ListRejectedLandlordComponent;
  let fixture: ComponentFixture<ListRejectedLandlordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListRejectedLandlordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListRejectedLandlordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
