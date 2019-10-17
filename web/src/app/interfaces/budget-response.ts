import { SchemaRef } from './schema-ref';
import { Row } from './row';

export interface BudgetResponse {
    'schema': SchemaRef;
    'rows': Row[];
}