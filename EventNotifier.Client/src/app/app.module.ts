import { NgModule } from '@angular/core';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AppLayoutModule } from './layout/app.layout.module';
import { NotfoundComponent } from './demo/components/notfound/notfound.component';
import { AuthModule } from './demo/components/auth/auth.module';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptorService } from './demo/services/identity services/token-interceptor.service';
import { HomePageComponent } from './demo/components/home-page/home-page.component';

@NgModule({
    declarations: [
        AppComponent, NotfoundComponent, HomePageComponent
    ],
    imports: [
        AppRoutingModule,
        AppLayoutModule,
        AuthModule,
    ],
    providers: [
        { provide: LocationStrategy, useClass: HashLocationStrategy },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: TokenInterceptorService,
            multi: true
          },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
