import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class BuildingsService {
  private apiUrl = '/api/buildings';

  constructor(private http: HttpClient) {}

  getBuildings(params?: any): Observable<any> {
    return this.http.get(this.apiUrl, { params });
  }

  getBuildingById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  getBuildingByCode(code: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/code/${code}`);
  }

  getBuildingDetailsByCode(code: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/code/${code}/details`);
  }

  getBuildingsByStatus(status: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/status/${status}`);
  }

  createBuilding(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  updateBuilding(code: string, data: any): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${code}`, data);
  }

  deleteBuilding(code: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${code}`);
  }

  getApartmentsByBuilding(code: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${code}/apartments`);
  }

  getProfitabilityByBuilding(code: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/code/${code}/profitability`);
  }

  getProfitabilityByLocation(params?: any): Observable<any> {
    return this.http.get(`${this.apiUrl}/profitability-by-location`, { params });
  }
}
