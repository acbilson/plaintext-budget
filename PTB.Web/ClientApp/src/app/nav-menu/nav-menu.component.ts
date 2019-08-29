import { Component, OnInit } from '@angular/core';
import { PtbService } from '../services/ptb.service';
import { LoggingService } from '../services/logging.service';
import { IFileFolders, IPTBFile } from '../ledger/interfaces/ptbfile';
import { INavLink } from './nav-link';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  public fileFolders: IFileFolders;
  public defaultLedgerName: string;
  public ledgerLinks: INavLink[];

  constructor(private ptbService: PtbService, private logger: LoggingService) {
    this.ptbService = ptbService;
    this.logger = logger;
    this.fileFolders = null;
    this.ledgerLinks = [];
  }

  ngOnInit() {

    this.getFileFolders()
    .then(fileFolders => {
      this.getDefaultName(fileFolders);
      this.generateNavLinks(fileFolders.ledgerFolder.files);
      return fileFolders;
    })
    .catch(error => console.log(error));
  }

  getFileFolders(): Promise<IFileFolders> {

    return this.ptbService.getFileFolders()
    .then(fileFolders => this.fileFolders = fileFolders);
  }

  getDefaultName(fileFolders: IFileFolders): void {

      this.defaultLedgerName = fileFolders.ledgerFolder.files.find(
        file => file.fileName === fileFolders.ledgerFolder.defaultFileName + '.txt').shortName;
  }

  generateNavLinks(files: IPTBFile[]): void {

      files.forEach(ledgerFile => {
        this.ledgerLinks.push({
          'path': '/ledger',
          'name': ledgerFile.shortName,
          'text': ledgerFile.shortName
        });
      });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
