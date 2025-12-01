import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppLayoutComponent } from 'src/app/layout/app.layout.component';
import { LoginComponent } from './login.component';

@NgModule({
    imports: [RouterModule.forChild([
        { path: '', component: LoginComponent }
    ])],
    exports: [RouterModule]
})
export class LoginRoutingModule { }
