import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ExchangeRate, Alert, AlertInput } from '../models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  // Adres bramy YARP
  private baseUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) {}

  // Kursy 
  getRates(): Observable<ExchangeRate[]> {
    return this.http.get<ExchangeRate[]>(`${this.baseUrl}/rates`);
  }

  getRateHistory(code: string, days = 30): Observable<ExchangeRate[]> {
    return this.http.get<ExchangeRate[]>(`${this.baseUrl}/rates/${code}/history?days=${days}`);
  }

  // Alerty
  getAlerts(): Observable<Alert[]> {
    return this.http.get<Alert[]>(`${this.baseUrl}/alerts`);
  }

  createAlert(alert: AlertInput): Observable<Alert> {
    return this.http.post<Alert>(`${this.baseUrl}/alerts`, alert);
  }

  updateAlert(id: number, alert: AlertInput): Observable<Alert> {
    return this.http.put<Alert>(`${this.baseUrl}/alerts/${id}`, alert);
  }

  deleteAlert(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/alerts/${id}`);
  }
}