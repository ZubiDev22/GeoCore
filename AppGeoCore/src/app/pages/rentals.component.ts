
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RentalsService } from '../services/rentals.service';

@Component({
  selector: 'app-rentals',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="rentals-wrapper">
      <h2 class="mb-4"><i class="bi bi-cash-coin"></i> Alquileres</h2>
      <div class="row mb-3 g-2 align-items-end">
        <div class="col-md-4">
          <label for="estado" class="form-label">Filtrar por estado:</label>
          <select id="estado" class="form-select" [(ngModel)]="filterStatus">
            <option value="">Todos</option>
            <option value="activo">Activo</option>
            <option value="pendiente">Pendiente</option>
            <option value="vencido">Vencido</option>
          </select>
        </div>
        <div class="col-md-4">
          <label for="busqueda" class="form-label">Buscar:</label>
          <input id="busqueda" type="text" class="form-control" placeholder="Inquilino o propiedad" [(ngModel)]="searchText">
        </div>
      </div>
      <table class="table table-striped table-hover">
        <thead>
          <tr>
            <th>Tipo</th>
            <th>Inquilino</th>
            <th>Propiedad</th>
            <th>Precio (€)</th>
            <th>Inicio</th>
            <th>Fin</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let item of paginatedCombined()">
            <td>
              <span *ngIf="item.tipo === 'Rental'" class="badge bg-primary">Rental</span>
              <span *ngIf="item.tipo === 'CashFlow'" class="badge bg-success">CashFlow</span>
            </td>
            <td>{{ item.tenant || '-' }}</td>
            <td>{{ item.property || '-' }}</td>
            <td>{{ item.price | number:'1.0-2' }}</td>
            <td>{{ item.start | date:'dd/MM/yyyy' }}</td>
            <td>{{ item.end | date:'dd/MM/yyyy' }}</td>
            <td>
              <span *ngIf="item.status" class="badge"
                [class.badge-estado-activo]="item.status === 'activo'"
                [class.badge-estado-pendiente]="item.status === 'pendiente'"
                [class.badge-estado-vencido]="item.status === 'vencido'"
              >{{ item.status }}</span>
              <span *ngIf="!item.status">-</span>
            </td>
            <td>
              <button class="btn btn-outline-primary btn-sm" (click)="verDetalles(item)"><i class="bi bi-eye"></i> Detalles</button>
            </td>
          </tr>
        </tbody>
      </table>
  <nav *ngIf="filteredCombined().length > pageSize" aria-label="Paginación de ingresos">
        <ul class="pagination justify-content-center">
          <li class="page-item" [class.disabled]="currentPage === 1">
            <button class="page-link" (click)="prevPage()" [disabled]="currentPage === 1">Anterior</button>
          </li>
          <li class="page-item" *ngFor="let page of totalPagesArray(); let i = index" [class.active]="currentPage === (i+1)">
            <button class="page-link" (click)="goToPage(i+1)">{{ i+1 }}</button>
          </li>
          <li class="page-item" [class.disabled]="currentPage === totalPages()">
            <button class="page-link" (click)="nextPage()" [disabled]="currentPage === totalPages()">Siguiente</button>
          </li>
        </ul>
      </nav>
    </div>
  `,
  styleUrls: []
})
export class RentalsComponent implements OnInit {
  filterStatus = '';
  searchText = '';
  currentPage = 1;
  pageSize = 5;
  combined: any[] = [];
  loading = false;
  error: string | null = null;

  constructor(private rentalsService: RentalsService) {}

  ngOnInit(): void {
    this.loading = true;
    this.rentalsService.getCombinedRentalsCashflows().subscribe({
      next: (data) => {
        // Normalizar los datos para la tabla unificada
        this.combined = (Array.isArray(data) ? data : (data?.value || data?.data || []))
          .map((item: any) => {
            if (item.RentalId) {
              // Es un Rental
              return {
                tipo: 'Rental',
                tenant: item.TenantName || item.tenant || '-',
                property: item.PropertyName || item.property || '-',
                price: item.Price || item.price || 0,
                start: item.StartDate ? new Date(item.StartDate) : null,
                end: item.EndDate ? new Date(item.EndDate) : null,
                status: item.Status || item.status || '-',
                ...item
              };
            } else if (item.CashFlowId) {
              // Es un CashFlow
              return {
                tipo: 'CashFlow',
                tenant: item.TenantName || '-',
                property: item.PropertyName || '-',
                price: item.Amount || 0,
                start: item.Date ? new Date(item.Date) : null,
                end: null,
                status: null,
                ...item
              };
            }
            return item;
          });
        this.loading = false;
      },
      error: (err) => {
        let msg = 'No se pudieron cargar los ingresos combinados.';
        if (typeof err === 'string') {
          msg += ` Detalle: ${err}`;
        }
        this.error = msg;
        this.loading = false;
      }
    });
  }

  filteredCombined() {
    return this.combined.filter(item => {
      const matchesStatus = this.filterStatus ? item.status === this.filterStatus : true;
      const matchesText = this.searchText
        ? ((item.tenant + ' ' + item.property).toLowerCase().includes(this.searchText.toLowerCase()))
        : true;
      return matchesStatus && matchesText;
    });
  }

  paginatedCombined() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredCombined().slice(start, start + this.pageSize);
  }

  totalPages() {
    return Math.ceil(this.filteredCombined().length / this.pageSize) || 1;
  }

  totalPagesArray() {
    return Array(this.totalPages());
  }

  goToPage(page: number) {
    this.currentPage = page;
  }

  prevPage() {
    if (this.currentPage > 1) this.currentPage--;
  }

  nextPage() {
    if (this.currentPage < this.totalPages()) this.currentPage++;
  }

  verDetalles(item: any) {
    alert('Detalles para: ' + (item.tenant || '-') + '\nPropiedad: ' + (item.property || '-') + '\nTipo: ' + item.tipo);
  }
}
