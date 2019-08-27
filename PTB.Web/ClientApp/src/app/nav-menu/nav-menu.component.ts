import { Component, OnInit } from '@angular/core';
import { PtbService } from '../services/ptb.service';
import { LoggingService } from '../services/logging.service';
import { IFileFolders, IPTBFile } from '../ledger/ptbfile';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  public fileFolders: IFileFolders;
  public defaultLedgerName: string;
  public ledgerLinks: object[];

  constructor(private ptbService: PtbService, private logger: LoggingService) {
    this.ptbService = ptbService;
    this.logger = logger;
    this.fileFolders = null;
  }

  ngOnInit() {

    this.ptbService.getFileFolders()
    .then(fileFolders => {
      this.fileFolders = fileFolders;
      this.defaultLedgerName = fileFolders.ledgerFolder.files.find(file => file.fileName == fileFolders.ledgerFolder.defaultFileName + '.txt').shortName;

      let links = [];
      this.fileFolders.ledgerFolder.files.forEach(ledgerFile => {
        links.push({
          'path': '/ledger',
          'name': ledgerFile.shortName,
          'text': ledgerFile.shortName
        });
      });

      this.ledgerLinks = links;

      console.log(fileFolders);
    }).catch(error => console.log(error));

  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
