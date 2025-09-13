import { Component, Input } from '@angular/core';
import { GoogleMapsModule } from '@angular/google-maps';

@Component({
  selector: 'app-building-map',
  standalone: true,
  imports: [GoogleMapsModule],
  template: `
    <google-map
      [height]="height"
      [width]="width"
      [center]="{lat: latitude, lng: longitude}"
      [zoom]="zoom"
      [options]="mapOptions">
      <map-marker [position]="{lat: latitude, lng: longitude}"></map-marker>
    </google-map>
    <button class="btn btn-outline-primary streetview-btn" (click)="openStreetView()" title="Ver Street View">
      <span style="vertical-align: middle;">👁️</span> Street View
    </button>
  `,
  styles: [`
    :host { display: block; }
    .streetview-btn {
      margin-top: 10px;
      border-radius: 20px;
      box-shadow: 0 2px 8px rgba(0,0,0,0.08);
      font-size: 1rem;
    }
  `]
})
export class BuildingMapComponent {
  @Input() latitude: number = 40.4168;
  @Input() longitude: number = -3.7038;
  @Input() zoom: number = 17;
  @Input() height: string = '300px';
  @Input() width: string = '100%';

  mapOptions: google.maps.MapOptions = {
    mapTypeId: 'roadmap',
    disableDefaultUI: false
  };

  openStreetView() {
    const url = `https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=${this.latitude},${this.longitude}`;
    window.open(url, '_blank');
  }
}
