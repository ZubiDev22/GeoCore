import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class GeolocationService {
  private apiKey = environment.googleMapsApiKey;
  private apiUrl = 'https://www.googleapis.com/geolocation/v1/geolocate';

  constructor(private http: HttpClient) {}

  getCurrentLocation(): Observable<any> {
    const url = `${this.apiUrl}?key=${this.apiKey}`;
    return this.http.post(url, {});
  }
}
