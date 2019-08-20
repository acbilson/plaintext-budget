import { Component, OnInit, HostListener } from '@angular/core';
import { ILedger } from './ledger';
import { IPTBFile } from './ptbfile';
import { PtbService } from '../ptb.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-ledger',
  templateUrl: './ledger.component.html',
  styleUrls: ['./ledger.component.css']
})
export class LedgerComponent implements OnInit {
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
      ledger.subject = subcategory;
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

      this.readLedgers(finalIndex, 10);
    }
  }

  async readLedgers(startIndex: number, count: number): Promise<ILedger[]> {

    let ledgers = [];

    try {
      ledgers = await this.ptbService.readLedgers(startIndex, count);
    } catch (error) {
      console.log(error);
    }

    return ledgers;
  }

  async getLedgerFiles(): Promise<IPTBFile[]> {
    let files = [];

    try {
      files = await this.ptbService.getLedgerFiles();
    }
    catch (error) {
      console.log(error);
    }

    return files;
  }
}


