import { ColumnSchema } from './column-schema';

export interface ReportSchema {
    'fileType': string;
    'lineSize': number;
    'columns': ColumnSchema[];
}
