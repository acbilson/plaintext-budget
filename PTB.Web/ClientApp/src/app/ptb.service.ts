import { Injectable } from '@angular/core';
import { ILedger } from './ledger/ledger';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class PtbService {

  constructor(private http: HttpClient) {
    this.http = http;
     }

   readLedgers(index: number, count: number): Observable<ILedger[]> {


    //const uri = 'http://localhost:5000/' + 'api/Ledger/ReadLedgers' + '?startIndex=' + startIndex + '&count=' + ledgerCount;
    //http.get<ILedger[]>(uri).subscribe(result => { this.ledgers = result; }, error => console.error(error));
    
    const uri = 'http://localhost:5000/' + 'api/Ledger/ReadLedgers' + '?startIndex=' + index + '&count=' + count;
    console.log(uri);
    return this.http.get<ILedger[]>(uri);
    }
}
