
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { RentalDto, Result } from '../models/reportes.model';

@Injectable({ providedIn: 'root' })
export class RentalsService {
  constructor(private http: HttpClient) {}

  // Listado de alquileres con filtros
  getRentals(params?: any): Observable<Result<RentalDto[]>> {
  return this.http.get<Result<RentalDto[]>>(`${environment.apiUrl}/api/rentals`, { params }).pipe(
      catchError(this.handleError)
    );
  }

  // Detalle de alquiler
  getRentalById(id: string): Observable<Result<RentalDto>> {
  return this.http.get<Result<RentalDto>>(`${environment.apiUrl}/rentals/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Comparativa de precios (lógica frontend, si no hay endpoint)
  getPriceComparison(rentals: RentalDto[], rental: RentalDto): { diff: number, percent: number } {
    if (!rentals || rentals.length === 0) return { diff: 0, percent: 0 };
    const prices = rentals.map(r => r.Price).filter(p => typeof p === 'number');
    const avg = prices.reduce((a, b) => a + b, 0) / prices.length;
    const diff = rental.Price - avg;
    const percent = avg ? (diff / avg) * 100 : 0;
    return { diff, percent };
  }
  // Crear alquiler
  createRental(rental: RentalDto): Observable<Result<{ RentalId: string }>> {
  return this.http.post<Result<{ RentalId: string }>>(`${environment.apiUrl}/rentals`, rental).pipe(
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
