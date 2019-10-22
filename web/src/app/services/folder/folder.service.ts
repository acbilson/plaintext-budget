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
import { ApiService } from '../api/api-service';

@Injectable()
export class FolderService extends ApiService {
  protected transform: TransformService;

  constructor(
    protected http: HttpClient,
    protected configService: ConfigService,
    protected loggingService: LoggingService
  ) {
    super(http, configService, loggingService);
    this.setLoggingContext('file-service');
  }

  read(): Promise<FolderResponse> {
    const response = this.baseRead<FolderResponse>('api/folder');
    return response;
  }
}
