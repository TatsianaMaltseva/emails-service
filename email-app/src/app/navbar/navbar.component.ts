import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { LoginComponent } from '../login/login.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  public get isAdmin(): boolean {
    return true;
  }

  constructor(
    private readonly matDialog: MatDialog
  ) { }

  public openLoginDialog(): void {
    this.matDialog.open(
      LoginComponent,
      {
        width: '400px'
      }
    )
  };
}
