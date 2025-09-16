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

  constructor(
    private buildingsService: BuildingsService,
    private rentalsService: RentalsService
  ) {}

  ngOnInit(): void {
    this.loadKpis();
  }

  loadKpis(): void {
    // 1. KPIs principales desde el backend (edificios, ingresos, rentabilidad)
    this.buildingsService.getProfitabilityByLocation().subscribe({
      next: (res) => {
        console.log('getProfitabilityByLocation response:', res);
        this.totalBuildings = res?.TotalEdificios ?? 0;
        this.totalIncome = res?.TotalIngresos ?? 0;
        this.avgProfitability = res?.RentabilidadMedia ? Number(parseFloat(res.RentabilidadMedia).toFixed(2)) : 0;
      },
      error: (err) => {
        console.error('Error getProfitabilityByLocation:', err);
        // Mostrar mensaje claro en consola, pero no bloquear el resto de KPIs
      }
    });

    // 2. Ocupación: % de apartamentos con alquiler activo
    // Se asume que RentalsService.getRentals() devuelve todos los alquileres activos
    this.rentalsService.getRentals({ status: 'active' }).subscribe({
      next: (result) => {
        console.log('getRentals response:', result);
        // Aceptar tanto 'value' como 'data' como posibles arrays de alquileres
        let rentals: any[] = [];
        if (Array.isArray(result?.value)) {
          rentals = result.value;
        } else if (Array.isArray(result?.data)) {
          rentals = result.data;
        }
        const occupied = rentals.length;
        // Obtener todas las páginas de edificios para el total real
        this.getAllBuildings().then((allBuildings) => {
          let totalApartments = allBuildings.reduce((acc: number, b: any) => acc + (b.apartmentsCount || b.totalApartments || 0), 0);
          this.totalBuildings = allBuildings.length;
          this.occupancyRate = totalApartments > 0 ? Math.round((occupied / totalApartments) * 100) : 0;
        }).catch((err) => {
          console.error('Error getAllBuildings:', err);
          this.occupancyRate = 0;
        });
      },
      error: (err) => {
        console.error('Error getRentals:', err);
        this.occupancyRate = 0;
      }
    });
  }

  /**
   * Obtiene todos los edificios paginados del backend
   */
  private async getAllBuildings(): Promise<any[]> {
    let all: any[] = [];
    let page = 1;
    let totalPages = 1;
    do {
      const res = await this.buildingsService.getBuildings({ page }).toPromise();
      const items = Array.isArray(res?.items) ? res.items : [];
      all = all.concat(items);
      totalPages = res?.totalPages || 1;
      page++;
    } while (page <= totalPages);
    return all;
  }
}
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

  constructor(
    private buildingsService: BuildingsService,
    private rentalsService: RentalsService
  ) {}

  ngOnInit(): void {
    this.loadKpis();
  }

  loadKpis(): void {
    // 1. KPIs principales desde el backend (edificios, ingresos, rentabilidad)
    this.buildingsService.getProfitabilityByLocation().subscribe({
      next: (res) => {
        console.log('getProfitabilityByLocation response:', res);
        this.totalBuildings = res?.TotalEdificios ?? 0;
        this.totalIncome = res?.TotalIngresos ?? 0;
        this.avgProfitability = res?.RentabilidadMedia ? Number(parseFloat(res.RentabilidadMedia).toFixed(2)) : 0;
      },
      error: (err) => {
        console.error('Error getProfitabilityByLocation:', err);
        // Mostrar mensaje claro en consola, pero no bloquear el resto de KPIs
      }
    });

    // 2. Ocupación: % de apartamentos con alquiler activo
    // Se asume que RentalsService.getRentals() devuelve todos los alquileres activos
    this.rentalsService.getRentals({ status: 'active' }).subscribe({
      next: (result) => {
        console.log('getRentals response:', result);
        // Aceptar tanto 'value' como 'data' como posibles arrays de alquileres
        let rentals: any[] = [];
        if (Array.isArray(result?.value)) {
          rentals = result.value;
        } else if (Array.isArray(result?.data)) {
          rentals = result.data;
        }
        const occupied = rentals.length;
        // Obtener todas las páginas de edificios para el total real
        this.getAllBuildings().then((allBuildings) => {
          let totalApartments = allBuildings.reduce((acc: number, b: any) => acc + (b.apartmentsCount || b.totalApartments || 0), 0);
          this.totalBuildings = allBuildings.length;
          this.occupancyRate = totalApartments > 0 ? Math.round((occupied / totalApartments) * 100) : 0;
        }).catch((err) => {
          console.error('Error getAllBuildings:', err);
          this.occupancyRate = 0;
        });
  /**
   * Obtiene todos los edificios paginados del backend
   */
  private async getAllBuildings(): Promise<any[]> {
    let all: any[] = [];
    let page = 1;
    let totalPages = 1;
    do {
      const res = await this.buildingsService.getBuildings({ page }).toPromise();
      const items = Array.isArray(res?.items) ? res.items : [];
      all = all.concat(items);
      totalPages = res?.totalPages || 1;
      page++;
    } while (page <= totalPages);
    return all;
  }
  /**
   * Obtiene todos los edificios paginados del backend
   */
  private async getAllBuildings(): Promise<any[]> {
    let all: any[] = [];
    let page = 1;
    let totalPages = 1;
    do {
      const res = await this.buildingsService.getBuildings({ page }).toPromise();
      const items = Array.isArray(res?.items) ? res.items : [];
                this.getAllBuildings().then((allBuildings) => {
                  let totalApartments = allBuildings.reduce((acc: number, b: any) => acc + (b.apartmentsCount || b.totalApartments || 0), 0);
                  this.totalBuildings = allBuildings.length;
                  this.occupancyRate = totalApartments > 0 ? Math.round((occupied / totalApartments) * 100) : 0;
                }).catch((err) => {
                  console.error('Error getAllBuildings:', err);
                  this.occupancyRate = 0;
                });
 * Obtiene todos los edificios paginados del backend
 */
// Debe estar fuera de cualquier función, como método privado de la clase
// pero aquí lo dejamos como función auxiliar para evitar errores de contexto
DashboardComponent.prototype.getAllBuildings = async function(): Promise<any[]> {
  let all: any[] = [];
  let page = 1;
  let totalPages = 1;
  do {
    const res = await this.buildingsService.getBuildings({ page }).toPromise();
    const items = Array.isArray(res?.items) ? res.items : [];
    all = all.concat(items);
    totalPages = res?.totalPages || 1;
    page++;
  } while (page <= totalPages);
  return all;
};
      },
      error: (err) => {
        console.error('Error getRentals:', err);
        this.occupancyRate = 0;
      }
    });
  }
}
