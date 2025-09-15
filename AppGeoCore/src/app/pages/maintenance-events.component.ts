import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe, CurrencyPipe } from '@angular/common';
import { MaintenanceEventsService } from '../services/maintenance-events.service';
import { BuildingsService } from '../services/buildings.service';

@Component({
  selector: 'app-maintenance-events',
  standalone: true,
  imports: [CommonModule, DatePipe, CurrencyPipe],
  template: `
    <div class="container mt-4">
      <h2>Eventos de Mantenimiento</h2>
      <table class="table table-striped" *ngIf="events.length">
        <thead>
          <tr>
            <th>ID</th>
            <th>Edificio</th>
            <th>Fecha</th>
            <th>Descripción</th>
            <th>Costo</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let e of events">
            <td>{{ e.maintenanceEventId || e.MaintenanceEventId }}</td>
            <td>{{ getBuildingName(e.buildingId || e.BuildingId) }}</td>
            <td>{{ e.date || e.Date | date:'yyyy-MM-dd' }}</td>
            <td>{{ e.description || e.Description }}</td>
            <td>{{ e.cost || e.Cost | currency }}</td>
            <td><button class="btn btn-link" (click)="showDetail(e)">Ver detalle</button></td>
          </tr>
        </tbody>
      </table>
      <div *ngIf="!events.length" class="text-muted">Sin eventos de mantenimiento.</div>

      <div *ngIf="selectedEvent" class="mt-4 card card-body">
        <h4>Detalle del evento</h4>
        <div><strong>ID:</strong> {{ selectedEvent.maintenanceEventId || selectedEvent.MaintenanceEventId }}</div>
        <div><strong>Edificio:</strong> {{ getBuildingName(selectedEvent.buildingId || selectedEvent.BuildingId) }}</div>
        <div><strong>Fecha:</strong> {{ (selectedEvent.date || selectedEvent.Date) | date:'yyyy-MM-dd' }}</div>
        <div><strong>Descripción:</strong> {{ selectedEvent.description || selectedEvent.Description }}</div>
        <div><strong>Costo:</strong> {{ (selectedEvent.cost || selectedEvent.Cost) | currency }}</div>
        <button class="btn btn-secondary mt-2" (click)="selectedEvent = null">Cerrar</button>
      </div>
    </div>
  `
})
export class MaintenanceEventsComponent implements OnInit {
  events: any[] = [];
  buildings: { [id: string]: string } = {};
  selectedEvent: any = null;

  constructor(
    private maintenanceEventsService: MaintenanceEventsService,
    private buildingsService: BuildingsService
  ) {}

  ngOnInit() {
    this.maintenanceEventsService.getMaintenanceEvents().subscribe((data: any) => {
      console.log('DEBUG eventos:', data);
      let eventos: any[] = [];
      if (Array.isArray(data)) {
        eventos = data;
      } else if (data && Array.isArray(data.items)) {
        eventos = data.items;
      } else if (data && Array.isArray(data.events)) {
        eventos = data.events;
      } else if (data && typeof data === 'object') {
        // Si el objeto tiene solo una propiedad, y esa propiedad es un array, úsala
        const arr = Object.values(data).find(v => Array.isArray(v));
        if (arr) eventos = arr as any[];
      }
      this.events = eventos;
      // Cargar nombres de edificios solo si hay eventos y BuildingId definido
      if (eventos.length) {
        const ids = Array.from(new Set(eventos.map((e: any) => e.BuildingId).filter((id: any) => !!id)));
        ids.forEach((id: any) => {
          this.buildingsService.getBuildingById(id).subscribe((b: any) => {
            this.buildings[id] = b.name || b.Name || b.buildingName || b.BuildingName || id;
          });
        });
      }
    }, (err) => {
      console.error('Error cargando eventos de mantenimiento:', err);
      this.events = [];
    });
  }

  getBuildingName(id: string): string {
  if (!id) return '(sin edificio)';
  if (this.buildings[id] === undefined) return id;
  return this.buildings[id] || id;
  }

  showDetail(event: any) {
    this.selectedEvent = event;
  }
}
