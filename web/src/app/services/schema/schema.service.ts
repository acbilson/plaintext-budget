import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TransformService } from 'app/services/transform/transform.service';
import { LoggingService } from 'app/services/logging/logging.service';
import { SchemaResponse } from 'app/interfaces/schema-response';

import { FileSchema } from 'app/interfaces/file-schema';
import { ReportSchema } from 'app/interfaces/report-schema';
import { SchemaRef } from 'app/interfaces/schema-ref';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { Folder } from 'app/interfaces/folder';

@Injectable()
export class SchemaService {
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

  async readFileSchema(): Promise<FileSchema[]> {
    const url = new URL('api/schema', this.config.apiUrl.href);
    return this.http
      .get<SchemaResponse>(url.href)
      .toPromise()
      .then((response: SchemaResponse) => {
        this.logger.logDebug(
          this.context,
          'There were this many schemas in file:' + response.files
        );
        return response.files;
      });
  }
}
