import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { RecommendationRequest } from '../models/recommendation-request.model';
import { RecommendationResponse } from '../models/recommendation-response.model';

@Injectable({
  providedIn: 'root',
})
export class RecommendationService {
  private readonly apiUrl = 'http://localhost:5235/api/Recommendations';

  constructor(private http: HttpClient) {}

  getRecommendation(
    request: RecommendationRequest,
  ): Observable<RecommendationResponse> {
    return this.http.post<RecommendationResponse>(this.apiUrl, request);
  }
}
