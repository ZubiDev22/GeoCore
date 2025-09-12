
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-apartments',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="apartments-wrapper">
      <h2 class="mb-4"><i class="bi bi-door-open"></i> Apartamentos</h2>
      <div class="row mb-3 g-2 align-items-end">
        <div class="col-md-4">
          <label for="filtroEdificio" class="form-label">Filtrar por edificio:</label>
          <input id="filtroEdificio" type="text" class="form-control" placeholder="Nombre del edificio" [(ngModel)]="filterBuilding">
        </div>
        <div class="col-md-4">
          <label for="busqueda" class="form-label">Buscar:</label>
          <input id="busqueda" type="text" class="form-control" placeholder="Apartamento o inquilino" [(ngModel)]="searchText">
        </div>
      </div>
      <table class="table table-striped table-hover">
        <thead>
          <tr>
            <th>Edificio</th>
            <th>Apartamento</th>
            <th>Inquilino</th>
            <th>Superficie (m²)</th>
            <th>Estado</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let apt of paginatedApartments()">
            <td>{{ apt.building }}</td>
            <td>{{ apt.name }}</td>
            <td>{{ apt.tenant }}</td>
            <td>{{ apt.area }}</td>
            <td>
              <span [ngClass]="{
                'badge bg-success': apt.status === 'ocupado',
                'badge bg-secondary': apt.status === 'libre',
                'badge bg-warning text-dark': apt.status === 'reservado'
              }">{{ apt.status }}</span>
            </td>
            <td>
              <button class="btn btn-outline-primary btn-sm" (click)="verDetalles(apt)"><i class="bi bi-eye"></i> Detalles</button>
            </td>
          </tr>
        </tbody>
      </table>
      <nav *ngIf="filteredApartments().length > pageSize" aria-label="Paginación de apartamentos">
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
export class ApartmentsComponent {
  filterBuilding = '';
  searchText = '';
  currentPage = 1;
  pageSize = 5;
  apartments = [
    {
      building: 'Edificio Sol',
      name: 'Apto. 1A',
      tenant: 'Juan Pérez',
      area: 80,
      status: 'ocupado'
    },
    {
      building: 'Edificio Luna',
      name: 'Apto. 2B',
      tenant: 'María López',
      area: 65,
      status: 'ocupado'
    },
    {
      building: 'Edificio Estrella',
      name: 'Apto. 3C',
      tenant: '',
      area: 70,
      status: 'libre'
    },
    {
      building: 'Edificio Sol',
      name: 'Apto. 1B',
      tenant: '',
      area: 75,
      status: 'reservado'
    },
    {
      building: 'Edificio Luna',
      name: 'Apto. 2A',
      tenant: 'Carlos Ruiz',
      area: 60,
      status: 'ocupado'
    },
    {
      building: 'Edificio Estrella',
      name: 'Apto. 3A',
      tenant: '',
      area: 68,
      status: 'libre'
    }
  ];

  filteredApartments() {
    return this.apartments.filter(apt => {
      const matchesBuilding = this.filterBuilding ? apt.building.toLowerCase().includes(this.filterBuilding.toLowerCase()) : true;
      const matchesText = this.searchText
        ? (apt.name + ' ' + apt.tenant).toLowerCase().includes(this.searchText.toLowerCase())
        : true;
      return matchesBuilding && matchesText;
    });
  }

  paginatedApartments() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredApartments().slice(start, start + this.pageSize);
  }

  totalPages() {
    return Math.ceil(this.filteredApartments().length / this.pageSize) || 1;
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

  verDetalles(apt: any) {
    alert('Detalles de apartamento: ' + apt.name + '\nEdificio: ' + apt.building);
  }
}
