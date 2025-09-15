import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-presentacion',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './presentacion.component.html',
  styleUrls: ['./presentacion.component.scss']
})
export class PresentacionComponent {
  images = [
    'assets/image_1.jpg',
    'assets/image_2.jpg',
    'assets/image_3.jpg'
  ];
  current = 0;
  intervalId: any;

  ngOnInit() {
    this.startCarousel();
  }
  ngOnDestroy() {
    clearInterval(this.intervalId);
  }
  startCarousel() {
    this.intervalId = setInterval(() => {
      this.next();
    }, 6000);
  }
  next() {
    this.current = (this.current + 1) % this.images.length;
  }
  prev() {
    this.current = (this.current - 1 + this.images.length) % this.images.length;
  }
  setImage(idx: number) {
    this.current = idx;
  }
  get currentImage() {
    return this.images[this.current];
  }
}
