import { IPTBFile } from './ptbfile';

export interface IPTBFolder {

  'defaultFileName': string;
  'name': string;
  'files': IPTBFile[];
}
