import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LogMessage } from 'app/services/logging/log-message';
import { LoggingLevel } from 'app/services/logging/logging-level';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { BaseResponse } from 'app/interfaces/response/base-response';
import { reject } from 'q';

@Injectable()
export class LoggingService {
  config: ServiceConfig;

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.http = http;
    this.configService = configService;
    this.config = null;
  }

  private getLevelName(level: LoggingLevel): string {
    let levelName = null;
    switch (level) {
      case LoggingLevel.Info:
        levelName = 'Info';
        break;

      case LoggingLevel.Debug:
        levelName = 'DBug';
        break;

      case LoggingLevel.Warning:
        levelName = 'Warn';
        break;

      case LoggingLevel.Error:
        levelName = 'Erro';
        break;

      default:
        break;
    }

    return levelName;
  }

  private async readConfig() {
    if (!this.config) {
      console.log('reading config from logging-service');
      await this.configService.getConfig().then(conf => (this.config = conf));
    }
  }

  private async log(level: LoggingLevel, context: string, message: string) {
    const now = new Date().getTime();

    const logMessage: LogMessage = {
      timestamp: now.toString(),
      level: level,
      context: context,
      message: message
    };

    const levelName = this.getLevelName(level);
    console.log(`${levelName}-${context}: ${message}`);

    const url = new URL('api/log', this.config.apiUrl);

    this.http
      .post<BaseResponse>(url.href, logMessage, this.config.httpOptions)
      .toPromise()
      .then(
        res => {
          if (res.success === false) {
            console.log(`logging was unsuccessful with this message: ${res.message}`);
            reject(res.message);
          }
          return res;
        },
        error => {
            console.log(`logging errored with this message: ${error}`);
          return error;
        }
      );
  }

  async logInfo(context: string, message: string) {
    await this.readConfig();
    if (this.config.loggingLevel <= LoggingLevel.Info) {
      this.log(LoggingLevel.Info, context, message);
    }
  }

  async logDebug(context: string, message: string) {
    await this.readConfig();
    if (this.config.loggingLevel <= LoggingLevel.Debug) {
      this.log(LoggingLevel.Debug, context, message);
    }
  }

  async logWarning(context: string, message: string) {
    await this.readConfig();
    if (this.config.loggingLevel <= LoggingLevel.Warning) {
      this.log(LoggingLevel.Warning, context, message);
    }
  }

  async logError(context: string, message: string) {
    await this.readConfig();
    if (this.config.loggingLevel <= LoggingLevel.Error) {
      this.log(LoggingLevel.Error, context, message);
    }
  }
}
