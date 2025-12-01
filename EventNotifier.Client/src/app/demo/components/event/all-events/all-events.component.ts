import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Table } from 'primeng/table';
import { MessageService, ConfirmationService } from 'primeng/api';
import { EventService } from 'src/app/demo/services/app services/event.service';
import {Event} from 'src/app/demo/models/event';
import { DialogService } from 'primeng/dynamicdialog';
import { EventDetailsComponent } from '../event-details/event-details.component';


@Component({
    templateUrl: './all-events.component.html',
    styleUrls: ['./all-events.component.scss'],
    providers: [MessageService, ConfirmationService, DialogService]
})
export class AllEventsComponent implements OnInit {

    events: Event[] = [];

    categories: any[] = [];

    formats: any[] = [];

    rowGroupMetadata: any;


    activityValues: number[] = [0, 100];

    loading: boolean = true;

    @ViewChild('filter') filter!: ElementRef;

    constructor(private eventService: EventService, private dialogService: DialogService) { }

    ngOnInit() {
        this.eventService.getAll().subscribe(events => {
            this.events = events;
            this.loading = false;
        });

        this.categories = [
          { label: 'Unqualified', value: 'unqualified' },
          { label: 'Qualified', value: 'qualified' },
          { label: 'New', value: 'new' },
          { label: 'Negotiation', value: 'negotiation' },
          { label: 'Renewal', value: 'renewal' },
          { label: 'Proposal', value: 'proposal' }
      ];
  }

    onGlobalFilter(table: Table, event: any) {
        table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }

    clear(table: Table) {
        table.clear();
        this.filter.nativeElement.value = '';
    }

    showEventDetails(id: number) { 
        this.dialogService.open(EventDetailsComponent, {
          header: 'Деталі про подію',
          data: {id: id },
        });
    
        console.log(id);
      }
    
}