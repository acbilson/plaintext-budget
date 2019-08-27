import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PtbService } from './ptb.service';
import { ILedgerEntry } from '../ledger/ledger';
import { IPTBFile } from '../ledger/ptbfile';

describe('PtbService', () => {
  let service: PtbService;
  let httpMock: HttpTestingController;

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

    const testLedgers: ILedgerEntry[] = [
      { index: '0',
        date: '19-01-01',
        type: 'D',
        amount: '80.17',
        subcategory: '',
        title: 'dskjfs;f0038',
        subject: '',
        location: '',
        locked: '0' },
        { index: '1',
        date: '19-01-02',
        type: 'C',
        amount: '890.17',
        subcategory: 'Coffee',
        title: 'starbucksd;lajwer',
        subject: 'Starbucks',
        location: '',
        locked: '1' },
      ];

    it('returns a Promise<ILedger[]>', () => {

      // Arrange
      const index = 0;
      const count = 25;

      // Act
      service.readLedgers(index, count).then((result) => {

        // Assert - results
        expect(result.length).toBe(testLedgers.length);
        expect(result).toEqual(testLedgers);
        });

        // Assert - request
        const request = httpMock.expectOne(`http://localhost:5000/api/Ledger/ReadLedgers?startIndex=${index}&count=${count}`);
        request.flush(testLedgers);
        httpMock.verify();
    });
  });

  describe('UpdateLedger', () => {

    const testLedgerToUpdate: ILedgerEntry = {
      index: '0',
        date: '19-01-01',
        type: 'D',
        amount: '80.17',
        subcategory: '',
        title: 'dskjfs;f0038',
        subject: '',
        location: '',
        locked: '0' };

    it('sends ledger in request body', () => {

      // Act
      service.updateLedger(testLedgerToUpdate);

      // Assert
      const request = httpMock.expectOne('http://localhost:5000/api/Ledger/UpdateLedger');
      expect(request.request.body).toEqual(testLedgerToUpdate);
      httpMock.verify();
    });
  });

  describe('GetLedgerFiles', () => {

    const testLedgerFiles: IPTBFile[] =
      [{
        isDefault: true,
        fullName: 'defaultPath'
        },
      {
        isDefault: false,
        fullName: 'anotherPath'
      }];

    it('returns a Promise<IPTBFile[]>', () => {

      // Act
      service.getFolderFiles().then((result) => {

        // Assert - result
        expect(result.length).toBe(testLedgerFiles.length);
        expect(result).toEqual(testLedgerFiles);
        });

        // Assert - request
        const request = httpMock.expectOne('http://localhost:5000/api/File/GetLedgerFiles');
        request.flush(testLedgerFiles);
        httpMock.verify();
    });
  });
});
