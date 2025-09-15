
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class ApartmentsService {
  constructor(private http: HttpClient) {}

  // Listado de apartamentos por edificio
  getApartmentsByBuilding(code: string): Observable<any> {
  return this.http.get(`${environment.apiUrl}/api/buildings/${code}/apartments`).pipe(
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
