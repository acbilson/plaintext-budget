import { LoggingLevel } from "./logging-level";

export interface LogMessage {
    'timestamp': string,
    'message': string,
    'context': string,
    'level': LoggingLevel
}