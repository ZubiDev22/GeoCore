import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CashFlowsService {
  constructor(private http: HttpClient) {}

  getCashFlowsByBuilding(code: string): Observable<any> {
    return this.http.get(`/api/buildings/${code}/cashflows`);
  }
}
