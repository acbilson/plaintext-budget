import { Injectable, SystemJsNgModuleLoaderConfig } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServiceConfig } from 'app/interfaces/service-config';

@Injectable()
export class ConfigService {
  private config: ServiceConfig;

  constructor(private http: HttpClient) {
    this.http = http;
    this.config = {
      apiUrl: new URL('http://localhost:5000')
    };
  }

  getConfig(): ServiceConfig {
    return this.config;
  }
}
