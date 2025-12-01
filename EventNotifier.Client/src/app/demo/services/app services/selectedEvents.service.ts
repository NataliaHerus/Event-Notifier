import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SelectedEventsService {
  private createPath = environment.apiUrl + 'selectedEvents/create'
  private deletePath = environment.apiUrl + 'selectedEvents/delete'
  private getByUserPath = environment.apiUrl + 'selectedEvents/collection/byUser'
  private getEvent = environment.apiUrl + 'selectedEvents/get'

  constructor(private http: HttpClient) { 

  }

    create(data: any) : Observable<any> {
        return this.http.post(this.createPath, data).pipe(
        map((result) => result), catchError(err => throwError(err))
        )
    }

    deleteById(id: number): Observable<any> {
        return this.http.delete(`${this.deletePath}/${id}`).
        pipe(
            map((result) => result), catchError(err => throwError(err))
        )
    }

    get(id: number): Observable<any> {
      return this.http.get(`${this.getEvent}/${id}`).
      pipe(
          map((result) => result), catchError(err => throwError(err))
      )
  }
    getAllByUser() : Observable<any> {
        return this.http.get(this.getByUserPath).pipe(
        map((result) => result), catchError(err => throwError(err))
        )
  }
}