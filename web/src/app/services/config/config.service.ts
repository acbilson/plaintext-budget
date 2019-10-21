import { Injectable, SystemJsNgModuleLoaderConfig } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ServiceConfig } from 'app/interfaces/service-config';
import { LoggingLevel } from '../logging/logging-level';

@Injectable()
export class ConfigService {
  private config: ServiceConfig;

  constructor(private http: HttpClient) {
    this.http = http;
    this.config = {
      apiUrl: new URL('http://localhost:5000'),
      httpOptions: {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      })
    },
    loggingLevel: LoggingLevel.Info
    };
  }

  getConfig(): ServiceConfig {
    return this.config;
  }
}
