import { Injectable } from '@angular/core';
import { ILedger } from './ledger/ledger';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { IPTBFile } from './ledger/ptbfile';

@Injectable()
export class PtbService {

  httpOptions: object;

  constructor(private http: HttpClient) {
    this.http = http;
    this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json'}) };
     }

   updateLedger(ledger: ILedger): Promise<ILedger> {

    const uri = 'http://localhost:5000/api/Ledger/UpdateLedger';
    console.log(uri);
    return this.http.put<ILedger>(uri, ledger, {headers: new HttpHeaders().set('Content-Type', 'application/json')}).toPromise();
    }

    readLedgers(index: number, count: number): Promise<ILedger[]> {

      const uri = `http://localhost:5000/api/Ledger/ReadLedgers?startIndex=${index}&count=${count}`;
      console.log(uri);
      return this.http.get<ILedger[]>(uri).toPromise();
    }

    getLedgerFiles(): Promise<IPTBFile[]> {
      const uri = 'http://localhost:5000/api/File/GetLedgerFiles';
      console.log(uri);
      return this.http.get<IPTBFile[]>(uri).toPromise();
    }

    log(level: number, context: string, message: string): void {
      const uri = 'http://localhost:5000/api/Logging/Log';
      const logMessage = {level: level, context: context, message: message};
      console.log(uri);
      this.http.post(uri, logMessage, {headers: new HttpHeaders().set('Content-Type', 'application/json')}).toPromise();
    }
}
