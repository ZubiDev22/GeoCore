
import { Component } from '@angular/core';
import { BuildingsService } from '../services/buildings.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { GoogleMapsModule } from '@angular/google-maps';

@Component({
  selector: 'app-buildings',
  standalone: true,
  imports: [CommonModule, FormsModule, GoogleMapsModule],
  template: `
    <div class="container mt-4">
      <div class="d-flex justify-content-between align-items-center mb-3">
        <h2>Edificios</h2>
        <button class="btn btn-success" (click)="goToCreate()"><i class="bi bi-plus-lg"></i> Nuevo edificio</button>
      </div>

      <form class="row g-3 mb-3" (ngSubmit)="onFilter()">
        <div class="col-md-3">
          <input type="text" class="form-control" placeholder="Código" [(ngModel)]="filters.code" name="code">
        </div>
        <div class="col-md-3">
          <input type="text" class="form-control" placeholder="Ciudad" [(ngModel)]="filters.city" name="city">
        </div>
        <div class="col-md-3">
          <select class="form-select" [(ngModel)]="filters.status" name="status">
            <option value="">Todos los estados</option>
            <option value="Active">Activo</option>
            <option value="Maintenance">Mantenimiento</option>
            <option value="Inactive">Inactivo</option>
          </select>
        </div>
        <div class="col-md-3">
          <button class="btn btn-primary w-100" type="submit">Filtrar</button>
        </div>
      </form>
      <div *ngIf="loading" class="text-center my-4">
        <div class="spinner-border" role="status"><span class="visually-hidden">Cargando...</span></div>
      </div>
      <div *ngIf="error" class="alert alert-danger">{{ error }}</div>
      <table class="table table-striped" *ngIf="!loading && buildings.length">
        <thead>
          <tr>
            <th>Código</th>
            <th>Nombre</th>
            <th>Dirección</th>
            <th>Ciudad</th>
            <th>Estado</th>
            <th>Fecha de compra</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let b of buildings">
            <td (click)="goToDetail(b.buildingCode)" style="cursor:pointer;">{{ b.buildingCode }}</td>
            <td (click)="goToDetail(b.buildingCode)" style="cursor:pointer;">{{ b.name }}</td>
            <td (click)="goToDetail(b.buildingCode)" style="cursor:pointer;">{{ b.address }}</td>
            <td (click)="goToDetail(b.buildingCode)" style="cursor:pointer;">{{ b.city }}</td>
            <td (click)="goToDetail(b.buildingCode)" style="cursor:pointer;">{{ b.status }}</td>
            <td (click)="goToDetail(b.buildingCode)" style="cursor:pointer;">{{ b.purchaseDate | date:'yyyy-MM-dd' }}</td>
            <td>
              <button class="btn btn-sm btn-outline-primary me-2" (click)="goToEdit(b.buildingCode); $event.stopPropagation()"><i class="bi bi-pencil"></i> Editar</button>
            </td>
          </tr>
        </tbody>
      </table>
      <div *ngIf="!loading && !buildings.length" class="alert alert-info">No hay edificios para mostrar.</div>
      <nav *ngIf="totalPages > 1" class="mt-3">
        <ul class="pagination justify-content-center">
          <li class="page-item" [class.disabled]="page === 1">
            <button class="page-link" (click)="setPage(page-1)" [disabled]="page === 1">Anterior</button>
          </li>
          <li class="page-item" *ngFor="let p of [].constructor(totalPages); let i = index" [class.active]="page === i+1">
            <button class="page-link" (click)="setPage(i+1)">{{ i+1 }}</button>
          </li>
          <li class="page-item" [class.disabled]="page === totalPages">
            <button class="page-link" (click)="setPage(page+1)" [disabled]="page === totalPages">Siguiente</button>
          </li>
        </ul>
      </nav>
    </div>
  `,
  styleUrls: []
})
export class BuildingsComponent {
  private router: Router;
  buildings: any[] = [];
  loading = false;
  error = '';
  page = 1;
  pageSize = 10;
  totalPages = 1;
  filters = { code: '', city: '', status: '' };

  // Para el mapa
  mapCenter: google.maps.LatLngLiteral = { lat: 0, lng: 0 };
  mapZoom = 6;
  mapOptions: google.maps.MapOptions = {
    mapTypeId: 'roadmap',
    streetViewControl: false,
    fullscreenControl: true,
    zoomControl: true
  };
  buildingsWithCoords: any[] = [];

  constructor(private buildingsService: BuildingsService, router: Router) {
    this.router = router;
    this.loadBuildings();
  }

  goToDetail(code: string) {
    this.router.navigate(['/buildings', code]);
  }
  goToCreate() {
    this.router.navigate(['/buildings/new']);
  }
  goToEdit(code: string) {
    this.router.navigate(['/buildings', code, 'edit']);
  }

  loadBuildings() {
    this.loading = true;
    this.error = '';
    const params: any = {
      page: this.page,
      pageSize: this.pageSize,
      ...(this.filters.code && { code: this.filters.code }),
      ...(this.filters.city && { city: this.filters.city }),
      ...(this.filters.status && { status: this.filters.status })
    };
    this.buildingsService.getBuildings(params).subscribe({
      next: (res) => {
        this.buildings = res.items || res;
        this.totalPages = res.totalPages || 1;
        // Filtrar edificios con lat/lng válidos
        this.buildingsWithCoords = this.buildings.filter(b => b.latitude && b.longitude);
        // Centrar el mapa en el primer edificio válido, o en España si no hay
        if (this.buildingsWithCoords.length) {
          this.mapCenter = {
            lat: this.buildingsWithCoords[0].latitude,
            lng: this.buildingsWithCoords[0].longitude
          };
          this.mapZoom = 12;
        } else {
          this.mapCenter = { lat: 40.4168, lng: -3.7038 }; // Madrid por defecto
          this.mapZoom = 6;
        }
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar edificios';
        this.loading = false;
      }
    });
  }

  setPage(p: number) {
    if (p < 1 || p > this.totalPages) return;
    this.page = p;
    this.loadBuildings();
  }

  onFilter() {
    this.page = 1;
    this.loadBuildings();
  }
}
