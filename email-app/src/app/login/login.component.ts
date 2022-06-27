import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

import { LoginService } from 'src/services/login.service';
import { LoginDialogComponent } from '../login-dialog/login-dialog.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent{
  public authForm: FormGroup;
  public serverErrorResponse: string = '';
  public hidePassword: boolean = true;

  public get email(): AbstractControl | null {
    return this.authForm.get('email');
  }

  public get password(): AbstractControl | null {
    return this.authForm.get('password');
  }

  public constructor(
    private readonly formBuilder: FormBuilder,
    private readonly loginService: LoginService,
    private readonly marDialogRef: MatDialogRef<LoginDialogComponent>
    ) {
    this.authForm = formBuilder.group({
      email: [
        '', 
        [
          Validators.required,
          Validators.email
        ]
      ],
      password: ['', [Validators.required]]
    });
  }

  public login(email: string, password: string): void {
    this.loginService
      .login(email, password)
      .subscribe(
        () => {
          this.marDialogRef.close();
        },
        (serverError: HttpErrorResponse) => {
          this.serverErrorResponse = serverError.error as string;
        }
      );
  };
}
