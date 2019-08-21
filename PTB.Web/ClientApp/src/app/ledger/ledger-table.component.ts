import { Component, OnInit, HostListener } from '@angular/core';
import { ILedger } from './ledger';
import { IPTBFile } from './ptbfile';
import { PtbService } from '../ptb.service';
import { Observable } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-ledger-table',
  templateUrl: './ledger-table.component.html',
  styleUrls: ['./ledger-table.component.css']
})
export class LedgerTableComponent implements OnInit {
  public startIndex: number;
  public ledgerCount: number;
  public ledgers: ILedger[];
  public ledgerFiles: IPTBFile[];

  constructor(private ptbService: PtbService) {
    this.ptbService = ptbService;
    this.ledgers = [];
    this.ledgerFiles = [];
  };

  ngOnInit() {
    // gets all the available ledger files
    this.getLedgerFiles().then(files => this.ledgerFiles = files);

    // reads the first twentyfive ledger entries
    this.readLedgers(0,25).then(ledgers => this.ledgers = ledgers);
  }

  private getLedgerByIndex(index: string): ILedger {
    return this.ledgers.find(ledger => parseInt(ledger.index) == parseInt(index));
  }

  async updateLedgerSubcategory(index: string, subcategory: string): Promise<ILedger> {
    let updatedLedger : ILedger;

    try {
      var ledger = this.getLedgerByIndex(index);
      ledger.subcategory = subcategory;
      ledger.locked = '1';

      updatedLedger = await this.ptbService.updateLedger(ledger);
    }
    catch (error) {
      console.log(error);
    }
    
    return updatedLedger;
  }

  async updateLedgerSubject(index: string, subject: string): Promise<ILedger> {

    let updatedLedger : ILedger;

    try {
      var ledger = this.getLedgerByIndex(index);
      ledger.subject = subject;
      ledger.locked = '1';

      updatedLedger = await this.ptbService.updateLedger(ledger);
    }
    catch (error) {
      console.log(error);
    }
    
    return updatedLedger;
  }

  @HostListener('document:scroll', ['$event'])
  scrollHandler(event: UIEvent) {

    // adds a few extra pixels to make the scroll begin just before scrolling ends for better user experience
    let extraPixels = 10;
    let position = window.innerHeight + window.pageYOffset;
    let bottom = document.documentElement.offsetHeight - extraPixels;
    //console.log('pos is '+position+' and bottom is '+bottom);
    
    if(position >= bottom )   {
    
      // index of the last leger entry in the row
      let lastIndex = parseInt(this.ledgers[this.ledgers.length - 1].index);

      // adds ledger size plus carriage return to skip returning the last row again (need to retrieve schema via API)
      let finalIndex = lastIndex + 142;
      let ledgerCount = 10;

      this.readLedgers(finalIndex, ledgerCount)
      .then(ledgers => {
        //console.log(`number of ledgers before read: ${this.ledgers.length}`);
        this.ledgers = this.ledgers.concat(ledgers);
        //console.log(`number of ledgers after read: ${this.ledgers.length}. Should be ${ledgers.length} + ${this.ledgers.length}`);
      })
      .catch(error => console.log(`failed to retrieve next ${ledgerCount} ledgers with message: ${error.message}`));
    }
  }

  async readLedgers(startIndex: number, count: number): Promise<ILedger[]> {

    let ledgers = [];

    try {
      ledgers = await this.ptbService.readLedgers(startIndex, count);
    } catch (error) {
      console.log(`failed to retrieve ledgers with message: ${error.message}`);
    }

    return ledgers;
  }

  async getLedgerFiles(): Promise<IPTBFile[]> {
    let files = [];

    try {
      files = await this.ptbService.getLedgerFiles();
    }
    catch (error) {
      console.log(`failed to retrieve ledger files with message: ${error.message}`);
    }

    return files;
  }
}


