import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IFileFolders } from '../../shared/interfaces/file-folders';
import { TransformService } from '../transform/transform.service';

@Injectable()
export class BudgetService {

  httpOptions: object;
  baseUrl: URL;

  constructor(private http: HttpClient, private transform: TransformService) {
    this.http = http;
    this.transform = transform;
    this.baseUrl = new URL('http://localhost:5000');
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
