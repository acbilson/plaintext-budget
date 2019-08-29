import { Injectable } from '@angular/core';
import { ILedgerEntry, IColumn, IRow } from '../ledger/interfaces/ledger';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { IPTBFile, IFileFolders } from '../ledger/interfaces/ptbfile';
import { Observable } from 'rxjs/Observable';
import { PtbTransformService } from './ptb-transform.service';


@Injectable()
export class PtbService {

  httpOptions: object;
  baseUrl: URL;

  constructor(private http: HttpClient, private transform: PtbTransformService) {
    this.http = http;
    this.transform = transform;
    this.baseUrl = new URL('http://localhost:5000');
    this.httpOptions = { 
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      }) };
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

    getFileFolders(): Promise<IFileFolders> {
      const url = new URL('api/Folder/GetFileFolders', this.baseUrl.href);
      return this.http.get<IFileFolders>(url.href).toPromise();
    }
}
