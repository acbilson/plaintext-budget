import { SchemaRef } from './schema-ref';
import { Row } from './row';

export interface LedgerResponse {
    'schema': SchemaRef;
    'rows': Row[];
}
