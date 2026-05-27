export interface ExchangeRate {
  id: number;
  code: string;
  currency: string;
  mid: number;
  effectiveDate: string;
  fetchedAt: string;
}

export type AlertDirection = 'Above' | 'Below';

export interface Alert {
  id: number;
  code: string;
  threshold: number;
  direction: AlertDirection;
  isTriggered: boolean;
  lastCheckedRate: number | null;
  triggeredAt: string | null;
  createdAt: string;
}

// Do tworzenia/edycji alertu 
export interface AlertInput {
  code: string;
  threshold: number;
  direction: AlertDirection;
}