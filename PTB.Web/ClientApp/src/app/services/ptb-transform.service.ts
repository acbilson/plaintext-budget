import { Injectable } from '@angular/core';
import { ILedgerEntry, IColumn, IRow, ILedgerColumn } from '../ledger/ledger';
import { repeatWhen } from 'rxjs/operators';

@Injectable()
export class PtbTransformService {

  constructor() {
    
  }

    rowsToLedgerEntries(rows: IRow[]): ILedgerEntry[] {

        var ledgers: ILedgerEntry[] = [];
        
        rows.forEach((row) => {
          const ledger: ILedgerEntry = {
            "index": row.index,
            "date": this.getLedgerColumnByName(row.columns, 'date'),
            "amount": this.getLedgerColumnByName(row.columns, 'amount'),
            "type": this.getLedgerColumnByName(row.columns, 'type'),
            "subcategory": this.getLedgerColumnByName(row.columns, 'subcategory'),
            "title": this.getLedgerColumnByName(row.columns, 'title'),
            "subject": this.getLedgerColumnByName(row.columns, 'subject'),
            "locked": this.getLedgerColumnByName(row.columns, 'locked')
          };
  
          ledgers.push(ledger);
          })
  
          return ledgers;
    }
  
    getLedgerColumnByName(columns: IColumn[], name: string): ILedgerColumn {
  
        let column = columns.find(col => col.columnName.toLowerCase() == name);

        return {
          name: column.columnName,
          value: column.columnValue,
          index: column.index,
          offset: column.offset,
          size: column.size
        };
    }

    ledgerToRow(ledger: ILedgerEntry): IRow {

      let row: IRow = {
        index: ledger.index,
        columns: [
          this.getColumn(ledger.date),
          this.getColumn(ledger.amount),
          this.getColumn(ledger.type),
          this.getColumn(ledger.subcategory),
          this.getColumn(ledger.subject),
          this.getColumn(ledger.locked),
          this.getColumn(ledger.title)
        ]
      }
        return row;
      }

    getColumn(column: ILedgerColumn): IColumn {
      let newColumn: IColumn =
      {
        columnName: column.name,
        columnValue: column.value,
        index: column.index,
        offset: column.offset,
        size: column.size
      };
      return newColumn;
    }
}