import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
          <tr *ngFor="let rental of paginatedRentals()">
            <td>{{ rental.tenant }}</td>
            <td>{{ rental.property }}</td>
            <td>{{ rental.price | number:'1.0-2' }}</td>
            <td>{{ rental.start | date:'dd/MM/yyyy' }}</td>
            <td>{{ rental.end | date:'dd/MM/yyyy' }}</td>
            <td>
              <span [ngClass]="{
                'badge bg-success': rental.status === 'activo',
                'badge bg-warning text-dark': rental.status === 'pendiente',
                'badge bg-danger': rental.status === 'vencido'
              }">{{ rental.status }}</span>
            </td>
            <td>
              <button class="btn btn-outline-primary btn-sm" (click)="verDetalles(rental)"><i class="bi bi-eye"></i> Detalles</button>
            </td>
          </tr>
        </tbody>
      </table>
      <nav *ngIf="filteredRentals().length > pageSize" aria-label="Paginación de alquileres">
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
export class RentalsComponent {
  filterStatus = '';
  searchText = '';
  currentPage = 1;
  pageSize = 5;
  rentals = [
    {
      tenant: 'Juan Pérez',
      property: 'Apto. 1A - Edificio Sol',
      price: 850,
      start: new Date(2024, 0, 1),
      end: new Date(2024, 11, 31),
      status: 'activo'
    },
    {
      tenant: 'María López',
      property: 'Apto. 2B - Edificio Luna',
      price: 950,
      start: new Date(2024, 2, 15),
      end: new Date(2025, 2, 14),
      status: 'pendiente'
    },
    {
      tenant: 'Carlos Ruiz',
      property: 'Apto. 3C - Edificio Estrella',
      price: 780,
      start: new Date(2023, 5, 1),
      end: new Date(2024, 4, 31),
      status: 'vencido'
    }
  ];

  filteredRentals() {
    return this.rentals.filter(rental => {
      const matchesStatus = this.filterStatus ? rental.status === this.filterStatus : true;
      const matchesText = this.searchText
        ? (rental.tenant + ' ' + rental.property).toLowerCase().includes(this.searchText.toLowerCase())
        : true;
      return matchesStatus && matchesText;
    });
  }

  paginatedRentals() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredRentals().slice(start, start + this.pageSize);
  }

  totalPages() {
    return Math.ceil(this.filteredRentals().length / this.pageSize) || 1;
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

  verDetalles(rental: any) {
    alert('Detalles de alquiler para: ' + rental.tenant + '\nPropiedad: ' + rental.property);
  }
}
