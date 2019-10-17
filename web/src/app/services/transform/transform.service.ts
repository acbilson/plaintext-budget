import { Injectable } from '@angular/core';
import { ILedgerEntry } from '../../ledger/interfaces/ledger-entry';
import { ILedgerColumn } from '../../ledger/interfaces/ledger-column';
import { IColumnSchema } from '../../shared/interfaces/column-schema';
import { IRow } from '../../ledger/interfaces/row';
import { LoggingService } from '../logging/logging.service';
import { Row } from 'app/interfaces/row';
import { ColumnSchema } from 'app/interfaces/column-schema';

@Injectable()
export class TransformService {

  private context: string;

  constructor(private logger: LoggingService) {
    this.logger = logger;
    this.context = 'transform.service';
  }

  rowsToLedgerEntries(rows: Row[], ledgerSchema: ColumnSchema[]): ILedgerEntry[] {

    this.logger.logInfo(this.context, 'transforming rows to ledger entries');
    const ledgers: ILedgerEntry[] = [];

    rows.forEach((row) => {

      const index = this.getLedgerColumnByName(row.values, ledgerSchema, 'index').value;

      const ledger: ILedgerEntry = {

        'index': parseInt(index, 10),
        'date': this.getLedgerColumnByName(row.values, ledgerSchema, 'date'),
        'amount': this.getLedgerColumnByName(row.values, ledgerSchema, 'amount'),
        'type': this.getLedgerColumnByName(row.values, ledgerSchema, 'type'),
        'subcategory': this.getLedgerColumnByName(row.values, ledgerSchema, 'subcategory'),
        'title': this.getLedgerColumnByName(row.values, ledgerSchema, 'title'),
        'subject': this.getLedgerColumnByName(row.values, ledgerSchema, 'subject'),
        'locked': this.getLedgerColumnByName(row.values, ledgerSchema, 'locked')
      };

      ledgers.push(ledger);
    });

    // this.logger.logDebug(this.context, JSON.stringify(ledgers[0]));
    return ledgers;
  }

  getLedgerColumnByName(values: Array<string>, ledgerSchema: ColumnSchema[], name: string): ILedgerColumn {

    const columnSchema = ledgerSchema.find(col => col.name.toLowerCase() === name);
    const value = values[columnSchema.index - 1];

    return {
      name: columnSchema.name,
      value: value,
      index: columnSchema.index,
      offset: columnSchema.offset,
      size: columnSchema.size,
      editable: columnSchema.editable
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

  getColumn(column: ILedgerColumn): IColumnSchema {
    const newColumn: IColumnSchema = {
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
