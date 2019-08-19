import { TestBed, inject } from '@angular/core/testing';

import { PtbService } from './ptb.service';

describe('PtbService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PtbService]
    });
  });

  it('should be created', inject([PtbService], (service: PtbService) => {
    expect(service).toBeTruthy();
  }));
});
