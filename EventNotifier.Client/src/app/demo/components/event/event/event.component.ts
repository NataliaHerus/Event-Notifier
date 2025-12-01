import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { EventService } from 'src/app/demo/services/app services/event.service';
import { SelectItem } from 'primeng/api';
import { DataView } from 'primeng/dataview';
import { Dialog } from 'primeng/dialog';
import { DialogService } from 'primeng/dynamicdialog';
import { EventDetailsComponent } from '../event-details/event-details.component';

@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss'],
  providers: [DialogService]
})
export class EventComponent implements OnInit {

  events: Event[] = []; 
  selectedEvent!: Event;
  eventDialog!: Dialog;
  sortOptions: SelectItem[] = [];
  sortOrder: number = 0;
  sortField: string = '';
  @ViewChild('filter') filter!: ElementRef;

  constructor(  private eventService: EventService, private dialogService: DialogService) { }

  ngOnInit(): void {
    this.eventService.getClosest().subscribe(events => {
      this.events = events;
  });

this.sortOptions = [
    { label: 'Незабаром', value: 'startDate' },
    { label: 'Пізніші', value: '!startDate' }
];
  }
  onSortChange(event: any) {
      const value = event.value;

      if (value.indexOf('!') === 0) {
          this.sortOrder = -1;
          this.sortField = value.substring(1, value.length);
      } else {
          this.sortOrder = 1;
          this.sortField = value;
      }
  }

  onFilter(dv: DataView, event: Event) {
      dv.filter((event.target as HTMLInputElement).value);
  }

  showEventDetails(id: number) { 
    this.dialogService.open(EventDetailsComponent, {
      header: 'Деталі про подію',
      data: {id: id },
    });
  }

}
