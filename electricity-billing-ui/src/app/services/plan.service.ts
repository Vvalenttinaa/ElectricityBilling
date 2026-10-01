import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Plan } from '../models/plan.model';

@Injectable({
  providedIn: 'root',
})
export class PlanService {
  private readonly apiUrl = 'http://localhost:5235/api/Plans';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Plan[]> {
    return this.http.get<Plan[]>(this.apiUrl);
  }

  getById(id: string): Observable<Plan> {
    return this.http.get<Plan>(`${this.apiUrl}/${id}`);
  }

  create(plan: Plan): Observable<Plan> {
    return this.http.post<Plan>(this.apiUrl, plan);
  }

  update(id: string, plan: Plan): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, plan);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
