import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users/users.component';
import { EventsManagerComponent } from './events-manager/events-manager.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forChild([   
    { path: 'events', component:  EventsManagerComponent},
    { path: 'users', component: UsersComponent }
   ])],
  exports: [RouterModule]
})
export class AdminPanelRoutingModule { }
