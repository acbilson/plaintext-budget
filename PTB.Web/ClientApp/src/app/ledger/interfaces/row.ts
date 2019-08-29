import { IColumnSchema } from '../../shared/interfaces/column-schema';

export interface IRow {
  'index': number;
  'columns': IColumnSchema[];
}
