import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventComponent } from './event/event.component';
import { AllEventsComponent } from './all-events/all-events.component';

const routes: Routes = [];

@NgModule({
  imports: [RouterModule.forChild([   
     { path: 'collection', component: EventComponent },
     { path: 'all', component: AllEventsComponent }
    ])],
  exports: [RouterModule]
})
export class EventRoutingModule { }
