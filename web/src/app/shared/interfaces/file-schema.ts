import { IFolderSchema } from './folder-schema';

export interface IFileSchema {
    'ledger': IFolderSchema;
    'titleRegex': IFolderSchema;
    'categories': IFolderSchema;
}
