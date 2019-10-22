import { Component, OnInit } from '@angular/core';
import { SchemaService } from 'app/services/schema/schema.service';
import { LoggingService } from 'app/services/logging/logging.service';
import { File } from 'app/interfaces/file';
import { NavLink } from 'app/nav-menu/interfaces/nav-link';
import { FolderService } from 'app/services/folder/folder.service';
import { Folder } from 'app/interfaces/folder';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  public folders: Folder[];
  private context: string;
  public isExpanded = false;

  constructor(
    private schemaService: SchemaService,
    private logger: LoggingService,
    private folderService: FolderService
  ) {
    this.folderService = folderService;
    this.schemaService = schemaService;
    this.logger = logger;
    this.folders = [];
    this.context = 'nav-menu';
  }

  ngOnInit() {
    this.getFolders()
      .then(
        folders => {
          this.folders = folders;
        },
        error => {
          this.logger.logError(this.context, error);
          return error;
        }
      )
      .catch(error => this.logger.logError(this.context, error));
  }

  async getFolders(): Promise<Folder[]> {
    let folders: Folder[];
    try {
      const response = await this.folderService.read();
      folders = response.folders;
    } catch (error) {
      this.logger.logError(this.context, error);
    }

    return folders;
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
