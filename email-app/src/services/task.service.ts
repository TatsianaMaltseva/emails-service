import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

export interface Task {
  id: number;
  name: string;
  description: string;
  cron: string;
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly apiUrl: string;

  public constructor(
    private readonly http: HttpClient) {
      this.apiUrl = environment.api;
  }

  public createTask(task: Task): Observable<Task> {
    return this.http
      .post<Task>(
        `${this.apiUrl}users`,
        task
      )
  }
}
