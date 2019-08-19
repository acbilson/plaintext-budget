import { Component, OnInit, Inject } from '@angular/core';
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

  constructor(private ptbService: PtbService, @Inject('BASE_URL') baseUrl: string) {
    this.ptbService = ptbService;
    this.ledgers = [];
  };

  ngOnInit() {
    this.readLedgers(0,25);
  }

  readLedgers(startIndex: number, count: number): void {
    this.ptbService.readLedgers(startIndex, count).subscribe(result => { 
      this.ledgers = result; 
      console.log("retrieved " + this.ledgers.length + " ledger entries.");
    }, error => console.error(error));
  }
}


