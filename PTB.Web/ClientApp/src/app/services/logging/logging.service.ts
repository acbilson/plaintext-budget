import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {LogMessage } from './log-message';
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
      }) };
      this.level = LoggingLevel.Info;
     }

    private async log(context: string, message: string) {
        const now = new Date().getTime();

        const logMessage: LogMessage = {
            timestamp: now.toString(),
            level: this.level,
            context: context,
            message: message
        };

        console.log(`${this.level}-${context}: ${message}`);

        const url = new URL('api/Logging/log', this.baseUrl.href);
            try {
                const response = await this.http.post(url.href, logMessage, this.httpOptions).toPromise();
            } catch (error) {
                console.log(error);
            }
    }

    async logInfo(context: string, message: string) {

      if (this.level <= LoggingLevel.Info) {
        this.log(context, message);
      }
    }

    async logDebug(context: string, message: string) {

      if (this.level <= LoggingLevel.Debug) {
        this.log(context, message);
      }
    }

    async logWarning(context: string, message: string) {

      if (this.level <= LoggingLevel.Warning) {
        this.log(context, message);
      }
    }

    async logError(context: string, message: string) {

      if (this.level <= LoggingLevel.Error) {
        this.log(context, message);
      }
    }


}
