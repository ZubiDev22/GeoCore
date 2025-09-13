// Ejemplo de uso del servicio GeocodingService en un componente Angular
import { Component } from '@angular/core';
import { GeocodingService } from '../services/geocoding.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-geocoding-demo',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <input [(ngModel)]="address" placeholder="Dirección" />
      <button (click)="buscarCoordenadas()">Buscar coordenadas</button>
      <div *ngIf="coords as c">
        <strong>Latitud:</strong> {{ c.lat }}<br />
        <strong>Longitud:</strong> {{ c.lng }}
      </div>
    </div>
  `
})
export class GeocodingDemoComponent {
  address = '';
  coords: { lat: number, lng: number } | null = null;

  constructor(private geocoding: GeocodingService) {}

  buscarCoordenadas() {
    this.geocoding.getCoordinates(this.address).subscribe(resp => {
      if (resp.status === 'OK' && resp.results.length > 0) {
        this.coords = resp.results[0].geometry.location;
      } else {
        this.coords = null;
      }
    });
  }
}
