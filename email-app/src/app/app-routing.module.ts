import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from 'src/auth.guard';
import { TasksComponent } from './tasks/tasks.component';
import { UsersComponent } from './users/users.component';
import { WelcomePageComponent } from './welcome-page/welcome-page.component';

const routes: Routes = [
  { path: '', redirectTo: '/welcome-page', pathMatch: 'full' }, 
  { path: 'welcome-page', component: WelcomePageComponent },
  { path: 'tasks', canActivate: [AuthGuard], component: TasksComponent },
  { path: 'users', canActivate: [AuthGuard], component: UsersComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
