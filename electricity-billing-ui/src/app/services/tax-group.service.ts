import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { TaxGroup } from '../models/tax-group.model';

@Injectable({
  providedIn: 'root',
})
export class TaxGroupService {
  private readonly apiUrl = 'http://localhost:5235/api/TaxGroups';

  constructor(private http: HttpClient) {}

  getAll(): Observable<TaxGroup[]> {
    return this.http.get<TaxGroup[]>(this.apiUrl);
  }

  getById(id: string): Observable<TaxGroup> {
    return this.http.get<TaxGroup>(`${this.apiUrl}/${id}`);
  }

  create(taxGroup: TaxGroup): Observable<TaxGroup> {
    return this.http.post<TaxGroup>(this.apiUrl, taxGroup);
  }

  update(id: string, taxGroup: TaxGroup): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, taxGroup);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
