import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { TransformService } from 'app/services/transform/transform.service';
import { LedgerResponse } from 'app/interfaces/ledger-response';
import { SchemaResponse } from 'app/interfaces/schema-response';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { Row } from 'app/interfaces/row';
import { LedgerEntry } from 'app/interfaces/ledger-entry';

@Injectable()
export class LedgerService {
  httpOptions: object;
  config: ServiceConfig;

  constructor(
    private http: HttpClient,
    private configService: ConfigService,
    private transform: TransformService
  ) {
    this.http = http;
    this.transform = transform;
    this.configService = configService;
    this.config = this.configService.getConfig();
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      })
    };
  }

  updateLedger(ledger: LedgerEntry): Promise<Row> {
    const url = new URL('api/Ledger/Update', this.config.apiUrl.href);
    const row = this.transform.ledgerToRow(ledger);
    console.log(row);
    return this.http.put<Row>(url.href, row, this.httpOptions).toPromise();
  }

  read(fileName: string, id: number, count: number): Promise<LedgerEntry[]> {
    const url = new URL(
      `api/ledger?_fileName=${fileName}&_start=${id}&_limit=${count}`,
      this.config.apiUrl.href
    );

    return this.http
      .get<LedgerResponse>(url.href)
      .toPromise()
      .then((ledgerResponse: LedgerResponse) => {
        const schemaUrl = new URL(
          ledgerResponse.schema.link,
          this.config.apiUrl.href
        );
        return this.http
          .get<SchemaResponse>(schemaUrl.href)
          .pipe(
            map((schemaResponse: SchemaResponse) => {
              console.log('Schema response is:');
              console.log(schemaResponse);
              const ledgerSchema = schemaResponse.files.find(
                sch => sch.fileType === 'ledger'
              );
              return this.transform.rowsToLedgerEntries(
                ledgerResponse.rows,
                ledgerSchema.columns
              );
            })
          )
          .toPromise();
      });
  }
}
