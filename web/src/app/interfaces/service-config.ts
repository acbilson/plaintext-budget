import { LoggingLevel } from 'app/services/logging/logging-level';

export interface ServiceConfig {
  apiUrl: URL;
  httpOptions: object;
  loggingLevel: LoggingLevel;
}
