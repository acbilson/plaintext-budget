import { SchemaRef } from './schema-ref';
import { Folder } from './folder';

export interface FolderResponse {
    'schema': SchemaRef;
    'folders': Folder[];
}
