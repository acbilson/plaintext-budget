import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IPTBFile } from '../../shared/interfaces/ptbfile';
import { IPTBFolder } from '../../shared/interfaces/ptbfolder';
import { IFileFolders } from '../../shared/interfaces/file-folders';
import { TransformService } from '../transform/transform.service';
import { IFileSchema } from '../../shared/interfaces/file-schema';
import { LoggingService } from '../logging/logging.service';


@Injectable()
export class FileService {

  httpOptions: object;
  baseUrl: URL;
  private fileSchema: IFileSchema;
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

  async getFileSchema(): Promise<IFileSchema> {

    // only retrieves file schema once
    if (this.fileSchema === undefined) {
      const url = new URL('api/Folder/GetFileSchema', this.baseUrl.href);
      try {
        this.fileSchema = await this.http.get<IFileSchema>(url.href).toPromise();
        this.logger.logInfo(this.context, 'retrieves file schema from server');
        console.log(this.fileSchema);
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
