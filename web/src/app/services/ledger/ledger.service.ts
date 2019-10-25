import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { TransformService } from 'app/services/transform/transform.service';
import { LedgerResponse } from 'app/interfaces/response/ledger-response';
import { SchemaResponse } from 'app/interfaces/response/schema-response';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { Row } from 'app/interfaces/row';
import { LedgerEntry } from 'app/interfaces/ledger-entry';
import { LoggingService } from '../logging/logging.service';
import { ApiService } from '../api/api-service';
import { BaseResponse } from 'app/interfaces/response/base-response';

@Injectable()
export class LedgerService extends ApiService {
  constructor(
    protected http: HttpClient,
    protected configService: ConfigService,
    protected transform: TransformService,
    protected loggingService: LoggingService
  ) {
    super(http, configService, loggingService);
    this.transform = transform;
    this.context = 'ledger-service';
  }

  async update(ledger: LedgerEntry): Promise<BaseResponse> {
    const row = this.transform.ledgerToRow(ledger);
    const response = await this.baseUpdate('api/ledger', row);
    return response;
  }

  async read(
    fileName: string,
    id: number,
    count: number
  ): Promise<LedgerEntry[]> {
    const response = this.baseRead<LedgerResponse>(
      `api/ledger?_fileName=${fileName}&_start=${id}&_limit=${count}`
    );

    const finalResponse = await response.then(
      res => {
        const schemaUrl = new URL(res.schema.link, this.config.apiUrl);
        return this.http
          .get<SchemaResponse>(schemaUrl.href)
          .pipe(
            map(
              schemaRes => {
                const ledgerSchema = schemaRes.files.find(
                  sch => sch.fileType === 'ledger'
                );
                return this.transform.rowsToLedgerEntries(
                  res.rows,
                  ledgerSchema.columns
                );
              },
              schemaError => {
                this.logger.logError(this.context, schemaError);
                return schemaError;
              }
            )
          )
          .toPromise();
      },
      error => {
        this.logger.logError(this.context, error);
        return error;
      }
    );
    return finalResponse;
  }
}
