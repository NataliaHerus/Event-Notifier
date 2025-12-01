import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { EventService } from 'src/app/demo/services/app services/event.service';
import { Event } from 'src/app/demo/models/event';
import { Format } from 'src/app/demo/models/format';
import { Category } from 'src/app/demo/models/category';

@Component({
    selector: 'app-events-manager',
    templateUrl: './events-manager.component.html',
    styleUrls: ['./events-manager.component.scss'],
    providers: [MessageService]
})

export class EventsManagerComponent implements OnInit {

    events: Event[] = [];

    eventDialog: boolean = false;
    deleteEventDialog: boolean = false;
    deleteEventsDialog: boolean = false;
    createEventDialog: boolean = false;

    event: Event = {};
    selectedEvents: Event[] = [];

    submitted: boolean = false;
    cols: any[] = [];

    categories: any[] = [];
    formats: any[] = [];
    format: string = '';
    category: string = '';

    uploadedFile: File | undefined;
    rowsPerPageOptions = [5, 10, 20];

    constructor(private eventService: EventService, private messageService: MessageService) { }

    ngOnInit() {
        this.eventService.getAll().subscribe(events => {
            this.events = events;
        });

        this.categories = [
            { label: 'ХМАРНІ ТЕХНОЛОГІЇ', value: 'хмарні технології' },
            { label: 'Інфраструктура', value: 'інфраструктура' },
            { label: 'Розробка', value: 'розробка' },
            { label: 'ШТУЧНИЙ ІНТЕЛЕКТ', value: 'штучний інтелект' },
            { label: 'Інновації', value: 'інновації' }
        ];

        this.formats = [
            { label: 'Онлайн', value: 'офлайн' },
            { label: 'Офлайн', value: 'онлайн' },
            { label: 'Гібридний', value: 'гібридний' }
        ];

    }

    onFileSelected(event: any) {
        this.uploadedFile = event.target.files[0];
    }

    openNew() {
        this.createEventDialog = true
        this.event = {};
        this.submitted = false;
        this.eventDialog = true;
    }

    deleteSelectedProducts() {
        this.deleteEventsDialog = true;
    }

    editProduct(event: Event) {
        this.createEventDialog = false
        this.event = { ...event };
        if (this.format != '') {
            this.event.format!.name = this.format;
        }
        if (this.category != '') {
            this.event.category!.name = this.category;
        }
        this.eventDialog = true;
    }

    deleteProduct(event: Event) {
        this.deleteEventDialog = true;
        this.event = { ...event };
    }

    confirmDeleteSelected() {
        this.deleteEventsDialog = false;
        this.events = this.events.filter(val => !this.selectedEvents.includes(val));
        for (let i = 0; i < this.selectedEvents.length; i++) {
            this.eventService.deleteById(this.selectedEvents[i].id!).subscribe(data => {
                this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Події видалено' })
            });
        }
        this.selectedEvents = [];
    }

    confirmDelete() {
        this.deleteEventDialog = false;
        this.events = this.events.filter(val => val.id !== this.event.id);
        this.eventService.deleteById(this.event.id!).subscribe(data => {
            this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Подію видалено' })
        });
        this.event = {};
    }

    hideDialog() {
        this.eventDialog = false;
        this.submitted = false;
        this.event = {};
        this.category = '';
        this.format = '';
    }

    saveProduct(event: Event) {
        this.submitted = true;

        if (this.event.name?.trim()) {
            if (this.event.id) {
                this.eventService.update(this.event).subscribe(data => {
                    this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Подію оновлено', life: 3000 });
                });
                this.events[this.findIndexById(this.event.id!)] = this.event;
            }
            else {
                let fr: Format = { name: this.format }
                let ct: Category = { name: this.category }
                this.event.name = event.name;
                this.event.description = event.description;
                this.event.startDate = event.startDate;
                this.event.endDate = event.endDate;
                this.event.format = fr;
                this.event.category = ct;

                this.eventService.create(this.event).subscribe(data => {
                    if (this.uploadedFile) {
                        const chunk = this.uploadedFile.slice(0, this.uploadedFile.size);
                        this.eventService.uploadFile(chunk, data.id).subscribe(_ => {
                            this.eventService.getAll().subscribe(events => {
                                this.events = events;
                                this.createEventDialog = false;
                                this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Подію створено', life: 3000 });
                            });
                        });
                    }
                });
            }

            this.events = [...this.events];
            this.eventDialog = false;
            this.event = {};
            this.category = '';
            this.format = '';
        }
    }

    onformatChange(event: Event) {
        this.format = this.formats.find(format => format.label === event.format!.name);
    }

    onCategoryChange(event: Event) {
        this.category = this.categories.find(category => category.label === event.category!.name);
    }

    findIndexById(id: number): number {
        let index = -1;
        for (let i = 0; i < this.events.length; i++) {
            if (this.events[i].id === id) {
                index = i;
                break;
            }
        }

        return index;
    }

    onGlobalFilter(table: Table, event: any) {
        table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
    }
}
