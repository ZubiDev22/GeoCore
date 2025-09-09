import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RentalsService {
  constructor(private http: HttpClient) {}

  getRentals(params?: any): Observable<any> {
    return this.http.get('/api/rentals', { params });
  }

  getRentalById(id: number): Observable<any> {
    return this.http.get(`/api/rentals/${id}`);
  }
}
