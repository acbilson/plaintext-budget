import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LogMessage } from './log-message';
import { LoggingLevel } from './logging-level';

@Injectable()
export class LoggingService {

  httpOptions: object;
  baseUrl: URL;
  level: LoggingLevel;

  constructor(private http: HttpClient) {
    this.http = http;
    this.baseUrl = new URL('http://localhost:5000');
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      })
    };
    this.level = LoggingLevel.Info;
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

    const url = new URL('api/Logging/log', this.baseUrl.href);
    try {
      const response = await this.http.post(url.href, logMessage, this.httpOptions).toPromise();
    } catch (error) {
      console.log(`logger errored in context: ${context} with message: ${message}`);
      console.log(error);
    }
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
