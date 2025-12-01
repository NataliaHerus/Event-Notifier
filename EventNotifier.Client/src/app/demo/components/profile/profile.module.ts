import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileRoutingModule } from './profile-routing.module';
import { UserInfoComponent } from './user-info/user-info.component';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { EventRoutingModule } from '../event/event-routing.module';
import { DataViewModule } from 'primeng/dataview';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { ImageModule } from 'primeng/image';
import { PanelModule } from 'primeng/panel';
import { DividerModule } from 'primeng/divider';
import { CalendarComponent } from './calendar/calendar.component';
import { CalendarModule } from 'primeng/calendar';
import { ToastModule } from 'primeng/toast';


@NgModule({
  declarations: [
  
    UserInfoComponent,
       CalendarComponent
  ],
  imports: [
    CommonModule,
    ProfileRoutingModule,
    FormsModule,
		InputTextModule,
    EventRoutingModule,
    DataViewModule,
		InputTextModule,
		DropdownModule,
		ButtonModule,
    FormsModule,
    ImageModule,
    PanelModule,
    DividerModule,
    CalendarModule,
    ToastModule 
  ]
})
export class ProfileModule { }
