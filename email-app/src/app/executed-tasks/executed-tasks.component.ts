import { Component, OnInit } from '@angular/core';
import * as dayjs from 'dayjs';

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
  private readonly dateTimeFormat = 'MMMM D, YYYY h:mm A';

  public constructor(
    private readonly executedTasksService: ExecutedTasksService,
  ) {
  }

  public ngOnInit(): void {
    this.executedTasksService
      .getExecutedTasks()
      .subscribe(
        ((executedTasks: ExecutedTask[]) => {
          executedTasks.forEach(executedTask => {
            executedTask.executed = this.formatDate(executedTask.executed);
            executedTask.lastExecuted = this.formatDate(executedTask.lastExecuted);
            executedTask.startDate = this.formatDate(executedTask.startDate);
          });
          this.executedTasks = executedTasks;
        })
      )
  }

  private formatDate = (date: string): string => 
    dayjs(new Date(date)).format(this.dateTimeFormat);
}
