import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ILedgerEntry } from '../../ledger/interfaces/ledger-entry';
import { IRow } from '../../ledger/interfaces/row';
import { IColumnSchema } from '../../shared/interfaces/column-schema';
import { TransformService } from '../transform/transform.service';

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

  readLedgers(fileName: string, index: number, count: number): Promise<ILedgerEntry[]> {
    const url = new URL(`api/Ledger/Read?fileName=${fileName}&startIndex=${index}&count=${count}`, this.baseUrl.href);
    return this.http.get<IRow[]>(url.href)
      .pipe(
        map((ledgers: IRow[]) => {
          return this.transform.rowsToLedgerEntries(ledgers);
        })
      ).toPromise();
  }
}
