import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BuildingMapComponent } from './building-map.component';
import { BuildingsService } from '../services/buildings.service';
import { ApartmentsService } from '../services/apartments.service';

@Component({
  selector: 'app-building-detail',
  standalone: true,
  imports: [CommonModule, BuildingMapComponent],
  template: `
      <div *ngIf="profitability">
        <strong>totalIngresos crudo:</strong> {{ profitability.totalIngresos }}
      </div>
      <div class="alert alert-info" *ngIf="profitability">
        <strong>DEBUG profitability:</strong>
        <pre>{{ profitability | json }}</pre>
      </div>
    <div class="container mt-4" *ngIf="loading">
      <div class="spinner-border" role="status"><span class="visually-hidden">Cargando...</span></div>
    </div>
    <div class="container mt-4 building-detail-page" *ngIf="!loading && building">
      <div class="row">
        <div class="col-lg-6 col-md-12 mb-3">
          <h2 class="mb-3">{{ building.name }}</h2>
          <div class="building-info-box p-3 mb-3">
            <app-building-map
              [latitude]="isFiniteNumber(building.latitude) ? building.latitude * 1 : 40.4168"
              [longitude]="isFiniteNumber(building.longitude) ? building.longitude * 1 : -3.7038"
              [zoom]="17"
              [height]="'300px'"
              [width]="'100%'"
              [street]="building.address"
              [number]="building.number"
              [city]="building.city"
              [province]="
                building.city === 'Madrid' ? 'Madrid' :
                building.city === 'Pamplona' ? 'Navarra' :
                ''
              "
              [postalCode]="building.postalCode || building.PostalCode"
              [country]="'España'">
            </app-building-map>
            <div class="mb-2"><strong>Código:</strong> {{ building.buildingCode }}</div>
            <div class="mb-2"><strong>Dirección:</strong> {{ building.address }}</div>
            <div class="mb-2" *ngIf="building.PostalCode"><strong>Código Postal:</strong> {{ building.PostalCode }}</div>
            <div class="mb-2"><strong>Ciudad:</strong> {{ building.city }}</div>
            <div class="mb-2"><strong>Estado:</strong> {{ building.status }}</div>
            <div class="mb-2"><strong>Fecha de compra:</strong> {{ building.purchaseDate | date:'yyyy-MM-dd' }}</div>
            <div class="mb-2 text-muted small">Lat: {{ building.latitude }} | Lng: {{ building.longitude }}</div>
            <!-- DEBUG visual eliminado -->
          </div>
        </div>
        <!-- Mapa duplicado eliminado -->
      </div>

      <div class="mt-4">
        <h4>Apartamentos asociados</h4>
        <div *ngIf="loadingApartments" class="text-center"><div class="spinner-border"></div></div>
        <table class="table table-sm" *ngIf="apartments.length">
          <thead>
            <tr>
              <th>ID</th>
              <th>Estado</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let a of apartments">
              <td>{{ a.apartmentId || a.ApartmentId }}</td>
              <td>{{ a.status || '-' }}</td>
            </tr>
          </tbody>
        </table>
        <div *ngIf="!loadingApartments && !apartments.length" class="text-muted">Sin apartamentos.</div>
      </div>

      <!-- KPIs de Rentabilidad e Ingresos -->
      <div class="mt-4" *ngIf="profitability">
        <h4>KPIs de Rentabilidad</h4>
        <ul>
          <li><strong>Ingresos:</strong> {{ profitability.totalIngresos | currency:'EUR' }}</li>
          <li><strong>Gastos:</strong> {{ profitability.totalGastos | currency:'EUR' }}</li>
          <li><strong>Inversión:</strong> {{ profitability.totalInversion | currency:'EUR' }}</li>
          <li><strong>Rentabilidad:</strong> {{ profitability.rentabilidadMedia }}</li>
        </ul>
      </div>

      <!-- Tabla de detalle de ingresos por edificio -->
      <div class="mt-4" *ngIf="profitability?.detalle?.length">
        <h4>Detalle de ingresos por edificio</h4>
        <table class="table table-sm">
          <thead>
            <tr>
              <th>Edificio</th>
              <th>Ingresos</th>
              <th>Gastos</th>
              <th>Inversión</th>
              <th>Rentabilidad</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let d of profitability.detalle">
              <td>{{ d.BuildingCode || '-' }}</td>
              <td>{{ d.Ingresos | currency:'EUR' }}</td>
              <td>{{ d.Gastos | currency:'EUR' }}</td>
              <td>{{ d.Inversion | currency:'EUR' }}</td>
              <td>{{ d.Rentabilidad }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- NUEVA TABLA: Alquileres asociados si existen -->
      <div class="mt-4" *ngIf="building?.alquileres?.length">
        <h4>Alquileres asociados</h4>
        <table class="table table-sm">
          <thead>
            <tr>
              <th>ID</th>
              <th>Inquilino</th>
              <th>Propiedad</th>
              <th>Precio</th>
              <th>Inicio</th>
              <th>Fin</th>
              <th>Estado</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let r of building.alquileres">
              <td>{{ r.RentalId || r.id || '-' }}</td>
              <td>{{ r.TenantName || r.tenant || '-' }}</td>
              <td>{{ r.PropertyName || r.property || '-' }}</td>
              <td>{{ r.Price || r.price || '-' }}</td>
              <td>{{ r.StartDate || r.start | date:'yyyy-MM-dd' }}</td>
              <td>{{ r.EndDate || r.end | date:'yyyy-MM-dd' }}</td>
              <td>{{ r.Status || r.status || '-' }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <!-- Secciones legacy de eventos y cashflows eliminadas -->
    </div>
    <div *ngIf="error" class="alert alert-danger container mt-4">{{ error }}</div>
  `,
  styles: [`
    .building-detail-page {
      max-width: 1100px;
      background: #fff;
      border-radius: 16px;
      box-shadow: 0 2px 16px rgba(0,0,0,0.07);
      padding: 2rem 2rem 1.5rem 2rem;
      margin-bottom: 2rem;
    }
    .building-info-box {
      background: #f8f9fa;
      border-radius: 12px;
      box-shadow: 0 1px 6px rgba(25,118,210,0.07);
    }
    @media (max-width: 991px) {
      .building-detail-page { padding: 1rem; }
    }
  `]
})
export class BuildingDetailComponent {
  building: any = null;
  profitability: any = null;
  apartments: any[] = [];
  loading = true;
  loadingApartments = true;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private buildingsService: BuildingsService,
    private apartmentsService: ApartmentsService
  ) {
    this.route.params.subscribe((params: any) => {
      const code = params['code'];
      if (code) {
        this.loadBuilding(code);
        this.loadProfitability(code);
        this.loadApartments(code);
      }
    });
  }

  loadProfitability(code: string) {
    this.buildingsService.getProfitabilityByBuilding(code).subscribe({
      next: (data: any) => {
        this.profitability = data;
      },
      error: () => {
        this.profitability = null;
      }
    });
  }
  

  isFiniteNumber(val: any): boolean {
    return (typeof val === 'number' && isFinite(val)) ||
           (typeof val === 'string' && val.trim() !== '' && isFinite(Number(val)));
  }

  get debugCoords() {
    return {
      lat: this.building?.latitude,
      lng: this.building?.longitude,
      typeLat: typeof this.building?.latitude,
      typeLng: typeof this.building?.longitude
    };
  }

  loadBuilding(code: string) {
    this.loading = true;
    this.error = '';
    this.buildingsService.getBuildingDetailsByCode(code).subscribe({
      next: (data: any) => {
        this.building = data;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Error cargando el edificio';
        this.loading = false;
      }
    });
  }

  loadApartments(code: string) {
    this.loadingApartments = true;
    this.apartmentsService.getApartmentsByBuilding(code).subscribe({
      next: (data: any) => {
        this.apartments = data;
        console.log('DEBUG apartamentos:', this.apartments);
        this.loadingApartments = true;
        this.errorApartments = '';
        this.buildingsService.getApartmentsByBuilding(code).subscribe({
          next: (data: any) => {
            this.apartments = data;
            this.loadingApartments = false;
          },
          error: (err: any) => {
            this.errorApartments = 'Error cargando apartamentos';
            this.loadingApartments = false;
          }
        });