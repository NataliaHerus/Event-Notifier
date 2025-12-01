import { Component, OnInit } from '@angular/core';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { EventService } from 'src/app/demo/services/app services/event.service';
import {Event} from 'src/app/demo/models/event';
import { SelectedEventsService } from 'src/app/demo/services/app services/selectedEvents.service';
import { SelectedEvent } from 'src/app/demo/models/selectedEvent';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.scss'],
  providers: [MessageService]
})
export class EventDetailsComponent implements OnInit {

  event!: Event;
  selectedEvent!: SelectedEvent;
  isSaved = false;
  errorMessage: string = '';

  constructor(private selectedEventService: SelectedEventsService,private ref: DynamicDialogRef, 
    public config: DynamicDialogConfig,private eventService: EventService, private messageService: MessageService) {}

    ngOnInit(): void {
      this.eventService.getById(this.config.data.id).subscribe(event => {
      this.event = event;
});

    }

    selectEvent()
    {
      if (!this.isSaved) {
        const modal = {} as SelectedEvent;
        modal.eventId = this.event.id;
        this.selectedEventService.create(modal).subscribe(
          (data) => {
            this.messageService.add({
              severity: 'success',
              summary:
                'Подію успішно збережено. Ви отримуватиме нагадування про неї.',
            });
            this.isSaved = true;
          },
          (error) => {
      this.messageService.add({ severity: 'info', summary: 'Ця подія вже збережена вами.'});
      this.isSaved = true;
    });} 
      else {
        this.selectedEventService.get(this.event.id!).subscribe(event => {
        this.selectedEvent = event 
        this.selectedEventService.deleteById(this.selectedEvent.id!).subscribe(
          (data) => {
            this.messageService.add({
              severity: 'success',
              summary:
                'Подію успішно видалено з вибраних' });
            this.isSaved = false;
          },
          (error) => {
            this.messageService.add({
              severity: 'error' });
          }
        );
      })
      }
  }
}
