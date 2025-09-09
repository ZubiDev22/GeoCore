import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BuildingsService } from '../services/buildings.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-building-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container mt-4">
      <h2>{{ isEdit ? 'Editar' : 'Crear' }} Edificio</h2>
      <form (ngSubmit)="onSubmit()" #form="ngForm">
        <div class="row g-3">
          <div class="col-md-6">
            <label class="form-label">Código</label>
            <input class="form-control" name="buildingCode" [(ngModel)]="model.buildingCode" [readonly]="isEdit" required />
          </div>
          <div class="col-md-6">
            <label class="form-label">Nombre</label>
            <input class="form-control" name="name" [(ngModel)]="model.name" required />
          </div>
          <div class="col-md-6">
            <label class="form-label">Dirección</label>
            <input class="form-control" name="address" [(ngModel)]="model.address" required />
          </div>
          <div class="col-md-6">
            <label class="form-label">Ciudad</label>
            <input class="form-control" name="city" [(ngModel)]="model.city" required />
          </div>
          <div class="col-md-4">
            <label class="form-label">Latitud</label>
            <input class="form-control" name="latitude" [(ngModel)]="model.latitude" type="number" step="any" />
          </div>
          <div class="col-md-4">
            <label class="form-label">Longitud</label>
            <input class="form-control" name="longitude" [(ngModel)]="model.longitude" type="number" step="any" />
          </div>
          <div class="col-md-4">
            <label class="form-label">Fecha de compra</label>
            <input class="form-control" name="purchaseDate" [(ngModel)]="model.purchaseDate" type="date" required />
          </div>
          <div class="col-md-4">
            <label class="form-label">Estado</label>
            <select class="form-select" name="status" [(ngModel)]="model.status" required>
              <option value="Active">Activo</option>
              <option value="Maintenance">Mantenimiento</option>
              <option value="Inactive">Inactivo</option>
            </select>
          </div>
        </div>
        <div class="mt-4">
          <button class="btn btn-primary" type="submit" [disabled]="form.invalid || loading">{{ isEdit ? 'Guardar cambios' : 'Crear edificio' }}</button>
          <span *ngIf="loading" class="ms-3 spinner-border spinner-border-sm"></span>
        </div>
        <div *ngIf="error" class="alert alert-danger mt-3">{{ error }}</div>
      </form>
    </div>
  `,
  styleUrls: []
})
export class BuildingFormComponent implements OnInit {
  isEdit = false;
  model: any = {
    buildingCode: '', name: '', address: '', city: '', latitude: '', longitude: '', purchaseDate: '', status: 'Active'
  };
  loading = false;
  error = '';

  constructor(
    private buildingsService: BuildingsService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.isEdit = this.route.snapshot.data['isEdit'] || false;
    const code = this.route.snapshot.paramMap.get('code');
    if (this.isEdit && code) {
      this.loading = true;
      this.buildingsService.getBuildingByCode(code).subscribe({
        next: (data) => {
          this.model = { ...data };
          this.loading = false;
        },
        error: (err) => {
          this.error = 'No se pudo cargar el edificio';
          this.loading = false;
        }
      });
    }
  }

  onSubmit() {
    this.loading = true;
    this.error = '';
    const action = this.isEdit
      ? this.buildingsService.updateBuilding(this.model.buildingCode, this.model)
      : this.buildingsService.createBuilding(this.model);
    action.subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/buildings']);
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.message || 'Error al guardar edificio';
      }
    });
  }
}
