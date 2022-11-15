import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListHouseInfoComponent } from './list-house-info.component';

describe('ListHouseInfoComponent', () => {
  let component: ListHouseInfoComponent;
  let fixture: ComponentFixture<ListHouseInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListHouseInfoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListHouseInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
