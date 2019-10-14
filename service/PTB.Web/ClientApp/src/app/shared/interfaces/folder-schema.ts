import { IColumnSchema } from './column-schema';

export interface IFolderSchema {
    'fileMask': string;
    'folder': string;
    'delimiter': string;
    'defaultFileName': string;
    'lineSize': number;
    'columns': IColumnSchema[];
}
