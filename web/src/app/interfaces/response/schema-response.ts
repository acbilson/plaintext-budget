import { SchemaRef } from 'app/interfaces/schema-ref';
import { FileSchema } from 'app/interfaces/file-schema';
import { ReportSchema } from 'app/interfaces/report-schema';
import { BaseResponse } from 'app/interfaces/response/base-response';

export interface SchemaResponse extends BaseResponse {
  files: FileSchema[];
  reports: ReportSchema[];
}
