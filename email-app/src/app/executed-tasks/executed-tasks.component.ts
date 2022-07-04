import { Component, OnInit } from '@angular/core';

import { ExecutedTask, ExecutedTasksService } from 'src/services/executed-tasks.service';

@Component({
  selector: 'app-executed-tasks',
  templateUrl: './executed-tasks.component.html',
  styleUrls: ['./executed-tasks.component.css']
})
export class ExecutedTasksComponent implements OnInit {
  public executedTasks: ExecutedTask[] = [];
  public readonly tableColumns: string[] = [
    'name', 
    'executed', 
    'startDate', 
    'lastExecuted'
  ];

  public constructor(
    private readonly executedTasksService: ExecutedTasksService,
  ) {
  }

  public ngOnInit(): void {
    this.executedTasksService
      .getExecutedTasks()
      .subscribe(
        ((executedTasks: ExecutedTask[]) => {
          this.executedTasks = executedTasks;
        })
      )
  }
}
