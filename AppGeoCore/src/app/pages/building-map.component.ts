import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GoogleMapsModule, MapMarker } from '@angular/google-maps';

@Component({
  selector: 'app-building-map',
  standalone: true,
  imports: [CommonModule, GoogleMapsModule],
  template: `
    <div class="map-container">
      <google-map height="400px" width="100%"
        [center]="center"
        [zoom]="zoom"
        [options]="mapOptions">
        <map-marker [position]="center" [title]="'Ubicación del edificio'"></map-marker>
      </google-map>
      <button class="btn btn-outline-primary streetview-btn" (click)="openStreetView()" title="Ver Street View">
        <i class="bi bi-person-arms-up"></i> Street View
      </button>
    </div>
  `,
  styles: [`
    .map-container { width: 100%; height: 400px; border-radius: 12px; overflow: hidden; box-shadow: 0 2px 12px rgba(25,118,210,0.10); margin: 0 0 2rem 0; position: relative; }
    .streetview-btn { position: absolute; top: 20px; right: 20px; z-index: 10; background: white; border-radius: 20px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
    .streetview-btn i { margin-right: 4px; }
  `]
})
export class BuildingMapComponent {
  @Input() latitude: number = 0;
  @Input() longitude: number = 0;
  @Input() zoom: number = 17;

  get center() {
    return { lat: this.latitude, lng: this.longitude };
  }

  mapOptions = {
    mapTypeId: 'roadmap',
    streetViewControl: true,
    zoomControl: true,
    fullscreenControl: true
  };

  openStreetView() {
    const url = `https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=${this.latitude},${this.longitude}`;
    window.open(url, '_blank');
  }
}
