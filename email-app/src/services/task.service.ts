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

export enum TaskDialogState {
  Edit,
  Create
}

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private readonly apiUrl: string;
  private _tasks: Task[] = [];
  private _currentlyOpenedTaskId: number | null = null;

  public get tasks(): Task[] {
    return this._tasks;
  }

  public get taskDialogState(): TaskDialogState {
    if (this._currentlyOpenedTaskId) {
      return TaskDialogState.Edit;
    }
    return TaskDialogState.Create;
  }

  public get currentlyOpenedTask(): Task | null {
    return this._tasks
      .filter(task => task.id === this._currentlyOpenedTaskId)[0] ?? null;
  }

  public constructor(
    private readonly http: HttpClient,
    private readonly userService: UserService
  ) {
      this.apiUrl = environment.api;
  }

  public setCurrentlyOpenedTaskId(taskId: number | null): void {
    this._currentlyOpenedTaskId = taskId;
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

  public editTask(task: Task): Observable<Task> {
    const { id, ...taskToEdit } = task;
    return this.http
      .put<Task>(
        `${this.apiUrl}users/${this.userService.id}/tasks/${id}`,
        taskToEdit
      )
      .pipe(
        tap(() => {
          const idInTaskArray = this._tasks.findIndex(task => id === task.id);
          this._tasks[idInTaskArray] = task;
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
