import { Component, OnInit } from '@angular/core';
import { FileService } from '../services/file/file.service';
import { LoggingService } from '../services/logging/logging.service';
import { IPTBFile } from '../shared/interfaces/ptbfile';
import { IFileFolders } from '../shared/interfaces/file-folders';
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

  private context: string;

  constructor(private fileService: FileService, private logger: LoggingService) {
    this.fileService = fileService;
    this.logger = logger;
    this.fileFolders = null;
    this.ledgerLinks = [];

    this.context = 'nav-menu';
  }

  ngOnInit() {

    this.getFileFolders()
      .then(fileFolders => {
        this.getDefaultName(fileFolders);
        this.generateNavLinks(fileFolders.ledgerFolder.files);
      })
      .catch(error => this.logger.logError(this.context, error));
  }

  getFileFolders(): Promise<IFileFolders> {

    return this.fileService.getFileFolders()
      .then(fileFolders => this.fileFolders = fileFolders);
  }

  getDefaultName(fileFolders: IFileFolders): void {

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
