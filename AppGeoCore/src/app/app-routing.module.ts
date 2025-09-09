import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home.component';
// Aquí puedes importar otros componentes de página cuando los crees

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  // { path: 'peticiones', component: PeticionesComponent },
  // { path: 'presupuestos', component: PresupuestosComponent },
  // { path: 'historial', component: HistorialComponent },
  // { path: 'patrimonio', component: PatrimonioComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
