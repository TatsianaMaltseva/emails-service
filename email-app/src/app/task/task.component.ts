import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CronOptions } from 'ngx-cron-editor';

import { TaskService, TaskDialogState, Task } from 'src/services/task.service';
import { TaskDialogComponent } from '../task-dialog/task-dialog.component';

export enum Topic {
  Weather = "Weather",
  Languages = "Languages",
  Stops = "Stops"
}

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {
  public changeTaskForm: FormGroup;
  public readonly topicOptions = [Topic.Weather, Topic.Languages, Topic.Stops];
  public serverErrorResponse: string = '';
  public isTaskBeingEdited: boolean = false;
  public minDate = new Date();
  public cronOptions: CronOptions = {
    defaultTime: "00:00:00",

    hideMinutesTab: false,
    hideHourlyTab: false,
    hideDailyTab: false,
    hideWeeklyTab: false,
    hideMonthlyTab: false,
    hideYearlyTab: false,
    hideAdvancedTab: true,
    hideSpecificWeekDayTab: false,
    hideSpecificMonthWeekTab : false,

    use24HourTime: true,
    hideSeconds: false,

    cronFlavor: "standart"
 };
 
  public constructor(
    private readonly marDialogRef: MatDialogRef<TaskDialogComponent>,
    private readonly formBuilder: FormBuilder,
    private readonly taskService: TaskService
  ) {
    this.changeTaskForm = formBuilder.group(
      {
        id: [''],
        name: ['', Validators.required],
        description: [''],
        cron: ['0 0 1/1 * *'],
        topic: [null, Validators.required],
        startDate: [this.minDate, Validators.required]
      }
    )
  }

  public ngOnInit(): void {
    if (this.taskService.taskDialogState === TaskDialogState.Edit) {
      this.isTaskBeingEdited = true;
      this.changeTaskForm.patchValue(this.taskService.currentlyOpenedTask!);
    }
  }

  public createTask(): void {
    this.taskService
      .createTask(this.changeTaskForm.value)
      .subscribe(
        () => this.closeDialog(),
        (serverError: HttpErrorResponse) => {
          this.serverErrorResponse = serverError.error as string;
        });
  }

  public editTask(): void {
    this.taskService
      .editTask(this.changeTaskForm.value)
      .subscribe(
        () => this.closeDialog(),
        (serverError: HttpErrorResponse) => {
          this.serverErrorResponse = serverError.error as string
        }
      )
  }

  private closeDialog(): void {
    this.marDialogRef.close();
  }
}
