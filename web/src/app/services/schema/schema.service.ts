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

@Injectable()
export class SchemaService {

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

  getFileFolders(): Promise<IFileFolders> {
    const url = new URL('api/Folder/GetFileFolders', this.baseUrl.href);
    return this.http.get<IFileFolders>(url.href).toPromise();
  }

  async readFileSchema(): Promise<FileSchema[]> {

    // only retrieves file schema once -- not true, but it's supposed to
    console.log(this.fileSchema);
    if (this.fileSchema === undefined) {
      const url = new URL('api/schema', this.baseUrl.href);

      try {
        const response = await this.http.get<SchemaResponse>(url.href).toPromise();

        this.fileSchema = response.files;
        this.reportSchema = response.reports;

        this.logger.logInfo(this.context, 'retrieves file schema from server');
        return this.fileSchema;
      } catch (error) {
        this.logger.logError(this.context, error);
      }
    } else {
      this.logger.logInfo(this.context, 'retrieves file schema from cache');
      return Promise.resolve(this.fileSchema);
    }
  }
}
