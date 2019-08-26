import { Injectable } from '@angular/core';
import { ILedger, ILedgerColumn } from './ledger/ledger';

@Injectable()
export class IncomingTransformModule {

    rowToLedger(rows: ILedger[]): ILedger[] {

        rows.forEach((row) => {

            console.log(row);

            row.date = this.getValueByName(row.columns, 'date');
            row.amount = this.getValueByName(row.columns, 'amount');
            row.type = this.getValueByName(row.columns, 'type');
            row.subcategory = this.getValueByName(row.columns, 'subcategory');
            row.title = this.getValueByName(row.columns, 'title');
            row.subject = this.getValueByName(row.columns, 'subject');
            row.locked = this.getValueByName(row.columns, 'locked');
          })

          return rows;
    }

    getValueByName(columns: ILedgerColumn[], name: string): string {

        return columns.find(col => col.columnName.toLowerCase() == name).columnValue;
    }
}