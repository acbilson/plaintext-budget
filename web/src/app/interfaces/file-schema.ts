import { Column } from './column';

export interface FileSchema {
    'fileType': string;
    'lineSize': number;
    'columns': Column[];
}
