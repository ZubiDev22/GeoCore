import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient) {}

  login(credentials: { email: string; password: string }): Observable<any> {
  return this.http.post(`${environment.apiUrl}/auth/login`, credentials);
  }

  logout(): void {
    // Lógica para cerrar sesión (eliminar token, etc.)
    localStorage.removeItem('token');
  }
}
