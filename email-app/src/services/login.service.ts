import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { environment } from 'src/environments/environment';

export interface LoginResponse {
  role: string;
  id: number;
}

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private readonly apiUrl: string;
  private _id: number | undefined;
  private _role: string | undefined;

  public get role(): string | null {
    if (this._role) {
      return this._role;
    }
    return null;
  }

  public get id(): number | null {
    if (this._id) {
      return this._id;
    }
    return null;
  }

  public get isLoggedIn(): boolean {
    return !!this._id;
  }

  public constructor(
    private readonly http: HttpClient,
  ) {
    this.apiUrl = environment.api;
  }

  public login(email: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.apiUrl}users`,
        { email, password }
      )
      .pipe(
        tap((userData: LoginResponse) => {
          this._id = userData.id;
          this._role = userData.role;
        })
      )
  }
}
