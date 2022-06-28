import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { UserService } from './user.service';

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
  public readonly tasks: Task[] = [];

  public constructor(
    private readonly http: HttpClient,
    private readonly userService: UserService
  ) {
      this.apiUrl = environment.api;
  }

  public createTask(task: Task): Observable<Task> {
    const { id, ...taskToAdd } = task;
    return this.http
      .post<Task>(
        `${this.apiUrl}users/${this.userService.id}/tasks`,
         taskToAdd
      )
      .pipe(
        tap((task: Task) => {
          this.tasks.push(task);
        })
      )
  }
}
