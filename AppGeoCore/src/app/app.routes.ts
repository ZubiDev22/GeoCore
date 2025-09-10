import { GestionComponent } from './pages/gestion.component';
import { Routes } from '@angular/router';
import { BuildingDetailComponent } from './pages/building-detail.component';
import { BuildingFormComponent } from './pages/building-form.component';
import { DashboardComponent } from './pages/dashboard.component';
import { BuildingsComponent } from './pages/buildings.component';
import { ApartmentsComponent } from './pages/apartments.component';
import { RentalsComponent } from './pages/rentals.component';
import { MaintenanceEventsComponent } from './pages/maintenance-events.component';
import { CashFlowsComponent } from './pages/cashflows.component';
import { ProfitabilityComponent } from './pages/profitability.component';

export const routes: Routes = [
	{ path: 'gestion', component: GestionComponent },
	{ path: 'dashboard', component: DashboardComponent },
	{ path: 'buildings', component: BuildingsComponent },
	{ path: 'buildings/new', component: BuildingFormComponent },
	{ path: 'buildings/:code/edit', component: BuildingFormComponent, data: { isEdit: true } },
	{ path: 'buildings/:code', component: BuildingDetailComponent },
	{ path: 'apartments', component: ApartmentsComponent },
	{ path: 'rentals', component: RentalsComponent },
	{ path: 'maintenance-events', component: MaintenanceEventsComponent },
	{ path: 'cashflows', component: CashFlowsComponent },
	{ path: 'profitability', component: ProfitabilityComponent },
	{ path: '', redirectTo: '/dashboard', pathMatch: 'full' },
	{ path: '**', redirectTo: '/dashboard' }
];
