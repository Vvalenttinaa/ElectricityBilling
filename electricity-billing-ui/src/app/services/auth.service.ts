import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  username: string;
  role: string;
  expiresAt: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly apiUrl = 'http://localhost:5235/api/Auth';

  private readonly _username = signal<string | null>(
    localStorage.getItem('username'),
  );

  private readonly _role = signal<string | null>(
    localStorage.getItem('role'),
  );

  private readonly _token = signal<string | null>(
    localStorage.getItem('token'),
  );

  readonly username = this._username.asReadonly();

  readonly role = this._role.asReadonly();

  readonly isLoggedIn = computed(() => !!this._token() && !this.isTokenExpired());

  readonly isAdmin = computed(()=> this.isLoggedIn() && this._role() === 'Admin');

  constructor(private http: HttpClient) {}

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/login`, request)
      .pipe(
        tap((response) => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('username', response.username);
          localStorage.setItem('role', response.role);
          localStorage.setItem('expiresAt', response.expiresAt);

          this._token.set(response.token);
          this._username.set(response.username);
          this._role.set(response.role);
        }),
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('role');
    localStorage.removeItem('expiresAt');

    this._token.set(null);
    this._username.set(null);
    this._role.set(null);
  }

  isTokenExpired(): boolean {
  const expiresAt = localStorage.getItem('expiresAt');

  if (!expiresAt) {
    return true;
  }

  return new Date(expiresAt).getTime() <= Date.now();
}

  getToken(): string | null {
    return this._token();
  }
}