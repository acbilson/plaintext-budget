import { Injectable } from '@angular/core';
import { ILedgerEntry, IColumn, IRow, ILedgerColumn } from '../ledger/ledger';

@Injectable()
export class PTBTransformService {

  constructor() {
    
  }

    rowToLedger(rows: IRow[]): ILedgerEntry[] {

        var ledgers: ILedgerEntry[] = [];
        
        rows.forEach((row) => {
          const ledger: ILedgerEntry = {
            "index": row.index,
            "date": this.getColumnByName(row.columns, 'date'),
            "amount": this.getColumnByName(row.columns, 'amount'),
            "type": this.getColumnByName(row.columns, 'type'),
            "subcategory": this.getColumnByName(row.columns, 'subcategory'),
            "title": this.getColumnByName(row.columns, 'title'),
            "subject": this.getColumnByName(row.columns, 'subject'),
            "locked": this.getColumnByName(row.columns, 'locked')
          };
  
          ledgers.push(ledger);
          })
  
          return ledgers;
    }
  
    getColumnByName(columns: IColumn[], name: string): ILedgerColumn {
  
        let column = columns.find(col => col.columnName.toLowerCase() == name);

        return {
          name: column.columnName,
          value: column.columnValue,
          index: column.index,
          offset: column.offset,
          size: column.size
        };
    }
}