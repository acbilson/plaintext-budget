import { Component, OnInit } from '@angular/core';
import { FileService } from '../services/file/file.service';
import { LoggingService } from '../services/logging/logging.service';
import { IPTBFile } from '../shared/interfaces/ptbfile';
import { IFileFolders } from '../shared/interfaces/file-folders';
import { INavLink } from './nav-link';
import { IFileSchema } from '../shared/interfaces/file-schema';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  public fileFolders: IFileFolders;
  public fileSchema: IFileSchema;

  // ledgers
  public defaultLedgerName: string;
  public ledgerLinks: INavLink[];

  // budget
  public defaultBudgetName: string;

  // logging
  private context: string;

  constructor(private fileService: FileService, private logger: LoggingService) {
    this.fileService = fileService;
    this.logger = logger;
    this.fileFolders = null;
    this.fileSchema = null;
    this.ledgerLinks = [];
    this.defaultBudgetName = 'budget';

    this.context = 'nav-menu';
  }

  ngOnInit() {

    this.getFileSchema().then(schema => this.fileSchema = schema);
    this.getFileFolders()
      .then(fileFolders => {
        this.getDefaultLedgerName(fileFolders);
        this.getDefaultBudgetName(fileFolders);
        this.generateNavLinks(fileFolders.ledgerFolder.files);
      }).catch(error => this.logger.logError(this.context, error));
  }

  async getFileSchema(): Promise<IFileSchema> {

    let fileSchema: IFileSchema;

    try {
      fileSchema = await this.fileService.getFileSchema();
    } catch (error) {
      this.logger.logError(this.context, error);
    }

    return fileSchema;
  }

  async getFileFolders(): Promise<IFileFolders> {

    let fileFolders: IFileFolders;
    try {
      fileFolders = await this.fileService.getFileFolders();
    } catch (error) {
      this.logger.logError(this.context, error);
    }

    return fileFolders;
  }

  getDefaultBudgetName(fileFolders: IFileFolders): void {

    // this.defaultBudgetName = fileFolders.budgetFolder.files.find(
    //   file => file.fileName === fileFolders.budgetFolder.defaultFileName + '.txt').shortName;
    this.defaultBudgetName = '19-01-01:31';

    this.logger.logInfo(this.context, `set default budget to ${this.defaultBudgetName}`);
  }


  getDefaultLedgerName(fileFolders: IFileFolders): void {

    this.defaultLedgerName = fileFolders.ledgerFolder.files.find(
      file => file.fileName === fileFolders.ledgerFolder.defaultFileName + '.txt').shortName;

    this.logger.logInfo(this.context, `set default ledger to ${this.defaultLedgerName}`);
  }

  generateNavLinks(files: IPTBFile[]): void {

    this.logger.logInfo(this.context, `creating ${files.length} links`);

    files.forEach(ledgerFile => {
      const navLink: INavLink = {
        'path': '/ledger',
        'name': ledgerFile.shortName,
        'text': ledgerFile.shortName
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
