import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TransformService } from 'app/services/transform/transform.service';
import { LoggingService } from 'app/services/logging/logging.service';

import { FileSchema } from 'app/interfaces/file-schema';
import { ReportSchema } from 'app/interfaces/report-schema';
import { FolderResponse } from 'app/interfaces/response/folder-response';
import { Folder } from 'app/interfaces/folder';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';

@Injectable()
export class FolderService {
  httpOptions: object;
  config: ServiceConfig;
  public fileSchema: FileSchema[];
  public reportSchema: ReportSchema[];
  private context: string;

  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private transform: TransformService,
    private logger: LoggingService
  ) {
    this.http = http;
    this.transform = transform;
    this.logger = logger;
    this.context = 'file-service';
    this.configService = configService;
    this.config = this.configService.getConfig();
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      })
    };
  }

  read(): Promise<Folder[]> {
    const url = new URL('api/folder', this.config.apiUrl.href);
    return this.http
      .get<FolderResponse>(url.href)
      .toPromise()
      .then((response: FolderResponse) => {
        return response.folders;
      });
  }
}
