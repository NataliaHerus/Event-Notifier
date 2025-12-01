import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRoutingModule } from './auth-routing.module';
import { RegisterComponent } from './register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { EmailConfirmedComponent } from './email-confirmed/email-confirmed.component';

@NgModule({
    imports: [
        CommonModule,
        AuthRoutingModule,
        FormsModule,
        ReactiveFormsModule,
        PasswordModule,
        ButtonModule,
        InputTextModule,
        ToastModule
    ],
    declarations: [
    RegisterComponent,
    EmailConfirmationComponent,
    ResetPasswordComponent,
    EmailConfirmedComponent
  ]
})
export class AuthModule { }
