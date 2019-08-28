import { Component, OnInit, HostListener, Input } from '@angular/core';
import { ILedgerEntry, IRow } from '../ledger';
import { IPTBFile, IFileFolders } from '../ptbfile';
import { PtbService } from '../../services/ptb.service';
import { LoggingService } from '../../services/logging.service';
import { HttpErrorResponse } from '@angular/common/http';
import { LoggingLevel } from '../../services/logging-level';
import { LedgerPageComponent } from '../ledger-page.component';

@Component({
  selector: 'app-ledger-table',
  templateUrl: './ledger-table.component.html',
  styleUrls: ['./ledger-table.component.css']
})
export class LedgerTableComponent implements OnInit {

  @Input() fileFolders: IFileFolders;
  public ledgers: ILedgerEntry[];
  private context: string;
  private logger: LoggingService;

  constructor(private ptbService: PtbService, private loggingService: LoggingService) {
    this.ptbService = ptbService;
    this.logger = loggingService;
    this.ledgers = [];
    this.context = 'ledger-table';
  }

  log(message: string): void {
    this.logger.log(this.context, message);
  }

  ngOnInit() {
    // gets all the available ledger files
      const defaultShortName = this.getDefaultShortName(this.fileFolders);
      this.readLedgers(defaultShortName, 0, 25).then(ledgers => this.ledgers = ledgers);
  }

  private getDefaultShortName(fileFolders: IFileFolders): string {

    let defaultFileName = fileFolders.ledgerFolder.defaultFileName;
    let name = fileFolders.ledgerFolder.files.find(l => l.fileName == (defaultFileName + '.txt')).shortName;
    return name;
  }

  private getLedgerByIndex(index: string): ILedgerEntry {
    return this.ledgers.find(ledger => ledger.index == parseInt(index));
  }

  async updateLedgerSubcategory(index: string, subcategory: string): Promise<IRow> {
    let updatedLedger: IRow;

    try {
      let ledger = this.getLedgerByIndex(index);
      ledger.subcategory.value = subcategory;
      ledger.locked.value = '1';

      updatedLedger = await this.ptbService.updateLedger(ledger);
    }
    catch (error) {
      console.log(error);
    }

    this.log(`updated ledger ${index} with subcategory ${subcategory}`);
    return updatedLedger;
  }

  async updateLedgerSubject(index: string, subject: string): Promise<IRow> {
    let updatedLedger : IRow;

    try {
      var ledger = this.getLedgerByIndex(index);
      ledger.subject.value = subject;
      ledger.locked.value = '1';

      updatedLedger = await this.ptbService.updateLedger(ledger);
    }
    catch (error) {
      console.log(error);
    }

    this.log(`updated ledger ${index} with subject ${subject}`);
    return updatedLedger;
  }

  @HostListener('document:scroll', ['$event'])
  scrollHandler(event: UIEvent) {

    // adds a few extra pixels to make the scroll begin just before scrolling ends for better user experience
    const extraPixels = 10;
    const position = window.innerHeight + window.pageYOffset;
    const bottom = document.documentElement.offsetHeight - extraPixels;
    //console.log('pos is '+position+' and bottom is '+bottom);

    if (position >= bottom ) {

      // index of the last leger entry in the row
      const lastIndex = this.ledgers[this.ledgers.length - 1].index;

      // adds ledger size plus carriage return to skip returning the last row again (need to retrieve schema via API)
      const finalIndex = lastIndex + 142;
      const ledgerCount = 10;

      const defaultShortName = this.getDefaultShortName(this.fileFolders);

      this.readLedgers(defaultShortName, finalIndex, ledgerCount)
      .then(ledgers => {
        this.ledgers = this.ledgers.concat(ledgers);
      })
      .catch(error => console.log(`failed to retrieve next ${ledgerCount} ledgers with message: ${error.message}`));
    }
  }

  async readLedgers(fileName: string, startIndex: number, count: number): Promise<ILedgerEntry[]> {

    let ledgers = [];

    try {
      ledgers = await this.ptbService.readLedgers(fileName, startIndex, count);
    } catch (error) {
      console.log(`failed to retrieve ledgers with message: ${error.message}`);
    }

    this.log(`read ${count} ledgers from ${fileName} starting at index ${startIndex}`);
    return ledgers;
  }
}

