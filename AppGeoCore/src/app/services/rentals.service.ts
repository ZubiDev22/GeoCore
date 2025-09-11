
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class RentalsService {
  constructor(private http: HttpClient) {}

  // Listado de alquileres con filtros
  getRentals(params?: any): Observable<any> {
    return this.http.get('/api/rentals', { params }).pipe(
      catchError(this.handleError)
    );
  }

  // Detalle de alquiler
  getRentalById(id: number): Observable<any> {
    return this.http.get(`/api/rentals/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  // Comparativa de precios (lógica frontend, si no hay endpoint)
  getPriceComparison(rentals: any[], rental: any): { diff: number, percent: number } {
    if (!rentals || rentals.length === 0) return { diff: 0, percent: 0 };
    const prices = rentals.map(r => r.price).filter(p => typeof p === 'number');
    const avg = prices.reduce((a, b) => a + b, 0) / prices.length;
    const diff = rental.price - avg;
    const percent = avg ? (diff / avg) * 100 : 0;
    return { diff, percent };
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
