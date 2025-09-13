import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class GeolocationService {
  private apiKey = 'TU_API_KEY_AQUI'; // Reemplaza por tu API key
  private apiUrl = 'https://www.googleapis.com/geolocation/v1/geolocate';

  constructor(private http: HttpClient) {}

  getCurrentLocation(): Observable<any> {
    const url = `${this.apiUrl}?key=${this.apiKey}`;
    return this.http.post(url, {});
  }
}
