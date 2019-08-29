import { Injectable } from '@angular/core';
import { ILedgerEntry } from '../../ledger/interfaces/ledger-entry';
import { ILedgerColumn } from '../../ledger/interfaces/ledger-column';
import { IColumn } from '../../ledger/interfaces/column';
import { IRow } from '../../ledger/interfaces/row';
import { LoggingService } from '../logging/logging.service';

@Injectable()
export class TransformService {

  private context: string;

  constructor(private logger: LoggingService) {
    this.logger = logger;
    this.context = 'transform.service';
   }

    rowsToLedgerEntries(rows: IRow[]): ILedgerEntry[] {

      this.logger.logInfo(this.context, 'transforming rows to ledger entries');
        const ledgers: ILedgerEntry[] = [];

        rows.forEach((row) => {
          const ledger: ILedgerEntry = {
            'index': row.index,
            'date': this.getLedgerColumnByName(row.columns, 'date'),
            'amount': this.getLedgerColumnByName(row.columns, 'amount'),
            'type': this.getLedgerColumnByName(row.columns, 'type'),
            'subcategory': this.getLedgerColumnByName(row.columns, 'subcategory'),
            'title': this.getLedgerColumnByName(row.columns, 'title'),
            'subject': this.getLedgerColumnByName(row.columns, 'subject'),
            'locked': this.getLedgerColumnByName(row.columns, 'locked')
          };

          ledgers.push(ledger);
          });

          // this.logger.logDebug(this.context, JSON.stringify(ledgers[0]));
          return ledgers;
    }

    getLedgerColumnByName(columns: IColumn[], name: string): ILedgerColumn {

        const column = columns.find(col => col.columnName.toLowerCase() === name);

        return {
          name: column.columnName,
          value: column.columnValue,
          index: column.index,
          offset: column.offset,
          size: column.size,
          editable: column.editable
        };
    }

    ledgerToRow(ledger: ILedgerEntry): IRow {

      this.logger.logInfo(this.context, 'transforming ledgers entries to rows');

      const row: IRow = {
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
      };
        return row;
      }

    getColumn(column: ILedgerColumn): IColumn {
      const newColumn: IColumn = {
        columnName: column.name,
        columnValue: column.value,
        index: column.index,
        offset: column.offset,
        size: column.size,
        editable: column.editable
      };
      return newColumn;
    }
}
