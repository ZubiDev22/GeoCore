import { Component, ElementRef, AfterViewInit, ViewChild } from '@angular/core';
import { BuildingsService } from '../services/buildings.service';

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

  constructor(private buildingsService: BuildingsService) {}

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

  loadBuildings() {
    this.buildingsService.getBuildings().subscribe({
      next: (res) => {
        let buildings = Array.isArray(res) ? res : (res.items || []);
        buildings.forEach((b: any) => {
          const lat = b.latitude ?? b.Latitude;
          const lng = b.longitude ?? b.Longitude;
          if (lat && lng) {
            // @ts-ignore
            const marker = new window.google.maps.Marker({
              position: { lat, lng },
              map: this.map,
              title: b.name ?? b.Name ?? b.buildingCode ?? b.BuildingCode,
              icon: {
                url: 'https://maps.google.com/mapfiles/ms/icons/blue-dot.png',
                scaledSize: new window.google.maps.Size(38, 38)
              }
            });
            // InfoWindow personalizado
            const info = new window.google.maps.InfoWindow({
              content: `
                <div style='min-width:180px'>
                  <strong>${b.name ?? b.Name ?? '-'}</strong><br>
                  <span>${b.address ?? b.Address ?? '-'}</span><br>
                  <span>${b.city ?? b.City ?? ''}</span>
                </div>
              `
            });
            marker.addListener('click', () => {
              info.open(this.map, marker);
            });
          }
        });
      },
      error: () => {}
    });
  }
}
