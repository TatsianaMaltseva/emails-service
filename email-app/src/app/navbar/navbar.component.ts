import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { UserService } from 'src/services/user.service';
import { LoginComponent } from '../login/login.component';
import { TaskComponent } from '../task/task.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  public get isAdmin(): boolean {
    return this.userService.isAdmin
  }

  public get isLoggedIn(): boolean {
    return true;// this.userService.isLoggedIn
  }

  public constructor(
    private readonly matDialog: MatDialog,
    private readonly userService: UserService
  ) {
  }

  public openLoginDialog(): void {
    this.matDialog.open(
      LoginComponent,
      {
        width: '400px'
      }
    )
  };

  public openNewTaskDialog(): void {
    this.matDialog.open(
      TaskComponent,
      {
        width: '600px'
      }
    )
  };
}
