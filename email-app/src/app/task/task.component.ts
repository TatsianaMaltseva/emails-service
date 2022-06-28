import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CronOptions } from 'ngx-cron-editor';

import { TaskDialogComponent } from '../task-dialog/task-dialog.component';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {
  cronForm: FormControl<string | null>;

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
    hideSeconds: true,

    cronFlavor: "Standart"
 };
 
  public constructor(
    private readonly marDialogRef: MatDialogRef<TaskDialogComponent>
  ) {
    this.cronForm = new FormControl('0 0 1/1 * *');
  }

  public ngOnInit(): void {
    
  }

  public createTask(): void {
    console.log(this.cronForm.value);
  }

  private closeDialog(): void {
    this.marDialogRef.close();
  }
}
