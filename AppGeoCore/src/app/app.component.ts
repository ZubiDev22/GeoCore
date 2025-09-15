
import { Component, signal } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [
		RouterOutlet,
		HeaderComponent,
		FooterComponent,
		HttpClientModule
	],
	templateUrl: './app.component.html',
	styleUrls: ['./app.scss']
})
export class AppComponent {
	title = 'app-geo-core';
}
