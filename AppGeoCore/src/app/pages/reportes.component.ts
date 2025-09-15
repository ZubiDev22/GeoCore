import { Component, OnInit } from '@angular/core';
import { BuildingsService } from '../services/buildings.service';
import { ProfitabilityByLocationDto, ProfitabilityDto } from '../models/reportes.model';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-reportes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  providers: [CurrencyPipe],
  template: `
    <div class="container mt-4">
      <h2>Reportes de Rentabilidad</h2>
      <div *ngIf="profitByLocation?.EscalaBaremosDescripcion" class="alert alert-secondary mb-3">
        <strong>Escala de baremos:</strong> {{ profitByLocation?.EscalaBaremosDescripcion }}
      </div>
      <form class="row g-3 mb-4" (ngSubmit)="buscar()" autocomplete="off">
        <div class="col-md-4">
          <input type="text" class="form-control" placeholder="Ciudad" [(ngModel)]="city" name="city">
        </div>
        <div class="col-md-4">
          <input type="text" class="form-control" placeholder="Zona" [(ngModel)]="zone" name="zone">
        </div>
        <div class="col-md-4">
          <input type="text" class="form-control" placeholder="Código Postal" [(ngModel)]="postalCode" name="postalCode">
        </div>
        <div class="col-12 d-flex gap-2">
          <button class="btn btn-primary" type="submit" [disabled]="!city && !zone && !postalCode || loading">Buscar</button>
          <button class="btn btn-outline-secondary" type="button" (click)="resetFiltros()" [disabled]="loading">Resetear filtros</button>
        </div>
      </form>
      <div *ngIf="loading" class="text-center my-4"><div class="spinner-border"></div></div>
      <div *ngIf="error" class="alert alert-danger">{{ error }}</div>
      <div *ngIf="profitByLocation">
  <ng-container *ngIf="profitByLocation && profitByLocation.Detalle && profitByLocation.Detalle.length > 0; else noDatos">
          <h4>Rentabilidad por Localización</h4>
          <div class="mb-2"><strong>Total edificios:</strong> {{ profitByLocation.TotalEdificios }}</div>
          <div class="mb-2"><strong>Total ingresos:</strong> {{ profitByLocation.TotalIngresos | currency:'EUR' }}</div>
          <div class="mb-2"><strong>Total gastos:</strong> {{ profitByLocation.TotalGastos | currency:'EUR' }}</div>
          <div class="mb-2"><strong>Total inversión:</strong> {{ profitByLocation.TotalInversion | currency:'EUR' }}</div>
          <div class="mb-2"><strong>Rentabilidad media:</strong> {{ profitByLocation.RentabilidadMedia }}</div>
          <div class="row g-3 mt-3">
            <div class="col-md-6 col-lg-4" *ngFor="let d of profitByLocation.Detalle">
              <div class="card shadow-sm h-100 border-0 building-card">
                <div class="card-body">
                  <h5 class="card-title mb-2 text-primary">
                    {{ d.Address || '-' }}<span *ngIf="d.City">, {{ d.City }}</span><span *ngIf="d.PostalCode"> (CP: {{ d.PostalCode }})</span>
                  </h5>
                    <div class="mb-1 small">
                      <span *ngIf="d.TipoRentabilidad === 'Potencial' && d.Baremo" class="badge-baremo-potencial me-1">Rentabilidad potencial: {{ d.Baremo }}</span>
                      <span *ngIf="d.TipoRentabilidad === 'Real' && d.Baremo" class="badge-baremo-real me-1">Nivel de rentabilidad real: {{ d.Baremo }}</span>
                    </div>
                  <div class="mb-2 small text-secondary">
                    <span class="fw-bold">Código:</span> {{ d.BuildingCode }}
                  </div>
                  <ul class="list-unstyled mb-2">
                    <li><strong>Ingresos:</strong> <span class="text-success">{{ d.Ingresos | currency:'EUR' }}</span></li>
                    <li><strong>Gastos:</strong> <span class="text-danger">{{ d.Gastos | currency:'EUR' }}</span></li>
                    <li><strong>Inversión:</strong> <span>{{ d.Inversion | currency:'EUR' }}</span></li>
                    <li>
                      <ng-container *ngIf="d.TipoRentabilidad === 'Real'">
                        <strong>Rentabilidad real:</strong> <span class="fw-bold">{{ d.Rentabilidad }}</span>
                        <span *ngIf="d.Baremo"> ({{ d.Baremo }})</span>
                      </ng-container>
                      <ng-container *ngIf="d.TipoRentabilidad === 'Potencial'">
                        <strong>Rentabilidad potencial:</strong> <span class="fw-bold">{{ d.Rentabilidad }}</span>
                        <span *ngIf="d.Baremo"> ({{ d.Baremo }})</span>
                      </ng-container>
                      <ng-container *ngIf="!d.TipoRentabilidad">
                        <strong>Rentabilidad:</strong> <span class="fw-bold">{{ d.Rentabilidad }}</span>
                        <span *ngIf="d.Baremo"> ({{ d.Baremo }})</span>
                      </ng-container>
                    </li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </ng-container>
        <ng-template #noDatos>
          <div class="alert alert-info mt-4">No hay datos para los filtros seleccionados.</div>
        </ng-template>
      </div>
    </div>
  `,
  styles: [`
    .container { max-width: 1100px; background: #fff; border-radius: 16px; box-shadow: 0 2px 16px rgba(0,0,0,0.07); padding: 2rem 2rem 1.5rem 2rem; margin-bottom: 2rem; }
    .building-card { border-radius: 16px; background: #f8f9fa; transition: box-shadow 0.2s; }
    .building-card:hover { box-shadow: 0 4px 24px rgba(0,0,0,0.13); }
    .card-title { font-size: 1.15rem; font-weight: 600; }
    .list-unstyled li { margin-bottom: 0.25rem; }
    @media (max-width: 767px) {
      .container { padding: 1rem 0.5rem; }
    }
  `]
})
export class ReportesComponent implements OnInit {
  profitByLocation: ProfitabilityByLocationDto | null = null;
  loading = false;
  error = '';
  city = '';
  zone = '';
  postalCode = '';

  constructor(private buildingsService: BuildingsService) {}

  ngOnInit() {
    // No cargar nada hasta que el usuario filtre
  }

  buscar() {
    if (!this.city && !this.zone && !this.postalCode) {
      this.error = 'Debes ingresar al menos un filtro.';
      return;
    }
    this.loading = true;
    this.error = '';
    const variantesCity = this.city
      ? [
          this.city,
          this.city.trim(),
          this.city.toLowerCase(),
          this.city.toUpperCase(),
          this.city.charAt(0).toUpperCase() + this.city.slice(1).toLowerCase(),
          this.city.replace(/\s+/g, ''),
        ]
      : [null];
    const variantesZone = this.zone
      ? [
          this.zone,
          this.zone.trim(),
          this.zone.toLowerCase(),
          this.zone.toUpperCase(),
          this.zone.charAt(0).toUpperCase() + this.zone.slice(1).toLowerCase(),
          this.zone.replace(/\s+/g, ''),
        ]
      : [null];

    // Probar todas las combinaciones de variantes de ciudad y zona
  const combinaciones: { city: string | null, zone: string | null }[] = [];
    for (const c of variantesCity) {
      for (const z of variantesZone) {
        combinaciones.push({ city: c, zone: z });
      }
    }

    const probarSiguiente = (i: number) => {
      if (i >= combinaciones.length) {
        this.profitByLocation = null;
        this.loading = false;
        this.error = 'No se encontraron datos para ninguna variante de ciudad/zona.';
        return;
      }
      const params: any = {};
      if (combinaciones[i].city) params.city = combinaciones[i].city;
      if (combinaciones[i].zone) params.zone = combinaciones[i].zone;
      if (this.postalCode) params.postalCode = this.postalCode;
      this.buildingsService.getProfitabilityByLocation(params).subscribe({
        next: (data) => {
          // LOG: Verificar cómo llegan los campos de rentabilidad y escalas
          console.log('Respuesta API reportes:', data);
          if (data && data.Detalle) {
            data.Detalle.forEach((d: any) => {
              console.log('Edificio:', d.BuildingCode, '| TipoRentabilidad:', d.TipoRentabilidad);
            });
          }
          if (data && data.EscalaBaremosDescripcion) {
            console.log('EscalaBaremosDescripcion:', data.EscalaBaremosDescripcion);
          }
          const adaptado = this.adaptarProfitByLocation(data);
          if (adaptado && adaptado.Detalle && adaptado.Detalle.length > 0) {
            this.profitByLocation = adaptado;
            this.loading = false;
          } else {
            probarSiguiente(i + 1);
          }
        },
        error: () => {
          probarSiguiente(i + 1);
        }
      });
    }
    probarSiguiente(0);
  }

  private adaptarProfitByLocation(data: any): ProfitabilityByLocationDto | null {
    if (!data) return null;
    // Adaptar cada detalle para soportar claves en minúsculas y variantes
    const adaptarDetalle = (d: any) => ({
      BuildingCode: d.BuildingCode ?? d.buildingCode ?? d.building_code ?? d.buildingcode,
      Ingresos: d.Ingresos ?? d.ingresos,
      Gastos: d.Gastos ?? d.gastos,
      Inversion: d.Inversion ?? d.inversion,
      Rentabilidad: d.Rentabilidad ?? d.rentabilidad,
      TipoRentabilidad: d.TipoRentabilidad ?? d.tiporentabilidad,
      Baremo: d.Baremo ?? d.baremo,
      Address: d.Address ?? d.address ?? d.direccion ?? d.Direccion,
      City: d.City ?? d.city ?? d.ciudad ?? d.Ciudad,
      PostalCode: d.PostalCode ?? d.postalCode ?? d.codigoPostal ?? d.CodigoPostal
    });
    return {
      TotalEdificios: data.TotalEdificios ?? data.totalEdificios,
      TotalIngresos: data.TotalIngresos ?? data.totalIngresos,
      TotalGastos: data.TotalGastos ?? data.totalGastos,
      TotalInversion: data.TotalInversion ?? data.totalInversion,
      RentabilidadMedia: data.RentabilidadMedia ?? data.rentabilidadMedia,
      Detalle: (data.Detalle ?? data.detalle ?? []).map(adaptarDetalle),
      EscalaBaremosDescripcion: data.EscalaBaremosDescripcion ?? data.escalabaremosdescripcion
    };
  }
  resetFiltros() {
    this.city = '';
    this.zone = '';
    this.postalCode = '';
    this.profitByLocation = null;
    this.error = '';
  }
}
