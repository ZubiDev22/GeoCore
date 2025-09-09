import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class MaintenanceEventsService {
  constructor(private http: HttpClient) {}

  getMaintenanceEvents(params?: any): Observable<any> {
    return this.http.get('/api/maintenance-events', { params });
  }

  getMaintenanceEventById(id: number): Observable<any> {
    return this.http.get(`/api/maintenance-events/${id}`);
  }
}
