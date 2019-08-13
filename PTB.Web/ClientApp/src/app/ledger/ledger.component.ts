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

    http.get<ILedger[]>(baseUrl + 'api/Ledger/ReadAllLedgers').subscribe(result => { this.ledgers = result; }, error => console.error(error));

    const ledger: ILedger = {
      "date": "19-01-01",
      "type": "D",
      "amount": "10.80",
      "subcategory": "",
      "title": "mytesttitle",
      "location": "001284",
      "locked": "0"
      };

    this.ledgers = [ledger];

  };

  ngOnInit() {
  }

}

interface ILedger {
  "date": string;
  "type": string;
  "amount": string;
  "subcategory": string;
  "title": string;
  "location": string;
  "locked": string;
}
