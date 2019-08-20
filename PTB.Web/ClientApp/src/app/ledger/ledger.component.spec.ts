import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LedgerComponent } from './ledger.component';
import { Pipe, PipeTransform, Injectable } from '@angular/core';
import { ILedger } from './ledger';
import { IPTBFile } from './ptbfile';
import { FormsModule } from '@angular/forms';
import { PtbService } from '../ptb.service';
import { defer } from 'rxjs/observable/defer';

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

  public ledgers: ILedger[];
  public ledgerFiles: IPTBFile[];

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
  }
  
  readLedgers(index: number, count: number) {
    return defer(() => Promise.resolve(this.ledgers));
  }

  getLedgerFiles() {
    return defer(() => Promise.resolve(this.ledgerFiles));
  }
}

fdescribe('LedgerComponent', () => {
  let component: LedgerComponent;
  let fixture: ComponentFixture<LedgerComponent>;
  let mockPtbService: MockPtbService;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [ LedgerComponent, 
                      MyPrependPipeMock, 
                      MyTrimPipeMock,
                      MyDebitPipeMock ],
      providers: [
        { provide: PtbService, useClass: MockPtbService }
      ], })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LedgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    mockPtbService = new MockPtbService();
  });

  fit('instantiates', () => {
    expect(component).toBeTruthy();
  });

  fit('sets properties', (done) => {
    component.ngOnInit();
    fixture.whenStable()
    .then(() => {
      expect(component.ledgers.length).toEqual(mockPtbService.ledgers.length);
      expect(component.ledgers).toEqual(mockPtbService.ledgers);
    });
    done();
  });
});
