import { Component } from '@angular/core';

@Component({
  selector: 'app-rentals',
  standalone: true,
  template: `
    <div class="rentals-wrapper d-flex align-items-center justify-content-center">
      <div class="rentals-card text-center">
        <i class="bi bi-cash-coin display-1 text-success mb-3"></i>
        <h2 class="mb-3">Gestión de Alquileres</h2>
        <p class="lead mb-4">Administra contratos, pagos y vencimientos de alquileres fácilmente.</p>
        <a routerLink="/rentals/new" class="btn btn-success btn-lg"><i class="bi bi-plus-circle"></i> Nuevo alquiler</a>
      </div>
    </div>
  `,
  styleUrls: []
})
export class RentalsComponent {}
