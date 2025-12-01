import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../../models/user';

@Injectable({
  providedIn: 'root'
})
export class PuzzleService {
  private createPath = environment.apiUrl + 'api/puzzle/create'
  private userTaskPath = environment.apiUrl + 'api/puzzle/currentUser'
  private Url = 'https://localhost:'

  constructor(private http: HttpClient) { 

  }

 create(data: any, port:any) : Observable<any> {
  console.log(`${this.Url}${port}/api/puzzle/create/${port}`, data, port);
    return this.http.post(`${this.Url}${port}/api/puzzle/create/${port}`, data, port).pipe(
      map((result) => result), catchError(err => throwError(err))
    )
  }

  get() : Observable<any> {
    return this.http.get(this.userTaskPath).pipe(
      map((result) => result), catchError(err => throwError(err))
    )
  }

}