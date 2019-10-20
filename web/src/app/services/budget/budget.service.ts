import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IFileFolders } from '../../shared/interfaces/file-folders';
import { TransformService } from '../transform/transform.service';
import { ServiceConfig } from 'app/interfaces/service-config';
import { ConfigService } from '../config/config.service';

@Injectable()
export class BudgetService {

  httpOptions: object;
  baseUrl: URL;

  constructor(private http: HttpClient, private config: ConfigService, private transform: TransformService) {
    this.http = http;
    this.transform = transform;
    this.config = config;
    this.baseUrl = this.config.apiUrl;
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Access-Control-Allow-Origin': '*'
      })
    };
  }

  getFileFolders(): Promise<IFileFolders> {
    const url = new URL('api/Folder/GetFileFolders', this.baseUrl.href);
    return this.http.get<IFileFolders>(url.href).toPromise();
  }
}
