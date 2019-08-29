import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, HostListener, Input } from '@angular/core';
import { ILedgerEntry } from '../interfaces/ledger-entry';
import { IRow } from '../interfaces/row';
import { LedgerService } from '../../services/ledger/ledger.service';
import { LoggingService } from '../../services/logging/logging.service';

@Component({
  selector: 'app-ledger-table',
  templateUrl: './ledger-table.component.html',
  styleUrls: ['./ledger-table.component.css']
})
export class LedgerTableComponent implements OnInit {

  public ledgerName: string;
  public ledgers: ILedgerEntry[];

  // logging
  private context: string;

  constructor(
    private ledgerService: LedgerService,
    private logger: LoggingService,
    private route: ActivatedRoute,
  ) {
    this.ledgerService = ledgerService;
    this.route = route;

    this.ledgerName = null;
    this.ledgers = [];
    this.context = 'ledger-table';
  }

  ngOnInit() {
    this.ledgerName = this.route.snapshot.paramMap.get('name');
    this.readLedgers(this.ledgerName, 0, 25).then(ledgers => this.ledgers = ledgers);
  }

  private getLedgerByIndex(index: string): ILedgerEntry {
    return this.ledgers.find(ledger => ledger.index === parseInt(index, 10));
  }

  async updateLedgerSubcategory(index: string, subcategory: string): Promise<IRow> {
    let updatedLedger: IRow;

    try {
      const ledger = this.getLedgerByIndex(index);
      ledger.subcategory.value = subcategory;
      ledger.locked.value = '1';

      updatedLedger = await this.ledgerService.updateLedger(ledger);
    } catch (error) {
      this.logger.logError(this.context, `failed to update ledger subcategory with message: ${error.message}`);
    }

    this.logger.logInfo(this.context, `updated ledger ${index} with subcategory ${subcategory}`);
    return updatedLedger;
  }

  async updateLedgerSubject(index: string, subject: string): Promise<IRow> {
    let updatedLedger: IRow;

    try {
      const ledger = this.getLedgerByIndex(index);
      ledger.subject.value = subject;
      ledger.locked.value = '1';

      updatedLedger = await this.ledgerService.updateLedger(ledger);
    } catch (error) {
      this.logger.logError(this.context, `failed to update ledger subject with message: ${error.message}`);
    }

    this.logger.logInfo(this.context, `updated ledger ${index} with subject ${subject}`);
    return updatedLedger;
  }

  @HostListener('document:scroll', ['$event'])
  scrollHandler(event: UIEvent) {

    // adds a few extra pixels to make the scroll begin just before scrolling ends for better user experience
    const extraPixels = 10;
    const position = window.innerHeight + window.pageYOffset;
    const bottom = document.documentElement.offsetHeight - extraPixels;
    // console.log('pos is '+position+' and bottom is '+bottom);

    if (position >= bottom) {

      // index of the last leger entry in the row
      const lastIndex = this.ledgers[this.ledgers.length - 1].index;

      // adds ledger size plus carriage return to skip returning the last row again (need to retrieve schema via API)
      const finalIndex = lastIndex + 142;
      const ledgerCount = 10;

      this.readLedgers(this.ledgerName, finalIndex, ledgerCount)
        .then(ledgers => {
          this.ledgers = this.ledgers.concat(ledgers);
        })
        .catch(error => this.logger.logError(this.context, `failed to retrieve next ${ledgerCount} ledgers with message: ${error.message}`));
    }
  }

  async readLedgers(fileName: string, startIndex: number, count: number): Promise<ILedgerEntry[]> {

    this.logger.logInfo(this.context, `reading logs from ledger ${fileName}`);

    let ledgers = [];

    try {
      ledgers = await this.ledgerService.readLedgers(fileName, startIndex, count);
    } catch (error) {
      this.logger.logError(this.context, `failed to retrieve ledgers with message: ${error.message}`);
    }

    this.logger.logInfo(this.context, `read ${count} ledgers from ${fileName} starting at index ${startIndex}`);
    return ledgers;
  }
}
