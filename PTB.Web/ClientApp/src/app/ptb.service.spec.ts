import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PtbService } from './ptb.service';

fdescribe('PtbService', () => {
  let service : PtbService;
  let httpMock : HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [],
      imports: [HttpClientTestingModule],
      providers: [PtbService]
    });

    service = TestBed.get(PtbService);
    httpMock = TestBed.get(HttpTestingController);
  });

  fit('should be created', () => {
    expect(service).toBeTruthy();
  });
});
