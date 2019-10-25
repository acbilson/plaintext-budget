import { LoggingLevel } from 'app/services/logging/logging-level';

export interface ServiceConfig {
  apiUrl: string;
  httpOptions: object;
  loggingLevel: LoggingLevel;
}
