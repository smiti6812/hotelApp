import {Component, Inject, Injectable } from '@angular/core';
import { DateTime, Info, Interval} from 'luxon';
import { HttpClient, HttpParams, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { MessageService } from './message.service';
import { Slide } from './interfaces/Slide';
import { PicturePath } from './interfaces/PicturePath';


@Injectable({
  providedIn: 'root',
})

export class SlideService {
  http: HttpClient;
  messageService: MessageService;
  httpOptions = {
   headers: new HttpHeaders({ 'Content-Type': 'application/json' })
 };
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, messageService: MessageService){
    this.http = http;
    this.messageService = messageService;
  }

  getSlides(): Observable<Slide[]> {
    return this.http.get<Slide[]>('https://localhost:7246/slides/');
  }

  private handleError<T>(operation = 'operation', result?: T) {
      return (error: any): Observable<T> => {
      console.error(error);
      this.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }
  private log(message: string) {
    this.messageService.add(`SlideService: ${message}`);
  }
}

