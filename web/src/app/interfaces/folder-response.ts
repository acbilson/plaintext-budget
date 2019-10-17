import { SchemaRef } from './schema-ref';
import { Folder } from './folder';

export interface Folders {
    'schema': SchemaRef;
    'folders': Folder[];
}