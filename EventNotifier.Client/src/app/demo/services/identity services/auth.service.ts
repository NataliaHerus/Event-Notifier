import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loginPath = environment.identityUrl + 'identity/login'
  private registerPath = environment.identityUrl + 'identity/register'
  private createPath = environment.identityUrl + 'identity/create'
  private editPath = environment.identityUrl + 'identity/edit'
  private deletePath = environment.identityUrl + 'identity/delete'
  private emailConfirmationPath = environment.identityUrl + 'identity/email-confirmation'
  private resetPasswordPath = environment.identityUrl + 'identity/reset-password'
  private currentUserPath = environment.identityUrl + 'identity/user/'
  private currentUserRolePath = environment.identityUrl + 'identity/role/'
  private getAllPath = environment.identityUrl + 'identity/all/'
  private getUserByIdPath = environment.identityUrl + 'accounts/user/'

  constructor(private http: HttpClient) { 

  }

 login(data: any) : Observable<any> {
    return this.http.post(this.loginPath, data).pipe(
      map((result) => result), catchError(err => throwError(err))
    )
  }

  register(data: any) : Observable<any> {
    return this.http.post(this.registerPath, data).pipe(
      map((result) => result), catchError(err => throwError(err))
    )
  }

  emailConfirmation(data: any) : Observable<any> {
    return this.http.post(this.emailConfirmationPath, data).pipe(
      map((result) => result), catchError(err => throwError(err))
    )
  }

  resetPassword(data: any, userId: string, resetToken: string) : Observable<any> {
    return this.http.post(`${this.resetPasswordPath}/${userId}/${resetToken}`, data).pipe(
      map((result) => result), catchError(err => throwError(err))
    )
  }

  createUser(data: any) : Observable<any> {
    return this.http.post(this.createPath, data).pipe(
      map((result) => result), catchError(err => throwError(err))
    )
  }

  editUser(data: any) : Observable<any> {
    return this.http.put(this.editPath, data).pipe(
      map((result) => result), catchError(err => throwError(err))
    )
  }

  deleteUser(id: string): Observable<any> {
    return this.http.delete(`${this.deletePath}/${id}`).
    pipe(
        map((result) => result), catchError(err => throwError(err))
    )
}

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('currentUser');
    localStorage.removeItem('currentUsername');
  }

  getCurrentUser() {
    return this.http.get<User>(this.currentUserPath)
  }

  getCurrentUserRole() {
    return this.http.get<any>(this.currentUserRolePath)
  }

  getUserById(userId: string | null) {
    return this.http.get<User>(this.getUserByIdPath + userId)
  }

  saveToken(token: string) {
    localStorage.setItem('token', token)
  }
  getAll() : Observable<any> {
    return this.http.get(this.getAllPath).pipe(
    map((result) => result), catchError(err => throwError(err))
    )}

  getToken() {
    return localStorage.getItem('token')
  }

  saveCurrentUsername(username: string) {
    localStorage.setItem('currentUsername', username);
  }

  getCurrentUsername() {
    return localStorage.getItem('currentUsername')
  }

  isAuthenticated() {
    if(this.getToken())
      return true;
    return false;
  }
}