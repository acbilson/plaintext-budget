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
import { ApiService } from '../api/api-service';

@Injectable()
export class SchemaService extends ApiService {
  constructor(
    protected http: HttpClient,
    protected configService: ConfigService,
    protected loggingService: LoggingService
  ) {
    super(http, configService, loggingService);
    this.setLoggingContext('schema-service');
  }

  async read(): Promise<SchemaResponse> {
    const response = await this.baseRead<SchemaResponse>('api/schema');
    return response;
  }
}
