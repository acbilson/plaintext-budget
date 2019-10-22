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
  level: LoggingLevel;
  context: string;

  constructor(private http: HttpClient, private configService: ConfigService) {
    this.http = http;
    this.configService = configService;
    this.config = this.configService.getConfig();
    this.context = 'log-service';
    this.level = this.config.loggingLevel;
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

    const url = new URL('api/log', this.config.apiUrl.href);

    const response = await this.http
      .post<BaseResponse>(url.href, logMessage, this.config.httpOptions)
      .toPromise()
      .then(
        res => {
          if (!res.success) {
            console.log(this.context, res.message);
            reject(res.message);
          }
          return res;
        },
        error => {
          console.log(this.context, error);
          return error;
        }
      );
  }

  async logInfo(context: string, message: string) {
    if (this.level <= LoggingLevel.Info) {
      this.log(LoggingLevel.Info, context, message);
    }
  }

  async logDebug(context: string, message: string) {
    if (this.level <= LoggingLevel.Debug) {
      this.log(LoggingLevel.Debug, context, message);
    }
  }

  async logWarning(context: string, message: string) {
    if (this.level <= LoggingLevel.Warning) {
      this.log(LoggingLevel.Warning, context, message);
    }
  }

  async logError(context: string, message: string) {
    if (this.level <= LoggingLevel.Error) {
      this.log(LoggingLevel.Error, context, message);
    }
  }
}
