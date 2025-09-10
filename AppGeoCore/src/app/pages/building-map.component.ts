import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GoogleMapsModule, MapMarker } from '@angular/google-maps';
import { environment } from '../../environments/environment';

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
    </div>
  `,
  styles: [`
    .map-container { width: 100%; height: 400px; border-radius: 12px; overflow: hidden; box-shadow: 0 2px 12px rgba(25,118,210,0.10); margin: 1rem 0; }
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
}
