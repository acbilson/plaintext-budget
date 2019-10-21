import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TransformService } from 'app/services/transform/transform.service';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';

@Injectable()
export class BudgetService {
  httpOptions: object;
  config: ServiceConfig;

  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private transform: TransformService
  ) {
    this.http = http;
    this.transform = transform;
    this.configService = configService;
    this.config = this.configService.getConfig();
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      })
    };
  }
}
