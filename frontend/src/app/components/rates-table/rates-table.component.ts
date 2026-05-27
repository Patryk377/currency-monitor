import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';
import { ExchangeRate } from '../../models';

@Component({
  selector: 'app-rates-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './rates-table.component.html',
  styleUrl: './rates-table.component.css'
})
export class RatesTableComponent implements OnInit {
  rates: ExchangeRate[] = [];
  loading = true;
  error = false;

  constructor(private api: ApiService) {}

  ngOnInit(): void {
    this.api.getRates().subscribe({
      next: (data) => {
        this.rates = data.sort((a, b) => b.mid - a.mid);
        this.loading = false;
      },
      error: () => {
        this.error = true;
        this.loading = false;
      }
    });
  }
}