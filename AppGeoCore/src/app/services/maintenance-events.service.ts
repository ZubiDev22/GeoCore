
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class MaintenanceEventsService {
  constructor(private http: HttpClient) {}

  // Listado de eventos con filtros
  getMaintenanceEvents(params?: any): Observable<any> {
    return this.http.get('/api/maintenance-events', { params }).pipe(
      catchError(this.handleError)
    );
  }

  // Detalle de evento
  getMaintenanceEventById(id: number): Observable<any> {
    return this.http.get(`/api/maintenance-events/${id}`).pipe(
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
