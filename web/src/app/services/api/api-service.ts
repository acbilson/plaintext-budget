import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TransformService } from 'app/services/transform/transform.service';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { LoggingService } from '../logging/logging.service';
import { BaseResponse } from 'app/interfaces/response/base-response';
import { reject } from 'q';

@Injectable()
export class ApiService {
  protected httpOptions: object;
  protected config: ServiceConfig;
  protected logger: LoggingService;
  protected context: string;

  constructor(
    protected http: HttpClient,
    protected configService: ConfigService,
    protected loggingService: LoggingService
  ) {
    this.http = http;
    this.configService = configService;
    this.logger = loggingService;
    this.config = this.configService.getConfig();
  }

  setLoggingContext(context: string) {
    this.context = context;
  }

  async baseRead<T extends BaseResponse>(action: string): Promise<T> {
    const url = new URL(action, this.config.apiUrl.href);

    const response: T = await this.http
      .get<T>(url.href)
      .toPromise()
      .then(
        res => {
          if (!res.success) {
            this.logger.logDebug(this.context, res.message);
            reject(res.message);
          }
          return res;
        },
        err => {
          this.logger.logError(this.context, err);
          return err;
        }
      );

    return response;
  }
  async baseUpdate(action: string, body: object): Promise<BaseResponse> {
    const url = new URL(action, this.config.apiUrl.href);

    const response = await this.http
      .put<BaseResponse>(url.href, body, this.config.httpOptions)
      .toPromise()
      .then(
        res => {
          if (!res.success) {
            this.logger.logDebug(this.context, res.message);
            reject(res.message);
          }
          return res;
        },
        err => {
          this.logger.logError(this.context, err);
          return err;
        }
      );

    return response;
  }
}
