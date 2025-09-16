import { Component, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { BuildingsService } from '../services/buildings.service';
import { GeocodingService } from '../services/geocoding.service';

@Component({
  selector: 'app-dashboard-map',
  standalone: true,
  template: `<div #mapContainer class="dashboard-map-container"></div>`,
  styleUrls: ['./dashboard-map.component.scss'],
  providers: [BuildingsService]
})
export class DashboardMapComponent implements AfterViewInit {
  @ViewChild('mapContainer', { static: true }) mapContainer!: ElementRef;
  map: any;
  markers: any[] = [];

  constructor(
    private buildingsService: BuildingsService,
    private geocodingService: GeocodingService
  ) {}

  ngAfterViewInit() {
    // @ts-ignore
    if (window.google && window.google.maps) {
      this.initMap();
    } else {
      this.loadGoogleMapsScript().then(() => this.initMap());
    }
  }

  loadGoogleMapsScript(): Promise<void> {
    return new Promise((resolve) => {
      // Google Maps core
      const script = document.createElement('script');
      script.src = 'https://maps.googleapis.com/maps/api/js?key=AIzaSyBD6tJQnmib3hSFJ9ccCvkmNSMcVCBvZx4&callback=initMapCallback';
      script.async = true;
      (window as any)['initMapCallback'] = () => resolve();
      document.body.appendChild(script);
    });
  }

  initMap() {
    // @ts-ignore
    this.map = new window.google.maps.Map(this.mapContainer.nativeElement, {
      center: { lat: 40.4168, lng: -3.7038 }, // Centro España
      zoom: 6,
      mapTypeControl: false,
      streetViewControl: false,
      fullscreenControl: false
    });
    this.loadBuildings();
  }

  // Lógica local para Street View
  private buildStreetViewUrl(lat: number, lng: number): string {
    return `https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=${lat},${lng}`;
  }

  loadBuildings() {
    this.buildingsService.getBuildings({ page: 1, pageSize: 100 }).subscribe({
      next: (res) => {
        const buildings = Array.isArray(res) ? res : (res.items || []);
        buildings.forEach((b: any) => {
          let lat = b.latitude ?? b.Latitude;
          let lng = b.longitude ?? b.Longitude;
          // Igualar lógica del detalle: si hay street y number separados, únelos; si solo hay address, úsalo tal cual
          let streetWithNumber = '';
          if (b.street || b.Street) {
            streetWithNumber = [b.street ?? b.Street, b.number ?? b.Number].filter(Boolean).join(' ');
          } else if (b.address ?? b.Address) {
            streetWithNumber = b.address ?? b.Address;
          }
          const address = [
            streetWithNumber,
            b.city ?? b.City,
            b.province ?? b.Province,
            b.postalCode ?? b.PostalCode,
            b.country ?? b.Country
          ].filter(Boolean).join(', ');
          if (address) {
            this.geocodingService.getCoordinates(address).subscribe({
              next: (result: any) => {
                const loc = result?.results?.[0]?.geometry?.location;
                if (loc) {
                  this.addMarker(b, loc.lat, loc.lng);
                } else {
                  this.addMarker(b, null, null, true);
                }
              },
              error: () => {
                this.addMarker(b, null, null, true);
              }
            });
          } else if (lat && lng) {
            this.addMarker(b, lat, lng);
          } else {
            this.addMarker(b, null, null, true);
          }
        });
      },
      error: () => {}
    });
  }

  private addMarker(b: any, lat: number|null, lng: number|null, noStreetView = false) {
    let marker: any = null;
    if (lat && lng) {
  // Recalcular y limpiar el enlace de Street View
  const streetViewImg = `<img src="https://maps.googleapis.com/maps/api/streetview?size=200x100&location=${lat},${lng}&fov=80&heading=70&pitch=0&key=AIzaSyBD6tJQnmib3hSFJ9ccCvkmNSMcVCBvZx4" alt="Street View" style="width:100%;border-radius:6px;margin-bottom:6px;"/>`;
  // Usar siempre parseFloat para evitar strings
  const streetViewUrl = `https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=${Number(lat)},${Number(lng)}`;
      marker = new window.google.maps.Marker({
        position: { lat, lng },
        map: this.map,
        title: b.name ?? b.Name ?? b.buildingCode ?? b.BuildingCode,
        icon: {
          url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png',
          scaledSize: new window.google.maps.Size(38, 38)
        }
      });
      const info = new window.google.maps.InfoWindow({
        content: `
          <div style='min-width:220px'>
            ${streetViewImg}
            <strong>${b.name ?? b.Name ?? '-'}</strong><br>
            <span>${b.address ?? b.Address ?? '-'}</span><br>
            <span>${b.city ?? b.City ?? ''}</span><br>
            <span><b>Estado:</b> ${b.status ?? b.Status ?? '-'}</span><br>
            <span><b>Fecha compra:</b> ${b.purchaseDate ? (b.purchaseDate.split('T')[0]) : '-'}</span><br>
            <a href="${streetViewUrl}" target="_blank" rel="noopener">Ver Street View</a>
          </div>
        `
      });
      marker.addListener('click', () => {
        info.open(this.map, marker);
      });
    } else {
      marker = new window.google.maps.Marker({
        position: { lat: 40.4168, lng: -3.7038 },
        map: this.map,
        title: b.name ?? b.Name ?? b.buildingCode ?? b.BuildingCode,
        icon: {
          url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png',
          scaledSize: new window.google.maps.Size(38, 38)
        }
      });
      const info = new window.google.maps.InfoWindow({
        content: `
          <div style='min-width:220px'>
            <strong>${b.name ?? b.Name ?? '-'}</strong><br>
            <span>${b.address ?? b.Address ?? '-'}</span><br>
            <span>${b.city ?? b.City ?? ''}</span><br>
            <span><b>Estado:</b> ${b.status ?? b.Status ?? '-'}</span><br>
            <span><b>Fecha compra:</b> ${b.purchaseDate ? (b.purchaseDate.split('T')[0]) : '-'}</span><br>
            <span style='color:red;'>No hay datos suficientes para Street View</span>
          </div>
        `
      });
      marker.addListener('click', () => {
        info.open(this.map, marker);
      });
    }
  }
}
