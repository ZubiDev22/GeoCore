import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BuildingMapComponent } from './building-map.component';
import { BuildingsService } from '../services/buildings.service';
import { ApartmentsService } from '../services/apartments.service';
import { MaintenanceEventsService } from '../services/maintenance-events.service';
import { CashFlowsService } from '../services/cashflows.service';

@Component({
  selector: 'app-building-detail',
  standalone: true,
  imports: [CommonModule, BuildingMapComponent],
  template: `
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
              [zoom]="17">
            </app-building-map>
            <div class="mb-2"><strong>Código:</strong> {{ building.buildingCode }}</div>
            <div class="mb-2"><strong>Dirección:</strong> {{ building.address }}</div>
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
          <thead><tr><th>ID</th><th>Estado</th><th>Zona</th></tr></thead>
          <tbody>
            <tr *ngFor="let a of apartments">
              <td>{{ a.apartmentId }}</td>
              <td>{{ a.status }}</td>
              <td>{{ a.zone }}</td>
            </tr>
          </tbody>
        </table>
        <div *ngIf="!loadingApartments && !apartments.length" class="text-muted">Sin apartamentos.</div>
      </div>
      <div class="mt-4">
        <h4>Eventos de mantenimiento</h4>
        <div *ngIf="loadingEvents" class="text-center"><div class="spinner-border"></div></div>
        <table class="table table-sm" *ngIf="events.length">
          <thead><tr><th>ID</th><th>Fecha</th><th>Descripción</th><th>Costo</th></tr></thead>
          <tbody>
            <tr *ngFor="let e of events">
              <td>{{ e.maintenanceEventId }}</td>
              <td>{{ e.date | date:'yyyy-MM-dd' }}</td>
              <td>{{ e.description }}</td>
              <td>{{ e.cost | currency }}</td>
            </tr>
          </tbody>
        </table>
        <div *ngIf="!loadingEvents && !events.length" class="text-muted">Sin eventos.</div>
      </div>
      <div class="mt-4">
        <h4>Flujos de caja</h4>
        <div *ngIf="loadingCashflows" class="text-center"><div class="spinner-border"></div></div>
        <table class="table table-sm" *ngIf="cashflows.length">
          <thead><tr><th>ID</th><th>Fuente</th><th>Monto</th><th>Fecha</th></tr></thead>
          <tbody>
            <tr *ngFor="let c of cashflows">
              <td>{{ c.cashFlowId }}</td>
              <td>{{ c.source }}</td>
              <td>{{ c.amount | currency }}</td>
              <td>{{ c.date | date:'yyyy-MM-dd' }}</td>
            </tr>
          </tbody>
        </table>
        <div *ngIf="!loadingCashflows && !cashflows.length" class="text-muted">Sin flujos de caja.</div>
      </div>
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
  apartments: any[] = [];
  events: any[] = [];
  cashflows: any[] = [];
  loading = true;
  loadingApartments = true;
  loadingEvents = true;
  loadingCashflows = true;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private buildingsService: BuildingsService,
    private apartmentsService: ApartmentsService,
    private eventsService: MaintenanceEventsService,
    private cashflowsService: CashFlowsService
  ) {
    this.route.params.subscribe((params: any) => {
      const code = params['code'];
      if (code) {
        this.loadBuilding(code);
        this.loadApartments(code);
        this.loadEvents(code);
        this.loadCashflows(code);
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
        this.loadingApartments = false;
      },
      error: () => {
        this.loadingApartments = false;
      }
    });
  }

  loadEvents(code: string) {
    this.loadingEvents = true;
    // No hay un método getEventsByBuilding, así que filtramos por parámetro
    this.eventsService.getMaintenanceEvents({ buildingCode: code }).subscribe({
      next: (data: any) => {
        this.events = data;
        this.loadingEvents = false;
      },
      error: () => {
        this.loadingEvents = false;
      }
    });
  }

  loadCashflows(code: string) {
    this.loadingCashflows = true;
    this.cashflowsService.getCashFlowsByBuilding(code).subscribe({
      next: (data: any) => {
        this.cashflows = data;
        this.loadingCashflows = false;
      },
      error: () => {
        this.loadingCashflows = false;
      }
    });
  }
}