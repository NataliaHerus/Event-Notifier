import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { AppLayoutComponent } from "./layout/app.layout.component";
import { LoginComponent } from './demo/components/auth/login/login.component';
import { AuthGuard } from './demo/guards/auth-guard';
import { HomePageComponent } from './demo/components/home-page/home-page.component';

@NgModule({
    imports: [
        RouterModule.forRoot([
            {   
                path: '', component: AppLayoutComponent,
                children: [
                    { path: 'event', loadChildren: () => import('./demo/components/event/event.module').then(m => m.EventModule)},
                    { path: 'profile', loadChildren: () => import('./demo/components/profile/profile.module').then(m => m.ProfileModule)},
                    { path: 'admin', loadChildren: () => import('./demo/components/admin-panel/admin-panel.module').then(m => m.AdminPanelModule), canActivate: [AuthGuard], data: {roles: ['Admin']}},
                    { path: '', redirectTo: 'home', pathMatch: 'full' },
                    { path: 'home', component: HomePageComponent },
                ]
            },
            { path: 'auth', loadChildren: () => import('./demo/components/auth/auth.module').then(m => m.AuthModule) },
            { path: 'notfound', component: NotfoundComponent },
            { path: '**', redirectTo: '/notfound' },
        ], { scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled', onSameUrlNavigation: 'reload' })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
