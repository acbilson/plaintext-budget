import { IColumn } from './column';

export interface IRow {
  'index': number;
  'columns': IColumn[];
}
