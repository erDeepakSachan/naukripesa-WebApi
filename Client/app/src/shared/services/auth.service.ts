import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { NeoContextService } from './neo-context.service';
import AppConstants from './../app-constants';
import { withNoAuth } from '../auth.interceptor';
import { getDecodedAccessToken } from './../utils/auth-utils';
import { Menu, emptyMenu } from './../menu.entity';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl: string;
  private http = inject(HttpClient);
  private router = inject(Router);
  private neoCtx = inject(NeoContextService);

  constructor() {
    this.apiUrl = this.neoCtx.context.AppSettings!.ApiBaseUrl;
  }

  logout(): void {
    localStorage.removeItem(AppConstants.AuthTokenKey);
    this.router.navigate(['/login']);
  }

  getLeftMenu(): Observable<Menu> {
    const url = `${this.apiUrl}/LeftMenu`;
    return this.http.post<{ isSuccess: boolean, data: Menu }>(url, {}, {
    }).pipe(
      map((response) => {
        if (response.isSuccess) {
          return response.data;
        } else {
          return emptyMenu();
        }
      }),
      catchError(() => {
        return of(emptyMenu());
      })
    );
  }

  checkAuth(): Observable<boolean> {
    const authCheckUrl = `${this.apiUrl}/Home/check-auth-status`;
    const token = localStorage.getItem(AppConstants.AuthTokenKey);
    if (!token) {
      this.router.navigate(['/login']);
      return of(false);
    }

    return this.http.post<{ isSuccess: boolean }>(authCheckUrl, {}, {
    }).pipe(
      map((response) => {
        if (response.isSuccess) {
          return true;
        } else {
          this.router.navigate(['/login']);
          return false;
        }
      }),
      catchError(() => {
        this.router.navigate(['/login']);
        return of(false);
      })
    );
  }

  login(userName: string, password: string) {
    this.http.post<{ isSuccess: boolean, data: { token: string }, }>(`${this.apiUrl}/Home/Login`, { userName, password })
      .subscribe({
        next: (response) => {
          if (response.isSuccess) {
            localStorage.setItem(AppConstants.AuthTokenKey, response.data.token);
            this.router.navigate(['/dashboard']);
          }
        },
        error: () => {
        }
      });
  }

  getParsedJwtToken(): any {
    const token = localStorage.getItem(AppConstants.AuthTokenKey)!;
    const decodedToken = getDecodedAccessToken(token);
    return decodedToken;
  }
}