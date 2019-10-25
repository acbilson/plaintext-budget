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
      apiUrl: 'http://localhost:3000',
      httpOptions: {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Access-Control-Allow-Origin': '*'
        })
      },
      loggingLevel: LoggingLevel.Info
    };
  }

  async getConfig(): Promise<ServiceConfig> {
    const config = await this.http
      .get<ServiceConfig>('assets/app-config.json')
      .toPromise();

    if (config) {
      config.httpOptions = this.config.httpOptions;
      console.log(config);
      return config;
    } else {
      return this.config;
    }
  }
}
