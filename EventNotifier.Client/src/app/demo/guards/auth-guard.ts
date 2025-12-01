import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { AuthService } from "../services/identity services/auth.service";
import { map } from "rxjs";

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authService: AuthService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {

        return this.authService.getCurrentUserRole().pipe(
            map((user) => {
                if (user) {
                    // check if route is restricted by role
                    if (route.data['roles'] && !route.data['roles'].includes(user.role)) {
                        // role not authorized so redirect to home page
                        this.router.navigate(['/']);
                        return false;
                    }
                } else {
                    // not logged in so redirect to login page with the return url 
                    this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
                    return false;
                }

                // authorized so return true
                return true;
            }));
    }
}