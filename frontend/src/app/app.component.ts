import { Component } from '@angular/core';
import { RatesTableComponent } from './components/rates-table/rates-table.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RatesTableComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {}