import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ILedgerEntry } from '../../ledger/interfaces/ledger-entry';
import { IRow } from '../../ledger/interfaces/row';
import { IColumnSchema } from '../../shared/interfaces/column-schema';
import { TransformService } from '../transform/transform.service';

import { LedgerResponse } from 'app/interfaces/ledger-response';
import { SchemaResponse } from 'app/interfaces/schema-response';

@Injectable()
export class LedgerService {

  httpOptions: object;
  baseUrl: URL;

  constructor(private http: HttpClient, private transform: TransformService) {
    this.http = http;
    this.transform = transform;
    this.baseUrl = new URL('http://localhost:5000');
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      })
    };
  }

  updateLedger(ledger: ILedgerEntry): Promise<IRow> {
    const url = new URL('api/Ledger/Update', this.baseUrl.href);
    const row = this.transform.ledgerToRow(ledger);
    console.log(row);
    return this.http.put<IRow>(url.href, row, this.httpOptions).toPromise();
  }

  read(fileName: string, id: number, count: number): Promise<ILedgerEntry[]> {

    const url = new URL(`api/ledger?_fileName=${fileName}&_start=${id}&_limit=${count}`, this.baseUrl.href);

    return this.http.get<LedgerResponse>(url.href).toPromise()
      .then((ledgerResponse: LedgerResponse) => {

        const schemaUrl = new URL(ledgerResponse.schema.link, this.baseUrl.href);
        return this.http.get<SchemaResponse>(schemaUrl.href)
          .pipe(
            map((schemaResponse: SchemaResponse) => {
              console.log('Schema response is:');
              console.log(schemaResponse);
              const ledgerSchema = schemaResponse.files.find(sch => sch.fileType === 'ledger');
              return this.transform.rowsToLedgerEntries(ledgerResponse.rows, ledgerSchema.columns);
            })
          ).toPromise();
      });
  }
}
