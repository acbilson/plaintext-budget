import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PtbService } from './ptb.service';
import { ILedger } from './ledger/ledger';

describe('PtbService', () => {
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

  it('instantiates', () => {
    expect(service).toBeTruthy();
  });

  describe('ReadLedgers', () => {

    const testLedgers : ILedger[] = [
      { index: "0", 
        date: "19-01-01", 
        type: "D", 
        amount: "80.17", 
        subcategory: "", 
        title: "dskjfs;f0038", 
        subject: "", 
        location: "", 
        locked: "0" },
        { index: "1", 
        date: "19-01-02", 
        type: "C", 
        amount: "890.17", 
        subcategory: "Coffee", 
        title: "starbucksd;lajwer", 
        subject: "Starbucks", 
        location: "", 
        locked: "1" },      
      ];

    it('Returns an Observable<ILedger[]>', () => {

      const index = 0;
      const count = 25;

      service.readLedgers(index, count).subscribe((result) => {

        expect(result.length).toBe(testLedgers.length);
        expect(result).toEqual(testLedgers);
        });

        const request = httpMock.expectOne(`http://localhost:5000/api/Ledger/ReadLedgers?startIndex=${index}&count=${count}`);
        request.flush(testLedgers);
        httpMock.verify();
    });
  });

  describe('UpdateLedger', () => {

    const testLedgerToUpdate : ILedger = 
      { index: "0", 
        date: "19-01-01", 
        type: "D", 
        amount: "80.17", 
        subcategory: "", 
        title: "dskjfs;f0038", 
        subject: "", 
        location: "", 
        locked: "0" };

    it('Sends ledger in request body', () => {

      service.updateLedger(testLedgerToUpdate);

        const request = httpMock.expectOne('http://localhost:5000/api/Ledger/UpdateLedger');
        expect(request.request.body).toEqual(testLedgerToUpdate); 
        httpMock.verify();
    });
  });
});
