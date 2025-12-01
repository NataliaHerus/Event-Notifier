import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EventRoutingModule } from './event-routing.module';
import { EventComponent } from './event/event.component';
import { DataViewModule } from 'primeng/dataview';
import { PickListModule } from 'primeng/picklist';
import { OrderListModule } from 'primeng/orderlist';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { RatingModule } from 'primeng/rating';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from "primeng/calendar";
import { FormsModule } from '@angular/forms';
import { FieldsetModule } from 'primeng/fieldset';
import { EventDetailsComponent } from './event-details/event-details.component';
import { ImageModule } from 'primeng/image';
import { DividerModule } from 'primeng/divider';
import { PanelModule } from 'primeng/panel';
import { ToastModule } from 'primeng/toast';
import {TableModule} from 'primeng/table';
import { AllEventsComponent } from './all-events/all-events.component';
import { FileUploadModule } from 'primeng/fileupload';


@NgModule({
  declarations: [
    EventComponent,
    EventDetailsComponent,
    AllEventsComponent
  ],
  imports: [
    CommonModule,
    EventRoutingModule,
    DataViewModule,
		PickListModule,
		OrderListModule,
		InputTextModule,
		DropdownModule,
		RatingModule,
		ButtonModule,
    FormsModule,
    CalendarModule,
    FieldsetModule,
    ImageModule,
    DividerModule,
    PanelModule,
    ToastModule,
    TableModule,
    FileUploadModule
  ]
})
export class EventModule { }
