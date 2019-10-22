import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TransformService } from 'app/services/transform/transform.service';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { LoggingService } from '../logging/logging.service';

@Injectable()
export class BudgetService {
  httpOptions: object;
  config: ServiceConfig;
  context: string;

  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private transform: TransformService,
    private logger: LoggingService
  ) {
    this.http = http;
    this.transform = transform;
    this.configService = configService;
    this.config = this.configService.getConfig();
    this.context = 'budget-service';
    this.logger = logger;
  }
}
