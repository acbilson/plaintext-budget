import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TransformService } from 'app/services/transform/transform.service';
import { LoggingService } from 'app/services/logging/logging.service';
import { SchemaResponse } from 'app/interfaces/response/schema-response';

import { FileSchema } from 'app/interfaces/file-schema';
import { ReportSchema } from 'app/interfaces/report-schema';
import { SchemaRef } from 'app/interfaces/schema-ref';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { Folder } from 'app/interfaces/folder';
import { Observable } from 'rxjs/Observable';
import { resolve } from 'url';
import { reject } from 'q';

@Injectable()
export class SchemaService {
  private config: ServiceConfig;
  private context: string;

  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private logger: LoggingService
  ) {
    this.http = http;
    this.logger = logger;
    this.context = 'schema-service';
    this.configService = configService;
    this.config = this.configService.getConfig();
  }

  async read(): Promise<SchemaResponse> {
    const url = new URL('api/schema', this.config.apiUrl.href);
    const response = await this.http
      .get<SchemaResponse>(url.href)
      .toPromise()
      .then(
        res => {
          if (!res.success) {
            this.logger.logDebug(this.context, res.message);
            reject(res.message);
          }
          return res;
        },
        error => {
          this.logger.logError(this.context, error);
          return error;
        }
      );
    return response;
  }
}
