import { Component } from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { DashboardMapComponent } from './dashboard-map.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, DashboardMapComponent],
  providers: [CurrencyPipe],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  totalBuildings: number = 0;
  occupancyRate: number = 0;
  totalIncome: number = 0;
  avgProfitability: number = 0;

}
