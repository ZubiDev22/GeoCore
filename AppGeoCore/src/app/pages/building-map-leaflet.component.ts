import { Component, Input, OnChanges, SimpleChanges, AfterViewInit, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-building-map-leaflet',
  standalone: true,
  template: `
    <div #mapContainer [id]="mapId" style="height: 400px; width: 100%; border-radius: 12px; overflow: hidden; box-shadow: 0 2px 12px rgba(25,118,210,0.10); margin: 0 0 2rem 0; position: relative;"></div>
    <button class="btn btn-outline-primary streetview-btn" (click)="openStreetView()" title="Ver Street View">
      <i class="bi bi-person-arms-up"></i> Street View
    </button>
  `,
  styles: [`
    .streetview-btn { position: absolute; top: 20px; right: 20px; z-index: 10; background: white; border-radius: 20px; box-shadow: 0 2px 8px rgba(0,0,0,0.08); }
    .streetview-btn i { margin-right: 4px; }
  `]
})
export class BuildingMapLeafletComponent implements AfterViewInit, OnChanges {
  @Input() latitude: number = 0;
  @Input() longitude: number = 0;
  @Input() zoom: number = 17;
  private map: any;
  private marker: any;
  public mapId = 'leaflet-map-' + Math.random().toString(36).substring(2, 10);
  @ViewChild('mapContainer', { static: true }) mapContainerRef!: ElementRef<HTMLDivElement>;

  ngAfterViewInit() {
    console.log('[Leaflet] ngAfterViewInit', this.latitude, this.longitude, this.zoom, this.mapId);
    this.loadLeaflet(() => {
      this.initMap();
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log('[Leaflet] ngOnChanges', this.latitude, this.longitude, this.zoom, this.mapId);
    if (this.map && this.marker) {
      this.map.setView([this.latitude, this.longitude], this.zoom);
      this.marker.setLatLng([this.latitude, this.longitude]);
    }
  }

  private initMap() {
    if ((window as any).L) {
      // Eliminar el contenido del div para evitar residuos de instancias previas
      if (this.map) {
        this.map.remove();
        this.map = null;
      }
      if (this.mapContainerRef && this.mapContainerRef.nativeElement) {
        this.mapContainerRef.nativeElement.innerHTML = '';
      }
      const mapDiv = document.getElementById(this.mapId);
      if (!mapDiv) {
        console.error('[Leaflet] No se encontró el div del mapa', this.mapId);
        return;
      }
      this.map = (window as any).L.map(this.mapId).setView([this.latitude, this.longitude], this.zoom);
      (window as any).L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap'
      }).addTo(this.map);
      // Forzar icono estándar de Leaflet
      const L = (window as any).L;
      const icon = L.icon({
        iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
        iconRetinaUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon-2x.png',
        shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
        shadowSize: [41, 41]
      });
      this.marker = L.marker([this.latitude, this.longitude], { icon }).addTo(this.map);
      console.log('[Leaflet] initMap', this.latitude, this.longitude, this.zoom, this.mapId);
    }
  }

  private loadLeaflet(callback: () => void) {
    if ((window as any).L) {
      callback();
      return;
    }
    const link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css';
    document.head.appendChild(link);
    const script = document.createElement('script');
    script.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js';
    script.onload = callback;
    document.body.appendChild(script);
  }

  openStreetView() {
    const url = `https://www.google.com/maps/@?api=1&map_action=pano&viewpoint=${this.latitude},${this.longitude}`;
    window.open(url, '_blank');
  }
}
