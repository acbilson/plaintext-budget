import { Injectable } from '@angular/core';
import { ILedgerEntry, IColumn, IRow } from '../ledger/ledger';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { IPTBFile, IFileFolders } from '../ledger/ptbfile';
import { Observable } from 'rxjs/Observable';
import { PTBTransformService } from './ptb-transform.service';


@Injectable()
export class PtbService {

  httpOptions: object;
  baseUrl: URL;

  constructor(private http: HttpClient, private transform: PTBTransformService) {
    this.http = http;
    this.transform = transform;
    this.baseUrl = new URL('http://localhost:5000');
    this.httpOptions = { 
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      }) };
     }

   updateLedger(ledger: ILedgerEntry): Promise<ILedgerEntry> {
    const url = new URL('api/Ledger/Update', this.baseUrl);
    return this.http.put<ILedgerEntry>(url.href, ledger, this.httpOptions).toPromise();
    }

    readLedgers(fileName: string, index: number, count: number): Promise<ILedgerEntry[]> {
      const url = new URL(`api/Ledger/Read?fileName=${fileName}&startIndex=${index}&count=${count}`, this.baseUrl);
      return this.http.get<IRow[]>(url.href)
      .pipe(
        map((ledgers: IRow[]) => {
          return this.transform.rowToLedger(ledgers);
        })
      ).toPromise();
    }

    getFileFolders(): Promise<IFileFolders> {
      const url = new URL('api/Folder/GetFileFolders', this.baseUrl);
      return this.http.get<IFileFolders>(url.href).toPromise();
    }
}
