import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { Alert, AlertInput, AlertDirection } from '../../models';

@Component({
  selector: 'app-alerts-panel',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './alerts-panel.component.html',
  styleUrl: './alerts-panel.component.css'
})
export class AlertsPanelComponent implements OnInit {
  alerts: Alert[] = [];
  loading = true;
  newCode = 'EUR';
  newThreshold: number | null = null;
  newDirection: AlertDirection = 'Above';

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.loadAlerts();
  }

  loadAlerts(): void {
    this.loading = true;
    this.api.getAlerts().subscribe({
      next: (data) => { this.alerts = data; this.loading = false; },
      error: () => { this.loading = false; }
    });
  }

  addAlert(): void {
    if (!this.newCode || this.newThreshold === null) return;

    const input: AlertInput = {
      code: this.newCode.toUpperCase(),
      threshold: this.newThreshold,
      direction: this.newDirection
    };

    this.api.createAlert(input).subscribe({
      next: () => {
        this.newThreshold = null;   
        this.loadAlerts();         
      }
    });
  }

  removeAlert(id: number): void {
    this.api.deleteAlert(id).subscribe({
      next: () => this.loadAlerts()
    });
  }
}