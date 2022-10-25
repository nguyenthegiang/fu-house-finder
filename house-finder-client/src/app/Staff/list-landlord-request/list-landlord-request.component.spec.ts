import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListLandlordRequestComponent } from './list-landlord-request.component';

describe('ListLandlordRequestComponent', () => {
  let component: ListLandlordRequestComponent;
  let fixture: ComponentFixture<ListLandlordRequestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListLandlordRequestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListLandlordRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
