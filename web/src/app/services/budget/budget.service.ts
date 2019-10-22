import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TransformService } from 'app/services/transform/transform.service';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { LoggingService } from '../logging/logging.service';
import { ApiService } from '../api/api-service';

@Injectable()
export class BudgetService extends ApiService {
  constructor(
    protected http: HttpClient,
    protected configService: ConfigService,
    protected loggingService: LoggingService,
    protected transform: TransformService
  ) {
    super(http, configService, loggingService);
    this.transform = transform;
    this.context = 'budget-service';
  }
}
