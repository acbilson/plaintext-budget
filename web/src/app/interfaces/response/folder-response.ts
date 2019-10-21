import { SchemaRef } from 'app/interfaces/schema-ref';
import { Folder } from 'app/interfaces/folder';
import { BaseResponse } from 'app/interfaces/response/base-response';

export interface FolderResponse extends BaseResponse {
  schema: SchemaRef;
  folders: Folder[];
}
