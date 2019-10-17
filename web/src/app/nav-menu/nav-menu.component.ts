import { Component, OnInit } from '@angular/core';
import { SchemaService } from '../services/schema/schema.service';
import { LoggingService } from '../services/logging/logging.service';
import { IPTBFile } from '../shared/interfaces/ptbfile';
import { IFileFolders } from '../shared/interfaces/file-folders';
import { INavLink } from './nav-link';
import { FolderService } from 'app/services/folder/folder.service';
import {Folder} from 'app/interfaces/folder';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  public fileFolders: Folder[];

  // ledgers
  public defaultLedgerName: string;
  public ledgerLinks: INavLink[];

  // budget
  public defaultBudgetName: string;

  // logging
  private context: string;

  constructor(private schemaService: SchemaService, private logger: LoggingService, private folderService: FolderService) {
    this.folderService = folderService;
    this.schemaService = schemaService;
    this.logger = logger;
    this.fileFolders = null;
    this.ledgerLinks = [];
    this.defaultBudgetName = 'budget';

    this.context = 'nav-menu';
  }

  ngOnInit() {

    this.getFileFolders()
      .then(fileFolders => {
        this.getDefaultLedgerName(fileFolders);
        this.getDefaultBudgetName(fileFolders);
        this.generateNavLinks(fileFolders);
      }).catch(error => this.logger.logError(this.context, error));
  }

  async getFileFolders(): Promise<Folder[]> {

    let fileFolders: Folder[];
    try {
      fileFolders = await this.folderService.read();
    } catch (error) {
      this.logger.logError(this.context, error);
    }

    return fileFolders;
  }

  getDefaultBudgetName(fileFolders: Folder[]): void {

    // this.defaultBudgetName = fileFolders.budgetFolder.files.find(
    //   file => file.fileName === fileFolders.budgetFolder.defaultFileName + '.txt').shortName;
    this.defaultBudgetName = '19-01-01:31';
    this.logger.logInfo(this.context, `set default budget to ${this.defaultBudgetName}`);
  }


  getDefaultLedgerName(fileFolders: Folder[]): void {

    this.defaultLedgerName = fileFolders.find(fold => fold.fileType === 'ledger') + '.txt';
    this.logger.logInfo(this.context, `set default ledger to ${this.defaultLedgerName}`);
  }

  generateNavLinks(folders: Folder[]): void {

    const ledgerFolder = folders.find(fold => fold.fileType === 'ledger');
    this.logger.logInfo(this.context, `creating ${ledgerFolder.files.length} links`);

    ledgerFolder.files.forEach(file => {
      const navLink: INavLink = {
        'path': '/ledger',
        'name': file.shortName,
        'text': file.shortName
      };
      this.ledgerLinks.push(navLink);
      this.logger.logDebug(this.context, `NavLink={path:${navLink.path},name:${navLink.name},text:${navLink.text}}`);
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
