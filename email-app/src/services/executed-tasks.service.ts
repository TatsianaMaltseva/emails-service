import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';

export interface ExecutedTask {
  name: string;
  executed: string;
  startDate: string;
  lastExecuted: string;
}

@Injectable({
  providedIn: 'root'
})
export class ExecutedTasksService {
  private readonly apiUrl: string;
  private _currentlyOpenedUserId: number | null = null;

  public get getCurrentlyOpenedUserId(): number | null {
    return this._currentlyOpenedUserId;
  }

  public constructor(
    private readonly http: HttpClient,
  ) {
    this.apiUrl = environment.api;
  }

  public setCurrentlyOpenedUserId(userId: number): void {
    this._currentlyOpenedUserId = userId;
  }

  public getExecutedTasks(): Observable<ExecutedTask[]> {
    return this.http
      .get<ExecutedTask[]>(
        `${this.apiUrl}users/${this._currentlyOpenedUserId}/executedtasks`
      )
  }
}
