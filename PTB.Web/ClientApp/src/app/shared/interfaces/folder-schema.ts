import { IColumn } from '../../ledger/interfaces/column';

export interface IFolderSchema {
    'fileMask': string;
    'folder': string;
    'delimiter': string;
    'defaultFileName': string;
    'lineSize': number;
    'columns': IColumn[];
}
