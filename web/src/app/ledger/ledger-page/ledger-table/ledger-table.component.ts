import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, HostListener, Input } from '@angular/core';
import { ILedgerEntry } from 'app/ledger/interfaces/ledger-entry';
import { IRow } from 'app/ledger/interfaces/row';
import { LedgerService } from 'app/services/ledger/ledger.service';
import { LoggingService } from 'app/services/logging/logging.service';
import { SchemaService } from 'app/services/schema/schema.service';
import { IFolderSchema } from 'app/shared/interfaces/folder-schema';
import { FileSchema } from 'app/interfaces/file-schema';
import { scheduleMicroTask } from '@angular/core/src/util';

@Component({
  selector: 'app-ledger-table',
  templateUrl: './ledger-table.component.html',
  styleUrls: ['./ledger-table.component.css']
})
export class LedgerTableComponent implements OnInit {

  public ledgerName: string;
  public ledgers: ILedgerEntry[];
  public ledgerSchema: FileSchema;

  // logging
  private context: string;

  constructor(
    private ledgerService: LedgerService,
    private schemaService: SchemaService,
    private logger: LoggingService,
    private route: ActivatedRoute,
  ) {
    this.ledgerService = ledgerService;
    this.schemaService = schemaService;
    this.route = route;

    this.ledgerName = null;
    this.ledgers = [];
    this.context = 'ledger-table';
  }

  ngOnInit() {
    this.ledgerName = this.route.snapshot.paramMap.get('name');
    this.getFileSchema().then(schema => this.ledgerSchema = schema.find(sch => sch.fileType === 'ledger'));
    this.readLedgers(this.ledgerName, 0, 25).then(ledgers => this.ledgers = ledgers);
    console.log('First ledgers are:');
    console.log(this.ledgers);
  }

  async getFileSchema(): Promise<FileSchema[]> {

    let schema: FileSchema[];

    try {
    schema = await this.schemaService.readFileSchema();
    } catch (error) {
      this.logger.logError(this.context, error);
    }

    return schema;
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

      // adds ledger size plus carriage return to skip returning the last row again
      const finalIndex = lastIndex + this.ledgerSchema.lineSize + 2;
      const ledgerCount = 10;

      this.readLedgers(this.ledgerName, finalIndex, ledgerCount)
        .then(ledgers => {
          this.ledgers = this.ledgers.concat(ledgers);
        })
        .catch(error => {
          this.logger.logError(this.context, `failed to retrieve next ${ledgerCount} ledgers with message: ${error.message}`);
        });
    }
  }

  async readLedgers(fileName: string, startIndex: number, count: number): Promise<ILedgerEntry[]> {

    this.logger.logInfo(this.context, `reading logs from ledger ${fileName}`);

    let ledgers = [];

    try {
      ledgers = await this.ledgerService.read(fileName, startIndex, count);
    } catch (error) {
      this.logger.logError(this.context, `failed to retrieve ledgers with message: ${error.message}`);
    }

    this.logger.logInfo(this.context, `read ${count} ledgers from ${fileName} starting at index ${startIndex}`);
    return ledgers;
  }
}
