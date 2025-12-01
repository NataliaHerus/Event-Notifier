import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserInfoComponent } from './user-info/user-info.component';
import { CalendarComponent } from './calendar/calendar.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forChild([
    { path: 'user', component: UserInfoComponent },
    { path: 'calendar', component: CalendarComponent },
  ]
  )],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }
