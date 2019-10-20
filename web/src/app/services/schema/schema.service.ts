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
import { SchemaRef } from 'app/interfaces/schema-ref';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from '../config/config.service';

@Injectable()
export class SchemaService {

  httpOptions: object;
  baseUrl: URL;
  public fileSchema: FileSchema[];
  public reportSchema: ReportSchema[];
  private context: string;

  constructor(private http: HttpClient, private config: ConfigService, private transform: TransformService, private logger: LoggingService) {
    this.http = http;
    this.transform = transform;
    this.logger = logger;
    this.context = 'file-service';
    this.config = config;
    this.baseUrl = this.config.apiUrl;
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      })
    };
  }

  getFileFolders(): Promise<IFileFolders> {
    const url = new URL('api/Folder/GetFileFolders', this.baseUrl.href);
    return this.http.get<IFileFolders>(url.href).toPromise();
  }

  async readFileSchema(): Promise<FileSchema[]> {

      const url = new URL('api/schema', this.baseUrl.href);
      return this.http.get<SchemaResponse>(url.href).toPromise()
      .then((response: SchemaResponse) => {

        this.logger.logDebug(this.context, 'There were this many schemas in file:' + response.files);
        return response.files;
      });
  }
}
