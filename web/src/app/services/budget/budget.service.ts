import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TransformService } from 'app/services/transform/transform.service';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from 'app/services/config/config.service';
import { LoggingService } from '../logging/logging.service';
import { ApiService } from '../api/api-service';
import { BudgetResponse } from 'app/interfaces/response/budget-response';
import { BudgetColumn } from 'app/interfaces/budget-column';
import { SchemaResponse } from 'app/interfaces/response/schema-response';
import { map } from 'rxjs/operators';
import { BudgetEntry } from 'app/interfaces/budget-entry';
import { BaseResponse } from 'app/interfaces/response/base-response';

@Injectable()
export class BudgetService extends ApiService {
  constructor(
    protected http: HttpClient,
    protected configService: ConfigService,
    protected loggingService: LoggingService,
    protected transform: TransformService
  ) {
    super(http, configService, loggingService);
    this.transform = transform;
    this.context = 'budget-service';
  }

  async read(fileName: string): Promise<BudgetEntry[]> {
    const response = this.baseRead<BudgetResponse>(
      `api/budget?_fileName=${fileName}`
    );

    const finalResponse = await response.then(
      res => {
        const schemaUrl = new URL(res.schema.link, this.config.apiUrl);
        this.logger.logInfo(
          this.context,
          `retrieving schema at ${schemaUrl.href}`
        );
        return this.http
          .get<SchemaResponse>(schemaUrl.href)
          .pipe(
            map(
              schemaRes => {
                const budgetSchema = schemaRes.reports.find(
                  sch => sch.fileType === 'budget'
                );
                const entries = this.transform.rowsToBudgetEntries(
                  res.rows,
                  budgetSchema.columns
                );
                this.logger.logInfo(
                  this.context,
                  `${entries.length} budget entries retrieved`
                );
                console.log(entries);
                return entries;
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

  async update(): Promise<BaseResponse> {
    this.logger.logInfo(this.context, 'updating budget');
    const response = { success: true, message: ''};
    // const response = await this.baseUpdate('api/budget', {});
    return response;
  }
}
