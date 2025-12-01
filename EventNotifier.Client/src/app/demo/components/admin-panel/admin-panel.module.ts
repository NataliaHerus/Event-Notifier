import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToolbarModule } from 'primeng/toolbar';
import { AdminPanelRoutingModule } from './admin-panel-routing.module';
import { UsersComponent } from './users/users.component';
import { EventsManagerComponent } from './events-manager/events-manager.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { FormsModule } from '@angular/forms';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { DialogModule } from 'primeng/dialog';
import { InputNumberModule } from 'primeng/inputnumber';
import { RadioButtonModule } from 'primeng/radiobutton';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { RatingModule } from 'primeng/rating';
import { FileUploadModule } from 'primeng/fileupload';


@NgModule({
  declarations: [
    UsersComponent,
    EventsManagerComponent
  ],
  imports: [
    CommonModule,
    AdminPanelRoutingModule,
    TableModule,
    FormsModule,
    ButtonModule,
    RippleModule,
    ToastModule,
    RatingModule,
    InputTextModule,
    DropdownModule,
    RadioButtonModule,
    InputNumberModule,
    DialogModule, 
    ToolbarModule,
    FileUploadModule
  ]
})
export class AdminPanelModule { }
