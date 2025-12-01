import { LayoutService } from 'src/app/layout/service/app.layout.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/demo/services/identity services/auth.service';
import { HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { MessageService } from 'primeng/api';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    styles: [`
        :host ::ng-deep .pi-eye,
        :host ::ng-deep .pi-eye-slash {
            transform:scale(1.6);
            margin-right: 1rem;
            color: var(--primary-color) !important;
        }
    `],
    providers: [MessageService]
})

export class LoginComponent{
  loginForm: FormGroup;
  errorMessage: string = '';
  valCheck: string[] = ['remember'];

  constructor(
    public layoutService: LayoutService,
    private fb: FormBuilder, 
    private authService: AuthService,
    private messageService: MessageService,
    private router: Router
    ) { 
      this.loginForm = this.fb.group({
        'email': ['', [Validators.required, Validators.minLength(4), Validators.email, Validators.maxLength(20)]],
        'password': ['', [Validators.required,  Validators.maxLength(32)]]
      })
    
    }


  login() {
    this.authService.login(this.loginForm.value).subscribe( 
      data => {
      this.authService.saveToken(data['token']);
      this.authService.saveCurrentUsername(this.loginForm.value['email']);
      this.router.navigate(['/'], { replaceUrl: true });
    },
    (error) => {
      let errorMessages = [];
      let errors = (error as HttpErrorResponse).error.errors;
      for (let field in errors) {
        errorMessages.push(errors[field][0]);
      }
      this.messageService.add({ severity: 'error', summary: 'Вхід не здійснено!', detail: errorMessages.join(', ') });
    })
  }

  get loginFormControl() {
    return this.loginForm.controls;
  }
}