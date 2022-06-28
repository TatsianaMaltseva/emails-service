import { Component, OnInit } from '@angular/core';
import { TaskService } from 'src/services/task.service';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-tasks',
  templateUrl: './tasks.component.html',
  styleUrls: ['./tasks.component.css']
})
export class TasksComponent implements OnInit {
  public constructor(
    private readonly taskService: TaskService,
    private readonly userService: UserService
  ) {
  }

  public ngOnInit(): void {
    const userId = this.userService.id;
    if (userId) {
      this.taskService
        .getTasks(userId)
        .subscribe(tasks => {console.log(tasks)});
    }
  }
}
