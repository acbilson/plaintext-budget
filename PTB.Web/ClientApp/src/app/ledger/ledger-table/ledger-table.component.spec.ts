import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LedgerTableComponent } from './ledger-table.component';
import { Pipe, PipeTransform, Injectable } from '@angular/core';
import { ILedgerEntry } from '../interfaces/ledger';
import { IPTBFile } from '../interfaces/ptbfile';
import { FormsModule } from '@angular/forms';
import { PtbService } from '../ptb.service';

@Pipe({ name: 'prependZeros' })
class MyPrependPipeMock implements PipeTransform {
  transform(param) {
    console.log('mocking prependZeros');
    return true;
  }
}

@Pipe({ name: 'debit' })
class MyDebitPipeMock implements PipeTransform {
  transform(param) {
    console.log('mocking debit');
    return true;
  }
}

@Pipe({ name: 'trim' })
class MyTrimPipeMock implements PipeTransform {
  transform(param) {
    console.log('mocking trim');
    return true;
  }
}

@Injectable()
class MockPtbService {

  public ledgers: ILedgerEntry[];
  public ledgerFiles: IPTBFile[];
  public updatedLedger: ILedgerEntry;

  constructor() {

    this.ledgers = [
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

      this.ledgerFiles = [{
        isDefault: true,
        fullName: 'fakepath'
      }];

      this.updatedLedger = this.ledgers[0];
  }
  
  readLedgers(index: number, count: number) : Promise<ILedgerEntry[]> {
    return Promise.resolve(this.ledgers);
  }

  getLedgerFiles() : Promise<IPTBFile[]> {
    return Promise.resolve(this.ledgerFiles);
  }

  updateLedger(ledger: ILedgerEntry) : Promise<ILedgerEntry> {
    return Promise.resolve(this.updatedLedger);
  }
}

describe('LedgerComponent', () => {
  let component: LedgerTableComponent;
  let fixture: ComponentFixture<LedgerTableComponent>;
  let mockPtbService: MockPtbService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [ LedgerTableComponent, 
                      MyPrependPipeMock, 
                      MyTrimPipeMock,
                      MyDebitPipeMock ],
      providers: [
        { provide: PtbService, useClass: MockPtbService }
      ], })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LedgerTableComponent);
    component = fixture.debugElement.componentInstance;
    fixture.detectChanges();
    mockPtbService = new MockPtbService();
  });

  it('instantiates', () => {
    expect(component).toBeTruthy();
  });

  describe('ReadLedgers', () => {

    it('returns ledgers', async () => {

      // Arrange - Act
      const ledgers = await component.readLedgers(0, 25);

      // Assert
      expect(ledgers.length).toEqual(mockPtbService.ledgers.length);
      expect(ledgers).toEqual(mockPtbService.ledgers);
    });
  });

  describe('GetLedgerFiles', () => {

    it('returns ledger files', async () => {

      // Arrange - Act
      const ledgerFiles = await component.getLedgerFiles();

      // Assert
      expect(ledgerFiles.length).toEqual(mockPtbService.ledgerFiles.length);
      expect(ledgerFiles).toEqual(mockPtbService.ledgerFiles);
    });
  });

  describe('UpdateLedgerSubject', () => {

    it('calls update and returns ledger', async () => {

      // Arrange - set ledgers
      component.ledgers = mockPtbService.ledgers;
      fixture.detectChanges();

      // Arrange - watch service
      let ledgerToUpdate = mockPtbService.updatedLedger;
      ledgerToUpdate.subcategory = "CustomSubcategory";
      ledgerToUpdate.locked = "1";
      let mockService = TestBed.get(PtbService);
      spyOn(mockService, 'updateLedger').and.returnValue(Promise.resolve(ledgerToUpdate));

      // Act
      const updatedLedger = await component.updateLedgerSubcategory(ledgerToUpdate.index, ledgerToUpdate.subcategory);

      // Assert
      expect(updatedLedger).toEqual(ledgerToUpdate);
    });
  });

  describe('UpdateLedgerSubcategory', () => {

    it('calls update and returns ledger', async () => {

      // Arrange - set ledgers
      component.ledgers = mockPtbService.ledgers;
      fixture.detectChanges();

      // Arrange - watch service
      let ledgerToUpdate = mockPtbService.updatedLedger;
      ledgerToUpdate.subject = "CustomSubject";
      ledgerToUpdate.locked = "1";
      let mockService = TestBed.get(PtbService);
      spyOn(mockService, 'updateLedger').and.returnValue(Promise.resolve(ledgerToUpdate));

      // Act
      const updatedLedger = await component.updateLedgerSubject(ledgerToUpdate.index, ledgerToUpdate.subject);

      // Assert
      expect(updatedLedger).toEqual(ledgerToUpdate);
    });
  });
});
