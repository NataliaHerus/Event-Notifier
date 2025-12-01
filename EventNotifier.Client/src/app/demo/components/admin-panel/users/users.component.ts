import { Component, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table } from 'primeng/table';
import { User } from 'src/app/demo/models/user';
import { AuthService } from 'src/app/demo/services/identity services/auth.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
  providers: [MessageService]
})
export class UsersComponent implements OnInit {

  users: User[] = [];
  loading: boolean = true;
  userDialog: boolean = false;
  deleteUserDialog: boolean = false;
  createUserDialog:  boolean = false;
  editUserDialog:  boolean = false;
  user: User = {
    id: '',
    firstName: '',
    lastName: '',
    email: '',
    Password: ''
};

  constructor(private authService: AuthService, private messageService: MessageService) { }

  ngOnInit() {
    this.authService.getAll().subscribe(users => {
        this.users = users;
        this.loading = false;
    });
  }

  onGlobalFilter(table: Table, event: any) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }

  addNewUser()
  {
    this.userDialog = true;
    this.createUserDialog = true;
  }

  hideDialog() {
    this.userDialog = false;
    this.user = {
      id: '',
      firstName: '',
      lastName: '',
      email: '',
      Password: ''
  };
}

saveUser(user: User) {
  console.log(this.user)
  if (this.user.email) {
    if (this.user.id) {
        this.authService.editUser(this.user).subscribe(data => {
            this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Користувача оновлено', life: 3000 });
        });

        this.users[this.findIndexById(this.user.id!)] = this.user;
        this.editUserDialog = false;
    } 
    else {
        this.user.firstName = user.firstName;
        this.user.lastName = user.lastName;
        this.user.email = user.email;
        this.authService.createUser(this.user).subscribe(data => {
            this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Користувача створено', life: 3000 });
        });

        this.createUserDialog = false;
        this.users.push(this.user);
    }
    this.users = [...this.users];
    this.userDialog = false;
    this.user = {
      id: '',
      firstName: '',
      lastName: '',
      email: '',
      Password: ''
    };  
  }
}
  editUser(user: User) {
    this.userDialog = true;
    this.editUserDialog = true
    this.user = { ...user }
  }

  deleteUser(user: User) {
    this.deleteUserDialog = true;
    this.user = { ...user }
  }

  confirmDelete() {
    this.deleteUserDialog = false;
    this.users = this.users.filter(val => val.id !== this.user.id);
    this.authService.deleteUser(this.user.id!).subscribe(data => {
    this.messageService.add({ severity: 'success', summary: 'Успіх', detail: 'Користувача видалено' })});
    this.user = {
      id: '',
      firstName: '',
      lastName: '',
      email: '',
      Password: ''
    };
  }

  findIndexById(id: string): number {
    let index = -1;
    for (let i = 0; i < this.users.length; i++) {
        if (this.users[i].id === id) {
            index = i;
            break;
        }
    }

    return index;
  }
}
