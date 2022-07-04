import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { ExecutedTasksService } from 'src/services/executed-tasks.service';
import { User, UserService } from 'src/services/user.service';
import { ExecutedTasksComponent } from '../executed-tasks/executed-tasks.component';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  public users: User[] = [];

  public constructor(
    private readonly userService: UserService,
    private readonly executedTaskService: ExecutedTasksService,
    private readonly matDialog: MatDialog
  ) { }

  public ngOnInit(): void {
    this.getUsers();
  }

  public getUsers(): void {
    this.userService
      .getUsers()
        .subscribe(
          (users: User[]) => { this.users = users; }
        );
  }

  public openExecutedTaskDialog(userId: number) {
    this.executedTaskService.setCurrentlyOpenedUserId(userId);
    this.matDialog.open(
      ExecutedTasksComponent,
      {
        width: '1000px'
      }
    )
  }
}
