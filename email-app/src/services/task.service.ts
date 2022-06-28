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
  private _tasks: Task[] = [];

  public get tasks(): Task[] {
    return this._tasks;
  }

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
          this._tasks.push(task);
        })
      )
  }

  public getTasks(userId: number): Observable<Task[]> {
    return this.http
      .get<Task[]>(
        `${this.apiUrl}users/${userId}/tasks`
      )
      .pipe(
        tap((tasks: Task[]) =>
          this._tasks = tasks
        )
      )
  }

  public deleteTask(taskId: number): Observable<string> {
    return this.http
      .delete<string>(
        `${this.apiUrl}users/${this.userService.id}/tasks/${taskId}`
      )
      .pipe(
        tap(() => this._tasks = this._tasks.filter(task => task.id !== taskId))
      );
  }
}
