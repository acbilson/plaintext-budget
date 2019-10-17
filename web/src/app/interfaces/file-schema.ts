import { ColumnSchema } from './column-schema';

export interface FileSchema {
    'fileType': string;
    'lineSize': number;
    'columns': ColumnSchema[];
}
