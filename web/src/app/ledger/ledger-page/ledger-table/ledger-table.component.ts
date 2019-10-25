import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, HostListener, Input } from '@angular/core';
import { LedgerEntry } from 'app/interfaces/ledger-entry';
import { Row } from 'app/interfaces/row';
import { LedgerService } from 'app/services/ledger/ledger.service';
import { LoggingService } from 'app/services/logging/logging.service';
import { SchemaService } from 'app/services/schema/schema.service';
import { FileSchema } from 'app/interfaces/file-schema';
import { scheduleMicroTask } from '@angular/core/src/util';
import { BaseResponse } from 'app/interfaces/response/base-response';

@Component({
  selector: 'app-ledger-table',
  templateUrl: './ledger-table.component.html',
  styleUrls: ['./ledger-table.component.css']
})
export class LedgerTableComponent implements OnInit {
  public ledgerName: string;
  public ledgers: LedgerEntry[];
  public ledgerSchema: FileSchema;

  // logging
  private context: string;

  constructor(
    private ledgerService: LedgerService,
    private schemaService: SchemaService,
    private logger: LoggingService,
    private route: ActivatedRoute
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
    this.getFileSchema().then(
      schema =>
        (this.ledgerSchema = schema.find(sch => sch.fileType === 'ledger'))
    );
    this.readLedgers(this.ledgerName, 0, 25).then(ledgers => {
      this.ledgers = ledgers;
      console.log('First ledgers are:');
      console.log(this.ledgers);
    });
  }

  async getFileSchema(): Promise<FileSchema[]> {
    let schema: FileSchema[];

    try {
      const res = await this.schemaService.read();
      schema = res.files;
    } catch (error) {
      this.logger.logError(this.context, error);
    }

    return schema;
  }

  private getLedgerById(id: string): LedgerEntry {
    return this.ledgers.find(ledger => ledger.id === parseInt(id, 10));
  }

  async updateLedgerSubcategory(id: string, subcategory: string): Promise<any> {
    try {
      const ledger = this.getLedgerById(id);
      ledger.subcategory.value = subcategory;
      ledger.locked.value = '1';
      const response: BaseResponse = await this.ledgerService.update(ledger);
    } catch (error) {
      this.logger.logError(
        this.context,
        `failed to update ledger subcategory with message: ${error.message}`
      );
    }

    this.logger.logInfo(
      this.context,
      `updated ledger ${id} with subcategory ${subcategory}`
    );
  }

  async updateLedgerSubject(id: string, subject: string): Promise<any> {
    try {
      const ledger = this.getLedgerById(id);
      ledger.subject.value = subject;
      ledger.locked.value = '1';

      const response: BaseResponse = await this.ledgerService.update(ledger);
    } catch (error) {
      this.logger.logError(
        this.context,
        `failed to update ledger subject with message: ${error.message}`
      );
    }

    this.logger.logInfo(
      this.context,
      `updated ledger ${id} with subject ${subject}`
    );
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
      const lastId = this.ledgers[this.ledgers.length - 1].id;
      const nextId = lastId + 1;
      const ledgerCount = 10;

      this.readLedgers(this.ledgerName, nextId, ledgerCount)
        .then(ledgers => {
          this.ledgers = this.ledgers.concat(ledgers);
        })
        .catch(error => {
          this.logger.logError(
            this.context,
            `failed to retrieve next ${ledgerCount} ledgers with message: ${error.message}`
          );
        });
    }
  }

  async readLedgers(
    fileName: string,
    startId: number,
    count: number
  ): Promise<LedgerEntry[]> {
    this.logger.logInfo(this.context, `reading logs from ledger ${fileName}`);

    let ledgers = [];

    try {
      ledgers = await this.ledgerService.read(fileName, startId, count);
    } catch (error) {
      this.logger.logError(
        this.context,
        `failed to retrieve ledgers with message: ${error.message}`
      );
    }

    this.logger.logInfo(
      this.context,
      `read ${count} ledgers from ${fileName} starting at index ${startId}`
    );
    return ledgers;
  }
}
