import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LayoutService } from './service/app.layout.service';
import { AuthService } from '../demo/services/identity services/auth.service';
import { map } from 'rxjs';

@Component({
    selector: 'app-menu',
    templateUrl: './app.menu.component.html'
})
export class AppMenuComponent implements OnInit {

    model: any[] = [];
    role: string='';

    constructor(public layoutService: LayoutService, public authService: AuthService) { }

    ngOnInit() {
        this.getRole().subscribe(res =>{
            if (!res)
            {
                 this.role='';
            }
            else
            {
                this.role=res.role;
            }
        })
        this.model = [
            {
                label: 'Домашня сторінка',
                role: [''],
                items: [
                    {
                        label: 'Розпочати',
                        icon: 'pi pi-fw pi-user',
                        items: [
                            {
                                label: 'Увійти',
                                icon: 'pi pi-fw pi-sign-in',
                                routerLink: ['/auth/login']
                            },
                        ]
                    },
                ]
            },
            {
                label: 'Події',
                role: ['Admin', 'User'],
                icon: 'pi pi-fw pi-briefcase',
                items: [
                    {
                        label: 'Усі події',
                        icon: 'pi pi-fw pi-calendar',
                        routerLink: ['/event/all']
                    },
                    {
                        label: 'Найближчі події',
                        icon: 'pi pi-fw pi-calendar',
                        routerLink: ['/event/collection']
                    },
                ]
            },
            {
                label: 'Панель Адміністратора',
                role: ['Admin'],
                icon: 'pi pi-fw pi-briefcase',
                items: [
                    {
                        label: 'Керувати подіями',
                        icon: 'pi pi-fw pi-calendar',
                        routerLink: ['/admin/events']
                    },
                    {
                        label: 'Усі користувачі',
                        icon: 'pi pi-fw pi-calendar',
                        routerLink: ['/admin/users']
                    },
                ]
            }
        ];
    }

    getRole()
    {
       return  this.authService.getCurrentUserRole();
           
    }
}