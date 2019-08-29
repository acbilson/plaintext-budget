import { ILedgerColumn } from './ledger-column';

export interface ILedgerEntry {
  'index': number;
  'date': ILedgerColumn;
  'type': ILedgerColumn;
  'amount': ILedgerColumn;
  'subcategory': ILedgerColumn;
  'title': ILedgerColumn;
  'subject': ILedgerColumn;
  'locked': ILedgerColumn;
}


