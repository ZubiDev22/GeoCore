import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { BuildingMapLeafletComponent } from './building-map-leaflet.component';

@Component({
  selector: 'app-building-map',
  standalone: true,
  imports: [BuildingMapLeafletComponent],
  template: `
    <app-building-map-leaflet [latitude]="latitude" [longitude]="longitude" [zoom]="zoom"></app-building-map-leaflet>
  `
})
export class BuildingMapComponent implements OnChanges {
  @Input() latitude: number = 0;
  @Input() longitude: number = 0;
  @Input() zoom: number = 17;
  ngOnChanges(changes: SimpleChanges) {}
}
