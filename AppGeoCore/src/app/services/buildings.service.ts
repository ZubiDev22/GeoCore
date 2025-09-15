
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ProfitabilityDto, ProfitabilityByLocationDto } from '../models/reportes.model';

@Injectable({ providedIn: 'root' })
export class BuildingsService {
  private apiUrl = '/api/buildings';

  constructor(private http: HttpClient) {}

  // Listado paginado y filtrado de edificios
  getBuildings(params?: any): Observable<any> {
    return this.http.get(this.apiUrl, { params }).pipe(
      catchError(this.handleError)
    );
  }

  // Detalle por ID
  getBuildingById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Detalle por código
  getBuildingByCode(code: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/code/${code}`).pipe(
      catchError(this.handleError)
    );
  }

  // Detalle extendido
  getBuildingDetailsByCode(code: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/code/${code}/details`).pipe(
      catchError(this.handleError)
    );
  }

  // Listado por estado
  getBuildingsByStatus(status: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/status/${status}`).pipe(
      catchError(this.handleError)
    );
  }

  // Crear edificio
  createBuilding(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data).pipe(
      catchError(this.handleError)
    );
  }

  // Editar edificio
  updateBuilding(code: string, data: any): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${code}`, data).pipe(
      catchError(this.handleError)
    );
  }

  // Eliminar edificio
  deleteBuilding(code: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${code}`).pipe(
      catchError(this.handleError)
    );
  }

  // Apartamentos por edificio
  getApartmentsByBuilding(code: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/${code}/apartments`).pipe(
      catchError(this.handleError)
    );
  }

  // Rentabilidad por edificio
  getProfitabilityByBuilding(code: string): Observable<ProfitabilityDto> {
    return this.http.get<ProfitabilityDto>(`${this.apiUrl}/code/${code}/profitability`).pipe(
      catchError(this.handleError)
    );
  }

  // Rentabilidad por localización
  getProfitabilityByLocation(params?: any): Observable<ProfitabilityByLocationDto> {
    return this.http.get<ProfitabilityByLocationDto>(`${this.apiUrl}/profitability-by-location`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  // Manejo de errores para mostrar mensajes claros
  private handleError(error: HttpErrorResponse) {
    let msg = 'Error desconocido';
    if (error.error && error.error.message) {
      msg = error.error.message;
    } else if (error.status === 0) {
      msg = 'No se pudo conectar con el servidor';
    } else if (error.status >= 400) {
      msg = error.statusText || 'Error en la solicitud';
    }
    return throwError(() => msg);
  }
}
