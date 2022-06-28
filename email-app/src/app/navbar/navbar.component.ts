import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from 'src/services/user.service';
import { LoginComponent } from '../login/login.component';

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
    return this.userService.isLoggedIn
  }

  constructor(
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
}
