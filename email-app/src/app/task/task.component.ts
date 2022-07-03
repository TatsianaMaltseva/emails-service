import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CronOptions } from 'ngx-cron-editor';

import { TaskService, TaskDialogState, Task } from 'src/services/task.service';
import { TaskDialogComponent } from '../task-dialog/task-dialog.component';

export enum Topic {
  Weather = "Weather",
  Sport = "Sport",
  Stops = "Stops"
}

export enum WeatherOptions {
  London = "London",
  Minsk = "Minsk",
  Prague = "Prague"
}

export enum SportOptions {
  PremierLeague = "PremierLeague",
  EFL = "EFL"
}

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {
  public changeTaskForm: FormGroup;
  public readonly topicOptions = [Topic.Weather, Topic.Sport, Topic.Stops];
  public readonly sportOptions = [SportOptions.PremierLeague, SportOptions.EFL];
  public readonly weatherOptions = [WeatherOptions.London, WeatherOptions.Minsk, WeatherOptions.Prague];

  public serverErrorResponse: string = '';
  public isTaskBeingEdited: boolean = false;
  public today = new Date();
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

 public get options(): [] {
  const currentTopic = this.changeTaskForm.value.topic;
  if (currentTopic === Topic.Weather) {
    return this.weatherOptions as []
  }
  if (currentTopic === Topic.Sport) {
     return this.sportOptions as []
  }
   return [];
 }
 
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
        cron: ['* * * * *'],
        topic: [null, Validators.required],
        startDate: [null, Validators.required],
        lastExecuted: [null],
        option: [null]
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
