import { File } from './file';

export interface Folder {
    'folderName': string;
    'fileType': string;
    'defaultFileName': string;
    'files': File[];
}
