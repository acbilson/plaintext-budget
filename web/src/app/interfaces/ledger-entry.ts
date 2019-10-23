import { LedgerColumn } from './ledger-column';

export interface LedgerEntry {
  id: number;
  date: LedgerColumn;
  type: LedgerColumn;
  amount: LedgerColumn;
  subcategory: LedgerColumn;
  title: LedgerColumn;
  subject: LedgerColumn;
  locked: LedgerColumn;
}
