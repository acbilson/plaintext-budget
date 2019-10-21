import { SchemaRef } from 'app/interfaces/schema-ref';
import { Row } from 'app/interfaces/row';
import { BaseResponse } from 'app/interfaces/response/base-response';

export interface BudgetResponse extends BaseResponse {
  schema: SchemaRef;
  rows: Row[];
}
