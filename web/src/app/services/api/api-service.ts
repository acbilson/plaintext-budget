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
  }

  setLoggingContext(context: string) {
    this.context = context;
  }

  async readConfig() {
    if (!this.config) {
      console.log('reading config from api-service');
      await this.configService.getConfig().then(conf => {
        this.config = conf;
      });
    }
  }

  async baseHealth(healthUrl: URL): Promise<string> {
    const response = await this.http
      .get<BaseResponse>(healthUrl.href)
      .toPromise()
      .then(
        res => {
          if (!res.success) {
            this.logger.logDebug(
              this.context,
              `${healthUrl.href} failed health check with message: ${res.message}`
            );

            return res;
          }
        },
        err => {
          this.logger.logError(
            this.context,
            `${healthUrl.href} errored in health check with message: ${err}`
          );
          return err;
        }
      )
      .catch(error => {
        this.logger.logError(
          this.context,
          `${healthUrl.href} errored in health check with message: ${error}`
        );
      });

    return response.success ? 'Healthy' : 'Unhealthy';
  }

  async baseRead<T extends BaseResponse>(action: string): Promise<T> {
    await this.readConfig();

    const url = new URL(action, this.config.apiUrl);

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
    await this.readConfig();

    const url = new URL(action, this.config.apiUrl);

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
