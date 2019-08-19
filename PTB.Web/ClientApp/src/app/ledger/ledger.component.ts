import { Component, OnInit, Inject, HostListener, ElementRef, SimpleChanges } from '@angular/core';
import {ILedger} from './ledger';
import { PtbService } from '../ptb.service';

@Component({
  selector: 'app-ledger',
  templateUrl: './ledger.component.html',
  styleUrls: ['./ledger.component.css']
})
export class LedgerComponent implements OnInit {
  public startIndex: number;
  public ledgerCount: number;
  public ledgers: ILedger[];

  constructor(private ptbService: PtbService, private el: ElementRef, @Inject('BASE_URL') baseUrl: string) {
    this.ptbService = ptbService;
    this.ledgers = [];
  };

  ngOnInit() {
    this.readLedgers(0,25);
  }

  private getLedgerByIndex(index: string): ILedger {
    return this.ledgers.find(ledger => parseInt(ledger.index) == parseInt(index));
  }

  updateLedgerSubcategory(index: string, subcategory: string): void {
    var ledger = this.getLedgerByIndex(index);
    ledger.subcategory = subcategory;
    this.ptbService.updateLedger(ledger);
  }

  updateLedgerSubject(index: string, subject: string): void {
    var ledger = this.getLedgerByIndex(index);
    ledger.subject = subject;
    this.ptbService.updateLedger(ledger);
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

  readLedgers(startIndex: number, count: number): void {
    this.ptbService.readLedgers(startIndex, count).subscribe(result => { 
      this.ledgers = this.ledgers.concat(result);
      console.log("retrieved " + this.ledgers.length + " ledger entries.");
    }, error => console.error(error));
  }
}


