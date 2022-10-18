import { TestBed } from '@angular/core/testing';

import { LandlordInformationService } from './landlord-information.service';

describe('LandlordInformationService', () => {
  let service: LandlordInformationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LandlordInformationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
