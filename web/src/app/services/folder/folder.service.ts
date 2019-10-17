import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IPTBFile } from '../../shared/interfaces/ptbfile';
import { IPTBFolder } from '../../shared/interfaces/ptbfolder';
import { IFileFolders } from '../../shared/interfaces/file-folders';
import { TransformService } from '../transform/transform.service';
import { IFileSchema } from '../../shared/interfaces/file-schema';
import { LoggingService } from '../logging/logging.service';
import { SchemaResponse } from 'app/interfaces/schema-response';

import { FileSchema } from 'app/interfaces/file-schema';
import { ReportSchema } from 'app/interfaces/report-schema';
import { FolderResponse } from 'app/interfaces/folder-response';
import { Folder } from 'app/interfaces/folder';

@Injectable()
export class FolderService {

  httpOptions: object;
  baseUrl: URL;
  public fileSchema: FileSchema[];
  public reportSchema: ReportSchema[];
  private context: string;

  constructor(private http: HttpClient, private transform: TransformService, private logger: LoggingService) {
    this.http = http;
    this.transform = transform;
    this.logger = logger;
    this.context = 'file-service';
    this.baseUrl = new URL('http://localhost:5000');
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      })
    };
  }

  read(): Promise<Folder[]> {
    const url = new URL('api/folder', this.baseUrl.href);
    return this.http.get<FolderResponse>(url.href).toPromise()
      .then((response: FolderResponse) => {
        return response.folders;
      });
  }
}
