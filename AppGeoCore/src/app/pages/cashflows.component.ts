
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cashflows',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="cashflows-wrapper">
      <h2 class="mb-4"><i class="bi bi-graph-up-arrow"></i> Flujos de Caja</h2>
      <div class="row mb-3 g-2 align-items-end">
        <div class="col-md-4">
          <label for="filtroTipo" class="form-label">Filtrar por tipo:</label>
          <select id="filtroTipo" class="form-select" [(ngModel)]="filterType">
            <option value="">Todos</option>
            <option value="ingreso">Ingreso</option>
            <option value="gasto">Gasto</option>
          </select>
        </div>
        <div class="col-md-4">
          <label for="busqueda" class="form-label">Buscar:</label>
          <input id="busqueda" type="text" class="form-control" placeholder="Concepto o edificio" [(ngModel)]="searchText">
        </div>
      </div>
      <table class="table table-striped table-hover">
        <thead>
          <tr>
            <th>Fecha</th>
            <th>Edificio</th>
            <th>Concepto</th>
            <th>Tipo</th>
            <th>Cantidad (€)</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let cashflow of paginatedCashflows()">
            <td>{{ cashflow.date | date:'dd/MM/yyyy' }}</td>
            <td>{{ cashflow.building }}</td>
            <td>{{ cashflow.concept }}</td>
            <td>
              <span class="badge"
             [class.badge-tipo-ingreso]="cashflow.type === 'ingreso'"
             [class.badge-tipo-gasto]="cashflow.type === 'gasto'"
              >{{ cashflow.type }}</span>
            </td>
            <td [ngClass]="{'text-success': cashflow.type === 'ingreso', 'text-danger': cashflow.type === 'gasto'}">
              {{ cashflow.amount | number:'1.0-2' }}
            </td>
            <td>
              <button class="btn btn-outline-primary btn-sm" (click)="verDetalles(cashflow)"><i class="bi bi-eye"></i> Detalles</button>
            </td>
          </tr>
        </tbody>
      </table>
      <nav *ngIf="filteredCashflows().length > pageSize" aria-label="Paginación de flujos de caja">
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
export class CashFlowsComponent {
  filterType = '';
  searchText = '';
  currentPage = 1;
  pageSize = 5;
  cashflows = [
    {
      date: new Date(2024, 0, 10),
      building: 'Edificio Sol',
      concept: 'Cobro alquiler',
      type: 'ingreso',
      amount: 1200
    },
    {
      date: new Date(2024, 0, 15),
      building: 'Edificio Luna',
      concept: 'Pago limpieza',
      type: 'gasto',
      amount: 150
    },
    {
      date: new Date(2024, 1, 5),
      building: 'Edificio Estrella',
      concept: 'Cobro alquiler',
      type: 'ingreso',
      amount: 950
    },
    {
      date: new Date(2024, 1, 20),
      building: 'Edificio Sol',
      concept: 'Reparación fontanería',
      type: 'gasto',
      amount: 300
    },
    {
      date: new Date(2024, 2, 1),
      building: 'Edificio Luna',
      concept: 'Cobro alquiler',
      type: 'ingreso',
      amount: 950
    },
    {
      date: new Date(2024, 2, 10),
      building: 'Edificio Estrella',
      concept: 'Pago mantenimiento',
      type: 'gasto',
      amount: 200
    }
  ];

  filteredCashflows() {
    return this.cashflows.filter(cf => {
      const matchesType = this.filterType ? cf.type === this.filterType : true;
      const matchesText = this.searchText
        ? (cf.concept + ' ' + cf.building).toLowerCase().includes(this.searchText.toLowerCase())
        : true;
      return matchesType && matchesText;
    });
  }

  paginatedCashflows() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredCashflows().slice(start, start + this.pageSize);
  }

  totalPages() {
    return Math.ceil(this.filteredCashflows().length / this.pageSize) || 1;
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

  verDetalles(cashflow: any) {
    alert('Detalles de flujo de caja para: ' + cashflow.concept + '\nEdificio: ' + cashflow.building);
  }
}
