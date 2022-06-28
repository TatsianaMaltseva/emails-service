import { Component, OnInit } from '@angular/core';
import { User, UserService } from 'src/services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  public users: User[] = [];

  public constructor(
    private readonly userService: UserService
  ) { }

  public ngOnInit(): void {
    this.getUsers();
  }

  public getUsers(): void {
    this.userService
      .getUsers()
        .subscribe(
          (users: User[]) => {
            this.users = users;
          }
        );
  }
}
