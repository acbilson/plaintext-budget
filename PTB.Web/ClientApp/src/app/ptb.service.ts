import { Injectable } from '@angular/core';
import { ILedger } from './ledger/ledger';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { IPTBFile } from './ledger/ptbfile';

@Injectable()
export class PtbService {

  httpOptions: object;

  constructor(private http: HttpClient) {
    this.http = http;
    this.httpOptions = { headers: new HttpHeaders({'Content-Type': 'application/json'}) };
     }

   updateLedger(ledger: ILedger): void {

    const uri = 'http://localhost:5000/api/Ledger/UpdateLedger';
    console.log(uri);
    this.http.put<ILedger>(uri, ledger, {headers: new HttpHeaders().set('Content-Type', 'application/json')})
    .subscribe(
      data => { console.log(`Success! ${data}`); }, 
      error => { console.log(error); }
      );
    }

    readLedgers(index: number, count: number) : Observable<ILedger[]> {

      const uri = `http://localhost:5000/api/Ledger/ReadLedgers?startIndex=${index}&count=${count}`;
      console.log(uri);
      return this.http.get<ILedger[]>(uri);
    }

    getLedgerFiles() : Observable<IPTBFile[]> {
      const uri = 'http://localhost:5000/api/File/GetLedgerFiles';
      return this.http.get<IPTBFile[]>(uri);
    }
}
