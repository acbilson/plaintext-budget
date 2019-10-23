import { Injectable } from '@angular/core';
import { LedgerEntry } from 'app/interfaces/ledger-entry';
import { LedgerColumn } from 'app/interfaces/ledger-column';
import { ColumnSchema } from 'app/interfaces/column-schema';
import { Row } from 'app/interfaces/row';
import { LoggingService } from 'app/services/logging/logging.service';
import { BudgetColumn } from 'app/interfaces/budget-column';
import { BudgetEntryType } from 'app/interfaces/budget-entry-type';
import { BudgetEntry } from 'app/interfaces/budget-entry';
import { forEach } from '@angular/router/src/utils/collection';

@Injectable()
export class TransformService {
  private context: string;

  constructor(private logger: LoggingService) {
    this.logger = logger;
    this.context = 'transform.service';
  }

  rowsToLedgerEntries(
    rows: Row[],
    ledgerSchema: ColumnSchema[]
  ): LedgerEntry[] {
    this.logger.logInfo(this.context, 'transforming rows to ledger entries');
    const ledgers: LedgerEntry[] = [];

    rows.forEach(row => {
      const index = this.getLedgerColumnByName(
        row.values,
        ledgerSchema,
        'index'
      ).value;

      const ledger: LedgerEntry = {
        id: parseInt(index, 10),
        date: this.getLedgerColumnByName(row.values, ledgerSchema, 'date'),
        amount: this.getLedgerColumnByName(row.values, ledgerSchema, 'amount'),
        type: this.getLedgerColumnByName(row.values, ledgerSchema, 'type'),
        subcategory: this.getLedgerColumnByName(
          row.values,
          ledgerSchema,
          'subcategory'
        ),
        title: this.getLedgerColumnByName(row.values, ledgerSchema, 'title'),
        subject: this.getLedgerColumnByName(
          row.values,
          ledgerSchema,
          'subject'
        ),
        locked: this.getLedgerColumnByName(row.values, ledgerSchema, 'locked')
      };

      ledgers.push(ledger);
    });

    // this.logger.logDebug(this.context, JSON.stringify(ledgers[0]));
    return ledgers;
  }

  getLedgerColumnByName(
    values: Array<string>,
    ledgerSchema: ColumnSchema[],
    name: string
  ): LedgerColumn {
    const columnSchema = ledgerSchema.find(
      col => col.name.toLowerCase() === name
    );
    const value = values[columnSchema.index];

    return {
      name: columnSchema.name,
      value: value,
      index: columnSchema.index,
      offset: columnSchema.offset,
      size: columnSchema.size,
      editable: columnSchema.editable
    };
  }

  ledgerToRow(ledger: LedgerEntry): Row {
    this.logger.logInfo(this.context, 'transforming ledgers entries to rows');

    const row: Row = {
      id: ledger.id,
      link: null,
      values: [
        ledger.date.value,
        ledger.amount.value,
        ledger.type.value,
        ledger.subcategory.value,
        ledger.subject.value,
        ledger.locked.value,
        ledger.title.value
      ]
    };
    return row;
  }

  rowsToBudgetEntries(
    rows: Row[],
    columnSchemas: ColumnSchema[]
  ): BudgetEntry[] {
    if (!rows) {
      this.logger.logError(this.context, 'No rows were supplied for transform');
    }
    if (!columnSchemas || columnSchemas.length === 0) {
      this.logger.logError(
        this.context,
        'No column schemas were supplied for transform'
      );
    }

    this.logger.logInfo(this.context, 'transforming rows to budget entries');
    const budgetEntries: BudgetEntry[] = [];

    rows.forEach(row => {
      const columns: BudgetColumn[] = this.getColumns(row, columnSchemas);

      const entry: BudgetEntry = {
        id: row.id,
        index: columns.find(col => col.name === 'Index'),
        category: columns.find(col => col.name === 'Category'),
        subcategory: columns.find(col => col.name === 'Subcategory'),
        amount: columns.find(col => col.name === 'Amount'),
        type: this.getEntryType(columns)
      };

      budgetEntries.push(entry);
    });

    return budgetEntries;
  }

  getColumns(row: Row, columnSchemas: ColumnSchema[]): BudgetColumn[] {
    const columns: BudgetColumn[] = [];

    columnSchemas.forEach(sch => {
      const column: BudgetColumn = {
        index: row[sch.index],
        name: sch.name,
        editable: sch.editable,
        offset: sch.offset,
        size: sch.size,
        value: row.values[sch.index]
      };

      columns.push(column);
    });

    return columns;
  }

  getEntryType(columns: BudgetColumn[]): BudgetEntryType {
    if (columns.find(col => col.name === 'Category').value) {
      return BudgetEntryType.Header;
    } else {
      return BudgetEntryType.Row;
    }
  }
}
