import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TaskService, Task } from 'src/services/task.service';
import { UserService } from 'src/services/user.service';
import { TaskComponent } from '../task/task.component';

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
    private readonly userService: UserService,
    private readonly matDialog: MatDialog
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
    this.taskService.setCurrentlyOpenedTaskId(task.id);
    this.matDialog.open(
      TaskComponent,
      {
        width: '1500px'
      }
    )
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
