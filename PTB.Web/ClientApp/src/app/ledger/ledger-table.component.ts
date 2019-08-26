import { Component, OnInit, HostListener } from '@angular/core';
import { ILedger } from './ledger';
import { IPTBFile, IFileFolders } from './ptbfile';
import { PtbService } from '../ptb.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-ledger-table',
  templateUrl: './ledger-table.component.html',
  styleUrls: ['./ledger-table.component.css']
})
export class LedgerTableComponent implements OnInit {
  public ledgers: ILedger[];
  public fileFolders: IFileFolders;

  constructor(private ptbService: PtbService) {
    this.ptbService = ptbService;
    this.ledgers = [];
  }

  ngOnInit() {
    // gets all the available ledger files
    this.getFileFolders()
    .then(files => this.fileFolders = files)
    .then((fileFolders: IFileFolders) => {
      const defaultShortName = this.getDefaultShortName(fileFolders);
      this.readLedgers(defaultShortName, 0, 25).then(ledgers => this.ledgers = ledgers);
    }
    );

    // reads the first twentyfive ledger entries
    
  }

  private getDefaultShortName(fileFolders: IFileFolders): string {

    let defaultFileName = fileFolders.ledgerFolder.defaultFileName;
    //let name = fileFolders.ledgerFolder.files.find(l => l.fileName == defaultFileName).shortName;

    // TODO: implement short name retrieval;
    return "checking";
  }

  private getLedgerByIndex(index: string): ILedger {
    return this.ledgers.find(ledger => ledger.index == parseInt(index));
  }

  async updateLedgerSubcategory(index: string, subcategory: string): Promise<ILedger> {
    let updatedLedger: ILedger;

    try {
      let ledger = this.getLedgerByIndex(index);
      ledger.columns["subcategory"] = subcategory;
      ledger.columns["locked"] = '1';

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
      ledger["subject"] = subject;
      ledger["locked"] = '1';

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

      this.readLedgers(finalIndex, ledgerCount)
      .then(ledgers => {
        //console.log(`number of ledgers before read: ${this.ledgers.length}`);
        this.ledgers = this.ledgers.concat(ledgers);
        //console.log(`number of ledgers after read: ${this.ledgers.length}. Should be ${ledgers.length} + ${this.ledgers.length}`);
      })
      .catch(error => console.log(`failed to retrieve next ${ledgerCount} ledgers with message: ${error.message}`));
    }
  }

  async readLedgers(fileName: string, startIndex: number, count: number): Promise<ILedger[]> {

    let ledgers = [];

    try {
      ledgers = await this.ptbService.readLedgers(fileName, startIndex, count);
      this.ptbService.log(1, 'ledger-table', `read ${ledgers.length} ledgers`);
    } catch (error) {
      console.log(`failed to retrieve ledgers with message: ${error.message}`);
    }

    return ledgers;
  }

  async getFileFolders(): Promise<IFileFolders> {
    let files: IFileFolders;

    try {
      files = await this.ptbService.getFileFolders();
    }
    catch (error) {
      console.log(`failed to retrieve ledger files with message: ${error.message}`);
    }

    return files;
  }
}


