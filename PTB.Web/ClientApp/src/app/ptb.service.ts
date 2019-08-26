import { Injectable } from '@angular/core';
import { ILedger, ILedgerColumn, IRow } from './ledger/ledger';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { IPTBFile, IFileFolders } from './ledger/ptbfile';
import { Observable } from 'rxjs/Observable';
import { IncomingTransformModule } from './incoming-transform';

@Injectable()
export class PtbService {

  httpOptions: object;
  baseUrl: string;

  constructor(private http: HttpClient) {
    this.http = http;
    this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json'}) };
     }

   updateLedger(ledger: ILedger): Promise<ILedger> {

    console.log(this.baseUrl);
    const uri = 'http://localhost:5000/api/Ledger/Update';
    console.log(uri);
    return this.http.put<ILedger>(uri, ledger, {headers: new HttpHeaders().set('Content-Type', 'application/json')}).toPromise();
    }

    readLedgers(fileName: string, index: number, count: number): Promise<ILedger[]> {

      const uri = `http://localhost:5000/api/Ledger/Read?fileName=${fileName}&startIndex=${index}&count=${count}`;
      console.log(uri);
      return this.http.get<IRow[]>(uri)
      .pipe(
        map((ledgers: IRow[]) => {

          return this.rowToLedger(ledgers);
        })
      )
      .toPromise();
    }

    getFileFolders(): Promise<IFileFolders> {
      const uri = 'http://localhost:5000/api/Folder/GetFileFolders';
      console.log(uri);
      return this.http.get<IFileFolders>(uri)
      .pipe(
        tap((fileFolders: IFileFolders) {
          console.log(fileFolders);
        }

        )
      )
      .toPromise();
    }

    log(level: number, context: string, message: string): void {
      const uri = 'http://localhost:5000/api/Logging/Log';
      const logMessage = {level: level, context: context, message: message};
      console.log(uri);
      this.http.post(uri, logMessage, {headers: new HttpHeaders().set('Content-Type', 'application/json')}).toPromise();
    }

    rowToLedger(rows: IRow[]): ILedger[] {

      var ledgers: ILedger[] = [];
      
      rows.forEach((row) => {
        const ledger: ILedger = {
          "index": row.index,
          "date": this.getValueByName(row.columns, 'date'),
          "amount": this.getValueByName(row.columns, 'amount'),
          "type": this.getValueByName(row.columns, 'type'),
          "subcategory": this.getValueByName(row.columns, 'subcategory'),
          "title": this.getValueByName(row.columns, 'title'),
          "subject": this.getValueByName(row.columns, 'subject'),
          "locked": this.getValueByName(row.columns, 'locked')
        };

        ledgers.push(ledger);
        })

        return ledgers;
  }

  getValueByName(columns: ILedgerColumn[], name: string): string {

      return columns.find(col => col.columnName.toLowerCase() == name).columnValue;
  }
}
