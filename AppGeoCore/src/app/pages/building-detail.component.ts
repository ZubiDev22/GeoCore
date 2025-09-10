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
    <div class="container mt-4" *ngIf="!loading && building">
      <h2>Detalle de Edificio</h2>
  <table class="table table-bordered">
        <tr><th>Código</th><td>{{ building.buildingCode }}</td></tr>
        <tr><th>Nombre</th><td>{{ building.name }}</td></tr>
        <tr><th>Dirección</th><td>{{ building.address }}</td></tr>
        <tr><th>Ciudad</th><td>{{ building.city }}</td></tr>
        <tr><th>Estado</th><td>{{ building.status }}</td></tr>
        <tr><th>Fecha de compra</th><td>{{ building.purchaseDate | date:'yyyy-MM-dd' }}</td></tr>
        <tr><th>Latitud</th><td>{{ building.latitude }}</td></tr>
        <tr><th>Longitud</th><td>{{ building.longitude }}</td></tr>
      </table>
      <div class="mt-4">
        <h4>Ubicación en el mapa</h4>
        <app-building-map
          *ngIf="building.latitude && building.longitude"
          [latitude]="building.latitude"
          [longitude]="building.longitude"
          [zoom]="17">
        </app-building-map>
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
  styleUrls: []
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
    private maintenanceEventsService: MaintenanceEventsService,
    private cashFlowsService: CashFlowsService
  ) {
    this.route.params.subscribe(params => {
      const code = params['code'];
      if (code) {
        this.loadBuilding(code);
        this.loadApartments(code);
        this.loadEvents(code);
        this.loadCashflows(code);
      }
    });
  }

  loadBuilding(code: string) {
    this.loading = true;
    this.error = '';
    this.buildingsService.getBuildingByCode(code).subscribe({
      next: (res) => {
        this.building = res;
        this.loading = false;
      },
      error: () => {
        this.error = 'No se pudo cargar el edificio';
        this.loading = false;
      }
    });
  }

  loadApartments(code: string) {
    this.loadingApartments = true;
    this.apartmentsService.getApartmentsByBuilding(code).subscribe({
      next: (res) => {
        this.apartments = res.items || res;
        this.loadingApartments = false;
      },
      error: () => {
        this.apartments = [];
        this.loadingApartments = false;
      }
    });
  }

  loadEvents(code: string) {
    this.loadingEvents = true;
    this.maintenanceEventsService.getMaintenanceEvents({ buildingCode: code }).subscribe({
      next: (res) => {
        this.events = res.items || res;
        this.loadingEvents = false;
      },
      error: () => {
        this.events = [];
        this.loadingEvents = false;
      }
    });
  }

  loadCashflows(code: string) {
    this.loadingCashflows = true;
    this.cashFlowsService.getCashFlowsByBuilding(code).subscribe({
      next: (res) => {
        this.cashflows = res.items || res;
        this.loadingCashflows = false;
      },
      error: () => {
        this.cashflows = [];
        this.loadingCashflows = false;
      }
    });
  }
}
