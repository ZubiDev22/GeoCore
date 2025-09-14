import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class GeocodingService {
  private apiKey = environment.googleMapsApiKey;
  private apiUrl = 'https://maps.googleapis.com/maps/api/geocode/json';

  constructor(private http: HttpClient) {}

  getCoordinates(address: string): Observable<any> {
    const url = `${this.apiUrl}?address=${encodeURIComponent(address)}&key=${this.apiKey}`;
    return this.http.get(url);
  }

  getAddress(lat: number, lng: number): Observable<any> {
    const url = `${this.apiUrl}?latlng=${lat},${lng}&key=${this.apiKey}`;
    return this.http.get(url);
  }
}
