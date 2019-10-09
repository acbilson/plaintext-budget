import { Component, OnInit } from '@angular/core';
import { LedgerService } from '../../services/ledger/ledger.service';
import { LoggingService } from '../../services/logging/logging.service';
import { LedgerTableComponent } from './ledger-table/ledger-table.component';

@Component({
  selector: 'app-ledger-page',
  templateUrl: './ledger-page.component.html',
  styleUrls: ['./ledger-page.component.css']
})
export class LedgerPageComponent implements OnInit {

  private context: string;

  constructor(private ledgerService: LedgerService, private logger: LoggingService) {
    this.ledgerService = ledgerService;
    this.logger = logger;
    this.context = 'ledger-page';
  }

  ngOnInit() { }
}
