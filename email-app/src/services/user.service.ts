import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

import { environment } from 'src/environments/environment';
import { roles } from 'src/core/Roles';
import { tokenGetter } from 'src/app/app.module';

export interface LoginResponse {
 token: string;
}

export interface Token {
  id: number;
  role: string;
  exp: number;
  iss: string;
  aud: string;
}

export interface User {
  id: number,
  email: string,
  role: string
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly apiUrl: string;

  public get id(): number | null {
    if (this.isLoggedIn) {
      return this.decodedToken.id;
    }
    return null;
  }

  public get isLoggedIn(): boolean {
    const token: string | null = tokenGetter();
    return token !== null && !this.jwtHelper.isTokenExpired(token);
  }

  public get isAdmin(): boolean {
    return this.isLoggedIn && this.role === roles.admin
  }

  private get decodedToken(): Token {
    return this.jwtHelper.decodeToken(tokenGetter()!);
  }

  private get role(): string | null {
    if (this.isLoggedIn) {
      return this.decodedToken.role;
    }
    return null;
  }

  public constructor(
    private readonly http: HttpClient,
    private readonly jwtHelper: JwtHelperService
  ) {
    this.apiUrl = environment.api;
  }

  public login(email: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.apiUrl}login`,
        { email, password }
      )
      .pipe(
        tap((response: LoginResponse) => {
          localStorage.setItem(environment.jwt, response.token);
        })
      );
  }

  public logout(): void {
    localStorage.removeItem(environment.jwt);
  }

  public getUsers(): Observable<User[]> {
    let httpParams = new HttpParams();
    if (this.id) {
      httpParams = httpParams.set('currentUserId', this.id);
    }
    return this.http
      .get<User[]>(
        `${this.apiUrl}users`,
        { params : httpParams }
      )
  }
}
