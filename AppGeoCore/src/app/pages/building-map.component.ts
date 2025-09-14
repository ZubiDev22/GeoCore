import { Component, Input, OnInit } from '@angular/core';
import { GeocodingService } from '../services/geocoding.service';
import { GoogleMapsModule } from '@angular/google-maps';

@Component({
  selector: 'app-building-map',
  standalone: true,
  imports: [GoogleMapsModule],
  template: `
    <google-map
      [height]="height"
      [width]="width"
      [center]="{lat: mapLat, lng: mapLng}"
      [zoom]="zoom"
      [options]="mapOptions">
      <map-marker [position]="{lat: mapLat, lng: mapLng}"></map-marker>
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
export class BuildingMapComponent implements OnInit {
  @Input() latitude: number = 40.4168;
  @Input() longitude: number = -3.7038;
  @Input() zoom: number = 17;
  @Input() height: string = '300px';
  @Input() width: string = '100%';
  @Input() street?: string;
  @Input() number?: string;
  @Input() city?: string;
  @Input() province?: string;
  @Input() country?: string;
  @Input() postalCode?: string;
  address?: string;

  // Coordenadas internas para el centro del mapa y el marcador
  mapLat: number = 40.4168;
  mapLng: number = -3.7038;

  loadingGeocode = false;
  errorGeocode = '';

  constructor(private geocodingService: GeocodingService) {}

  mapOptions: google.maps.MapOptions = {
    mapTypeId: 'roadmap',
    disableDefaultUI: false
  };

  ngOnInit() {
    // Inicializa el centro con los inputs
    this.mapLat = this.latitude;
    this.mapLng = this.longitude;
    // Construir dirección completa si hay datos suficientes
    this.address = this.buildFullAddress();
    if (this.address) {
      this.loadingGeocode = true;
      this.geocodingService.getCoordinates(this.address).subscribe({
        next: (result: any) => {
          console.log('DEBUG geocoding response:', result);
          const loc = result?.results?.[0]?.geometry?.location;
          if (loc) {
            this.mapLat = loc.lat;
            this.mapLng = loc.lng;
          }
          this.loadingGeocode = false;
        },
        error: () => {
          this.errorGeocode = 'No se pudo geocodificar la dirección.';
          this.loadingGeocode = false;
        }
      });
    }
  }

  private buildFullAddress(): string {
    const parts = [
      this.street,
      this.number,
      this.city,
      this.province,
      this.postalCode,
      this.country
    ];
    // Filtra partes vacías y las une con coma
    return parts.filter(Boolean).join(', ');
  }

  openStreetView() {
    const url = `https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=${this.mapLat},${this.mapLng}`;
    window.open(url, '_blank');
  }
}
