// Ejemplo de uso del servicio GeocodingService en un componente Angular
import { Component } from '@angular/core';
import { GeocodingService } from '../services/geocoding.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-geocoding-demo',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div>
      <input [(ngModel)]="street" placeholder="Calle" />
      <input [(ngModel)]="number" placeholder="Número" style="width: 80px;" />
      <input [(ngModel)]="city" placeholder="Ciudad" />
      <input [(ngModel)]="province" placeholder="Provincia" />
      <input [(ngModel)]="postalCode" placeholder="Código Postal" style="width: 120px;" />
      <input [(ngModel)]="country" placeholder="País" />
      <br />
      <button (click)="buscarCoordenadas()">Buscar coordenadas</button>
      <div *ngIf="coords as c">
        <strong>Latitud:</strong> {{ c.lat }}<br />
        <strong>Longitud:</strong> {{ c.lng }}
      </div>
      <div *ngIf="addressCompleta">
        <em>Dirección enviada a Google:</em> {{ addressCompleta }}
      </div>
    </div>
  `
})
export class GeocodingDemoComponent {

  street = '';
  number = '';
  city = '';
  province = '';
  postalCode = '';
  country = '';
  addressCompleta = '';
  coords: { lat: number, lng: number } | null = null;

  constructor(private geocoding: GeocodingService) {}


  buscarCoordenadas() {
    // Forzar provincia y país según ciudad
    const cityToProvince: Record<string, string> = {
      'Madrid': 'Madrid',
      'Barcelona': 'Barcelona',
      'Valencia': 'Valencia',
      'Sevilla': 'Sevilla',
      'Zaragoza': 'Zaragoza',
      'Málaga': 'Málaga',
      'Murcia': 'Murcia',
      'Palma': 'Islas Baleares',
      'Palma de Mallorca': 'Islas Baleares',
      'Las Palmas': 'Las Palmas',
      'Bilbao': 'Bizkaia',
      'Alicante': 'Alicante',
      'Córdoba': 'Córdoba',
      'Valladolid': 'Valladolid',
      'Vigo': 'Pontevedra',
      'Gijón': 'Asturias',
      'Hospitalet de Llobregat': 'Barcelona',
      'A Coruña': 'A Coruña',
      'Vitoria-Gasteiz': 'Álava',
      'Granada': 'Granada',
      'Elche': 'Alicante',
      'Oviedo': 'Asturias',
      'Badalona': 'Barcelona',
      'Cartagena': 'Murcia',
      'Terrassa': 'Barcelona',
      'Jerez de la Frontera': 'Cádiz',
      'Sabadell': 'Barcelona',
      'Móstoles': 'Madrid',
      'Santa Cruz de Tenerife': 'Santa Cruz de Tenerife',
      'Alcalá de Henares': 'Madrid',
      'Pamplona': 'Navarra',
      'Pamplona/Iruña': 'Navarra',
      'San Sebastián': 'Gipuzkoa',
      'Santander': 'Cantabria',
      'Logroño': 'La Rioja',
      'Toledo': 'Toledo',
      'Badajoz': 'Badajoz',
      'Cáceres': 'Cáceres',
      'Salamanca': 'Salamanca',
      'León': 'León',
      'Burgos': 'Burgos',
      'Segovia': 'Segovia',
      'Soria': 'Soria',
      'Zamora': 'Zamora',
      'Palencia': 'Palencia',
      'Ávila': 'Ávila',
      'Guadalajara': 'Guadalajara',
      'Cuenca': 'Cuenca',
      'Ciudad Real': 'Ciudad Real',
      'Albacete': 'Albacete',
      'Castellón': 'Castellón',
      'Tarragona': 'Tarragona',
      'Lleida': 'Lleida',
      'Girona': 'Girona',
      'Huesca': 'Huesca',
      'Teruel': 'Teruel',
      'Ourense': 'Ourense',
      'Lugo': 'Lugo',
      'Pontevedra': 'Pontevedra',
      'Melilla': 'Melilla',
      'Ceuta': 'Ceuta'
    };
    // Si la ciudad está en el listado, forzar provincia y país
    const cityNorm = this.city.trim();
    this.province = cityToProvince[cityNorm] || this.province;
    this.country = 'España';
    this.addressCompleta = this.buildFullAddress();
    this.geocoding.getCoordinates(this.addressCompleta).subscribe(resp => {
      if (resp.status === 'OK' && resp.results.length > 0) {
        this.coords = resp.results[0].geometry.location;
      } else {
        this.coords = null;
      }
    });
  }

  buildFullAddress(): string {
    const parts = [
      this.street,
      this.number,
      this.city,
      this.province,
      this.postalCode,
      this.country
    ];
    return parts.filter(Boolean).join(', ');
  }
}
