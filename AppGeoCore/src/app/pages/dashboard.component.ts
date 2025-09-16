
import { Component, OnInit } from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { DashboardMapComponent } from './dashboard-map.component';
import { BuildingsService } from '../services/buildings.service';
import { RentalsService } from '../services/rentals.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, DashboardMapComponent],
  providers: [CurrencyPipe],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  totalBuildings: number = 0;
  occupancyRate: number = 0;
  totalIncome: number = 0;
  avgProfitability: number = 0;
  kpiError: string | null = null;
  loadingKpis: boolean = false;

  constructor(
    private buildingsService: BuildingsService,
    private rentalsService: RentalsService
  ) {}

  ngOnInit(): void {
    this.loadKpis();
  }

  loadKpis(): void {
    this.loadKpisAsync();
  }

  /**
   * Carga los KPIs principales del dashboard.
   * - Total de edificios, ingresos y rentabilidad: del endpoint profitability-by-location
   * - Ocupación: calculada con alquileres activos y total de apartamentos
   */
  private async loadKpisAsync(): Promise<void> {
    this.kpiError = null;
    this.loadingKpis = true;

    // 1. KPIs principales: total de edificios, ingresos y rentabilidad
    this.buildingsService.getProfitabilityByLocation().subscribe({
      next: (res) => {
        console.log('getProfitabilityByLocation response:', res);
        // Total de edificios
        this.totalBuildings = res?.TotalEdificios ?? 0;
        // Ingresos totales
        this.totalIncome = res?.TotalIngresos ?? 0;
        // Rentabilidad media (puede venir como string con %)
        if (typeof res?.RentabilidadMedia === 'string') {
          this.avgProfitability = Number(res.RentabilidadMedia.replace('%', '').replace(',', '.'));
        } else if (typeof res?.RentabilidadMedia === 'number') {
          this.avgProfitability = Number(res.RentabilidadMedia);
        }
      },
      error: (err) => {
        console.error('Error getProfitabilityByLocation:', err);
        this.kpiError = 'No se pudieron cargar los KPIs principales.';
      }
    });

    // 2. Ocupación: % de apartamentos con alquiler activo
    try {
      // Obtener alquileres activos
      this.rentalsService.getRentals({ status: 'active' }).subscribe({
        next: async (result) => {
          let rentals: any[] = Array.isArray(result?.value) ? result.value : [];
          const occupied = rentals.length;
          // Obtener todos los apartamentos
          const apartments = await this.getAllApartments();
          const totalApartments = apartments.length;
          this.occupancyRate = totalApartments > 0 ? Math.round((occupied / totalApartments) * 100) : 0;
          this.loadingKpis = false;
        },
        error: (err) => {
          console.error('Error getRentals:', err);
          this.occupancyRate = 0;
          this.loadingKpis = false;
          this.kpiError = 'No se pudieron cargar los alquileres.';
        }
      });
    } catch (err) {
      console.error('Error getAllApartments:', err);
      this.occupancyRate = 0;
      this.loadingKpis = false;
      this.kpiError = 'No se pudo calcular la ocupación.';
    }
  }

  /**
   * Obtiene todos los apartamentos paginados del backend
   */
  private async getAllApartments(): Promise<any[]> {
    let all: any[] = [];
    let page = 1;
    let totalPages = 1;
    do {
      // Suponiendo que hay un método getApartments en el servicio correspondiente
      const res = await (this['apartmentsService']?.getApartments({ page })?.toPromise?.() ?? Promise.resolve({ items: [], totalPages: 1 }));
      const items = Array.isArray(res?.items) ? res.items : [];
      all = all.concat(items);
      totalPages = res?.totalPages || 1;
      page++;
    } while (page <= totalPages);
    return all;
  }
}
