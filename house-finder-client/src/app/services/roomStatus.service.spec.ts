import { TestBed } from '@angular/core/testing';

import { StatusService } from './roomStatus.service';

describe('StatusService', () => {
  let service: StatusService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StatusService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
