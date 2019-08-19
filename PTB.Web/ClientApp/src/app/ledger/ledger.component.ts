import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-ledger',
  templateUrl: './ledger.component.html',
  styleUrls: ['./ledger.component.css']
})
export class LedgerComponent implements OnInit {
  public ledgers: ILedger[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    const startIndex = 0;
    const ledgerCount = 25;
    const uri = 'http://localhost:5000/' + 'api/Ledger/ReadLedgers' + '?startIndex=' + startIndex + '&count=' + ledgerCount;
    http.get<ILedger[]>(uri).subscribe(result => { this.ledgers = result; }, error => console.error(error));

    // this is dummy data while the subscription loads
    const ledger: ILedger = {
      "index": "0",
      "date": "19-01-01",
      "type": "D",
      "amount": "10.80",
      "subcategory": "",
      "title": "mytesttitle",
      "subject": "",
      "location": "001284",
      "locked": "0"
      };

    this.ledgers = [ledger];
  };

  ngOnInit() {
  }
}

interface ILedger {
  "index": string;
  "date": string;
  "type": string;
  "amount": string;
  "subcategory": string;
  "title": string;
  "subject": string;
  "location": string;
  "locked": string;
}
