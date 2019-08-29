import { Component, OnInit } from '@angular/core';
import { IFileFolders } from '../interfaces/ptbfile';
import { PtbService } from '../../services/ptb.service';
import { LoggingService } from '../../services/logging.service';
import { LedgerTableComponent } from '../ledger-table/ledger-table.component';

@Component({
  selector: 'app-ledger-page',
  templateUrl: './ledger-page.component.html',
  styleUrls: ['./ledger-page.component.css']
})
export class LedgerPageComponent implements OnInit {

  public fileFolders: IFileFolders;
  private context: string;

  constructor(private ptbService: PtbService, private logger: LoggingService) {
    this.ptbService = ptbService;
    this.logger = logger;
    this.context = 'ledger-page';
   }

   log(message: string) {
     this.logger.log(this.context, message);
   }

  ngOnInit() {

    this.getFileFolders()
    .then( (fileFolders: IFileFolders) => {
      this.fileFolders = fileFolders;
      console.log('retrieved file folders in ledger-page');
      return fileFolders;
     });
  }

  async getFileFolders(): Promise<IFileFolders> {
    let files: IFileFolders;

    try {
      files = await this.ptbService.getFileFolders();
    } catch (error) {
      console.log(`failed to retrieve ledger files with message: ${error.message}`);
    }

    this.log(`read all file folders`);
    return files;
  }

}
