import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AnalyticsSummary } from '../models/analytics.model';

@Injectable({
  providedIn: 'root',
})
export class AnalyticsService {
  private readonly apiUrl = 'http://localhost:5235/api/analytics';

  constructor(private http: HttpClient) {}

  getSummary(): Observable<AnalyticsSummary> {
    return this.http.get<AnalyticsSummary>(`${this.apiUrl}/summary`);
  }
}
