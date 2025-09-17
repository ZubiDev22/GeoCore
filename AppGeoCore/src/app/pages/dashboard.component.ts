
import { Component, OnInit } from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { DashboardMapComponent } from './dashboard-map.component';
import { BuildingsService } from '../services/buildings.service';
import { RentalsService } from '../services/rentals.service';
import { ApartmentsService } from '../services/apartments.service';

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
    private rentalsService: RentalsService,
    private apartmentsService: ApartmentsService
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

    // Inicializar todos los KPIs a cero
    this.totalBuildings = 0;
    this.totalIncome = 0;
    this.avgProfitability = 0;
    this.occupancyRate = 0;

    // 1. Total de edificios: recorrer todas las páginas y sumar
    this.getAllBuildings().then(total => {
      this.totalBuildings = total;
    }).catch(err => {
      console.error('Error getAllBuildings:', err);
      this.totalBuildings = 0;
      this.kpiError = (this.kpiError ? this.kpiError + ' | ' : '') + 'No se pudo cargar el total de edificios.';
    });

    // 2. Ingresos y rentabilidad: usando profitability-by-location
  // Enviar parámetro zone para cumplir con la validación del backend
  this.buildingsService.getProfitabilityByLocation({ zone: 'Centro' }).subscribe({
      next: (res) => {
        this.totalIncome = res?.TotalIngresos ?? 0;
        if (typeof res?.RentabilidadMedia === 'string') {
          this.avgProfitability = Number(res.RentabilidadMedia.replace('%', '').replace(',', '.'));
        } else if (typeof res?.RentabilidadMedia === 'number') {
          this.avgProfitability = Number(res.RentabilidadMedia);
        }
      },
      error: (err) => {
        console.error('Error getProfitabilityByLocation:', err);
        let msg = 'No se pudieron cargar ingresos/rentabilidad.';
        // Si el error es un objeto estructurado, mostrar sus campos
        if (err && typeof err === 'object') {
          if (err.message) {
            msg += `\nMensaje: ${err.message}`;
          }
          if (err.code) {
            msg += `\nCódigo: ${err.code}`;
          }
          if (err.details) {
            msg += `\nDetalles: ${err.details}`;
          }
        } else if (typeof err === 'string') {
          msg += ` Detalle: ${err}`;
        }
        this.kpiError = (this.kpiError ? this.kpiError + ' | ' : '') + msg;
      }
    });

    // 3. Ocupación: % de apartamentos con alquiler activo
    try {
      this.rentalsService.getRentals({ status: 'active' }).subscribe({
        next: async (result) => {
          let rentals: any[] = Array.isArray(result?.value) ? result.value : [];
          const occupied = rentals.length;
          try {
            const apartments = await this.getAllApartments();
            const totalApartments = apartments.length;
            this.occupancyRate = totalApartments > 0 ? Math.round((occupied / totalApartments) * 100) : 0;
          } catch (err) {
            console.error('Error getAllApartments:', err);
            this.occupancyRate = 0;
            this.kpiError = (this.kpiError ? this.kpiError + ' | ' : '') + 'No se pudo calcular la ocupación.';
          }
          this.loadingKpis = false;
        },
        error: (err) => {
          console.error('Error getRentals:', err);
          this.occupancyRate = 0;
          this.kpiError = (this.kpiError ? this.kpiError + ' | ' : '') + 'No se pudieron cargar los alquileres.';
          this.loadingKpis = false;
        }
      });
    } catch (err) {
      console.error('Error rentalsService.getRentals:', err);
      this.occupancyRate = 0;
      this.kpiError = (this.kpiError ? this.kpiError + ' | ' : '') + 'No se pudo calcular la ocupación.';
      this.loadingKpis = false;
    }
  }

  /**
   * Obtiene todos los edificios paginados del backend y suma el total
   */
  private async getAllBuildings(): Promise<number> {
    let total = 0;
    let page = 1;
    let totalPages = 1;
    do {
      const res = await this.buildingsService.getBuildings({ page }).toPromise();
      const items = Array.isArray(res?.items) ? res.items : (Array.isArray(res?.data) ? res.data : Array.isArray(res) ? res : []);
      total += items.length;
      totalPages = res?.totalPages || res?.total_pages || 1;
      page++;
    } while (page <= totalPages);
    return total;
  }


  /**
   * Obtiene todos los apartamentos paginados del backend
   */
  private async getAllApartments(): Promise<any[]> {
    let all: any[] = [];
    let page = 1;
    let totalPages = 1;
    do {
      const res = await this.apartmentsService.getApartments({ page }).toPromise();
      const items = Array.isArray(res?.items) ? res.items : [];
      all = all.concat(items);
      totalPages = res?.totalPages || 1;
      page++;
    } while (page <= totalPages);
    return all;
  }
}
