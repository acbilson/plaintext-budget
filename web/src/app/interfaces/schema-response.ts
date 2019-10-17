import { FileSchema } from './file-schema';
import { ReportSchema } from './report-schema';

export interface SchemaResponse {
    'files': FileSchema;
    'reports': ReportSchema;
}
