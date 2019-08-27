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

     // adds log to queue to slow log posts
    async log(context: string, message: string) {
        const now = new Date().getTime();

        const logMessage: LogMessage = {
            timestamp: now.toString(), 
            level: this.level, 
            context: context, 
            message: message
        };

        const url = new URL('api/Logging/log', this.baseUrl);
            try {
                var response = await this.http.post(url.href, logMessage, this.httpOptions).toPromise();
            } catch (error) {
                console.log(error);
            }
    }
}
