import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { EmailConfirmationComponent } from './email-confirmation/email-confirmation.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { EmailConfirmedComponent } from './email-confirmed/email-confirmed.component';

@NgModule({
    imports: [RouterModule.forChild([
        { path: 'register', component: RegisterComponent },
        { path: 'access', loadChildren: () => import('./access/access.module').then(m => m.AccessModule) },
        { path: 'login', loadChildren: () => import('./login/login.module').then(m => m.LoginModule) },
        { path: 'email-confirmation', component: EmailConfirmationComponent},
        { path: 'email-confirmed', component: EmailConfirmedComponent},
        { path: 'reset-password/:id/:token', component: ResetPasswordComponent},
        { path: '**', redirectTo: '/notfound' }
    ])],
    exports: [RouterModule]
})
export class AuthRoutingModule { }
