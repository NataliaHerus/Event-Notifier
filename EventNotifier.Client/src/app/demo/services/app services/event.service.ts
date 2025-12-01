import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../../models/user';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  private createPath = environment.apiUrl + 'event/create'
  private collectionPath = environment.apiUrl + 'event/collection'
  private collectionClosestPath = environment.apiUrl + 'event/collection/closest'
  private updatePath = environment.apiUrl + 'event/update'
  private deletePath = environment.apiUrl + 'event/delete'
  private getPath = environment.apiUrl + 'event/get'
  private uploadFilePath = environment.apiUrl + 'event/save/file'

  constructor(private http: HttpClient) { 

  }

 create(data: any) : Observable<any> {
    return this.http.post(this.createPath, data).pipe(
      map((result) => result), catchError(err => throwError(err))
    )
  }

  uploadFile(data: any, eventId: number) : Observable<any> {
    const formData = new FormData();
    formData.append('File', data, 'jpg');
    formData.append(`EventId`, eventId.toString())
      return this.http.post(this.uploadFilePath, formData);
  }

  update(data: any): Observable<any> {
    return this.http.put(this.updatePath, data).
    pipe(
        map((result) => result), catchError(err => throwError(err))
      )
    }

    getById(id: number): Observable<any> {
        return this.http.get(`${this.getPath}/${id}`).
        pipe(
            map((result) => result), catchError(err => throwError(err))
        )
    }

    deleteById(id: number): Observable<any> {
        return this.http.delete(`${this.deletePath}/${id}`).
        pipe(
            map((result) => result), catchError(err => throwError(err))
        )
    }

    getAll() : Observable<any> {
        return this.http.get(this.collectionPath).pipe(
        map((result) => result), catchError(err => throwError(err))
        )
  }

  getClosest() : Observable<any> {
    return this.http.get(this.collectionClosestPath).pipe(
    map((result) => result), catchError(err => throwError(err))
    )
}
}