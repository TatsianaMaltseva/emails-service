import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CronOptions } from 'ngx-cron-editor';

import { TaskDialogComponent } from '../task-dialog/task-dialog.component';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {
  public changeTaskForm: FormGroup;

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
    private readonly formBuilder: FormBuilder
  ) {
    this.changeTaskForm = formBuilder.group(
      {
        id: [''],
        name: ['', Validators.required],
        description: [''],
        cron: ['0 0 1/1 * *']
      }
    )
  }

  public ngOnInit(): void {
    
  }

  public createTask(): void {
    console.log(this.changeTaskForm.value);
  }

  private closeDialog(): void {
    this.marDialogRef.close();
  }
}
