import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-gestion',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="container mt-5">
      <h2 class="mb-4">Gestión</h2>
      <div class="row g-4">
        <div class="col-md-3 col-6">
          <div class="card gestion-card h-100" (click)="go('buildings')">
            <div class="card-body text-center">
              <i class="bi bi-building display-4 text-primary"></i>
              <h5 class="mt-3">Edificios</h5>
            </div>
          </div>
        </div>
        <div class="col-md-3 col-6">
          <div class="card gestion-card h-100" (click)="go('apartments')">
            <div class="card-body text-center">
              <i class="bi bi-door-open display-4 text-success"></i>
              <h5 class="mt-3">Apartamentos</h5>
            </div>
          </div>
        </div>
        <div class="col-md-3 col-6">
          <div class="card gestion-card h-100" (click)="go('maintenance-events')">
            <div class="card-body text-center">
              <i class="bi bi-tools display-4 text-warning"></i>
              <h5 class="mt-3">Mantenimientos</h5>
            </div>
          </div>
        </div>
        <div class="col-md-3 col-6">
          <div class="card gestion-card h-100" (click)="go('cashflows')">
            <div class="card-body text-center">
              <i class="bi bi-graph-up-arrow display-4 text-info"></i>
              <h5 class="mt-3">Flujos de Caja</h5>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .gestion-card { cursor: pointer; transition: box-shadow 0.2s, transform 0.2s; border-radius: 1rem; }
    .gestion-card:hover { box-shadow: 0 4px 24px rgba(25,118,210,0.13); transform: translateY(-4px) scale(1.03); }
    .card-body i { font-size: 2.5rem; }
    .card-body h5 { font-weight: 600; }
  `]
})
export class GestionComponent {
  constructor(private router: Router) {}
  go(path: string) {
    this.router.navigate(['/' + path]);
  }
}
