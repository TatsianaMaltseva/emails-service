import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TaskService } from 'src/services/task.service';

import { UserService } from 'src/services/user.service';
import { LoginComponent } from '../login/login.component';
import { TaskComponent } from '../task/task.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  public get isAdmin(): boolean {
    return this.userService.isAdmin
  }

  public get isLoggedIn(): boolean {
    return this.userService.isLoggedIn
  }

  public constructor(
    private readonly matDialog: MatDialog,
    private readonly userService: UserService,
    private readonly taskService: TaskService
  ) {
  }

  ngOnInit(): void {
    if (!this.isLoggedIn) this.openLoginDialog();
  }

  public openLoginDialog(): void {
    this.matDialog.open(
      LoginComponent,
      {
        width: '400px'
      }
    )
  };

  public logout(): void {
    this.userService.logout();
  }

  public openNewTaskDialog(): void {
    this.taskService.setCurrentlyOpenedTaskId(null);
    this.matDialog.open(
      TaskComponent,
      {
        width: '1500px'
      }
    )
  };
}
