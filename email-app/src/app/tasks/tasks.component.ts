import { Component, OnInit } from '@angular/core';
import { TaskService, Task } from 'src/services/task.service';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent implements OnInit {
  public get tasks(): Task[]  {
    return this.taskService.tasks;
  }

  public constructor(
    private readonly taskService: TaskService,
    private readonly userService: UserService
  ) {
  }

  public ngOnInit(): void {
    this.getTasks();
  }

  public deleteTask(taskId: number): void {
    this.taskService
      .deleteTask(taskId)
      .subscribe();
  }

  public openEditTaskDialog(task: Task): void {

  }

  private getTasks(): void {
    const userId = this.userService.id;
    if (userId) {
      this.taskService
        .getTasks(userId)
        .subscribe();
    }
  }
}