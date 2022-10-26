import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListLandlordComponent } from './list-landlord.component';

describe('ListLandlordComponent', () => {
  let component: ListLandlordComponent;
  let fixture: ComponentFixture<ListLandlordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListLandlordComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListLandlordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
