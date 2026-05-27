import { Component } from '@angular/core';
import { RatesTableComponent } from './components/rates-table/rates-table.component';
import { AlertsPanelComponent } from './components/alerts-panel/alerts-panel.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RatesTableComponent, AlertsPanelComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {}