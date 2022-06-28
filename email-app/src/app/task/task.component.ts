import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { TaskDialogComponent } from '../task-dialog/task-dialog.component';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {

  public constructor(
    private readonly marDialogRef: MatDialogRef<TaskDialogComponent>
  ) {
  }

  public ngOnInit(): void {
  }

  private closeDialog(): void {
    this.marDialogRef.close();
  }
}
