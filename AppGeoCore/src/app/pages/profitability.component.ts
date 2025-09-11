import { Component } from '@angular/core';

@Component({
  selector: 'app-profitability',
  standalone: true,
  template: `
    <div class="profit-wrapper d-flex align-items-center justify-content-center">
      <div class="profit-card text-center">
        <i class="bi bi-bar-chart display-1 text-info mb-3"></i>
        <h2 class="mb-3">Reportes de Rentabilidad</h2>
        <p class="lead mb-4">Visualiza reportes y análisis financieros de tus propiedades.</p>
        <a routerLink="/profitability/detail" class="btn btn-info btn-lg text-white"><i class="bi bi-graph-up"></i> Ver detalles</a>
      </div>
    </div>
  `,
  styles: [`
    .profit-wrapper { min-height: 60vh; display: flex; align-items: center; justify-content: center; }
    .profit-card { background: #fff; border-radius: 1.5rem; box-shadow: 0 4px 24px rgba(25,118,210,0.10); padding: 3rem 2.5rem; max-width: 520px; width: 100%; }
    .profit-card i { font-size: 3.5rem; }
  `]
})
export class ProfitabilityComponent {}
