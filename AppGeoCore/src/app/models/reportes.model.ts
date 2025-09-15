// Modelos para Alquileres y Reportes según los DTOs del backend

export interface RentalDto {
  RentalId: string;
  ApartmentId: string;
  StartDate: string; // formato "dd/MM/yyyy"
  EndDate: string;   // formato "dd/MM/yyyy"
  IsConfirmed: boolean;
  Price: number;
  Zone: string;
  PostalCode: string;
}

export interface RentalSummaryDto {
  RentalId: string;
  ApartmentId: string;
  StartDate: string;
  EndDate: string;
  Price: number;
  Zone: string;
  PostalCode: string;
}

export interface CashFlowSummaryDto {
  CashFlowId: string;
  Source: string;
  Amount: number;
  Date: string;
}

export interface MaintenanceSummaryDto {
  MaintenanceEventId: string;
  Date: string;
  Description: string;
  Cost: number;
}

export interface ProfitabilityDto {
  BuildingCode: string;
  Ingresos: number;
  Gastos: number;
  Inversion: number;
  Rentabilidad: string;
  Detalle: {
    Alquileres: RentalSummaryDto[];
    CashFlows: CashFlowSummaryDto[];
    Mantenimientos: MaintenanceSummaryDto[];
  };
}

export interface ProfitabilityByLocationDetailDto {
  BuildingCode: string;
  Ingresos: number;
  Gastos: number;
  Inversion: number;
  Rentabilidad: string;
  Address?: string;
  City?: string;
  PostalCode?: string;
}

export interface ProfitabilityByLocationDto {
  TotalEdificios: number;
  TotalIngresos: number;
  TotalGastos: number;
  TotalInversion: number;
  RentabilidadMedia: string;
  Detalle: ProfitabilityByLocationDetailDto[];
  EscalaBaremosDescripcion?: string; // Descripción de la escala de baremos enviada por el backend
}

// Result wrapper según backend
export interface Result<T> {
  success: boolean;
  message?: string;
  data: T;
}
