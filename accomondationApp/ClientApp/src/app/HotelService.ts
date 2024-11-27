import {Component, Inject, Injectable } from '@angular/core';
import { DateTime, Info, Interval} from 'luxon';
import { RoomCapacity } from './interfaces/RoomCapacity';
import { ReservationView } from './interfaces/ReservationView';
import { Reservation } from './interfaces/Reservation';
import { HttpClient, HttpParams, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { Room } from './interfaces/Room';
import { ReservationViewWrapper } from './interfaces/ReservationViewWrapper';
import { catchError, map, tap } from 'rxjs/operators';
import { MessageService } from './message.service';
import { ReservationParams } from './interfaces/ReservationParams';
import { DeleteReservationParams } from './interfaces/DeleteReservationParams';


@Injectable({
  providedIn: 'root',
})

export class HotelService {
 reservation: Reservation = { } as Reservation;
 months: number = 3;
 monthArr: number[] = [];
 reservationView: ReservationView[] = [];
 http: HttpClient;
 rooms!: Observable<Room>;
 messageService: MessageService;
 httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, messageService: MessageService){
    this.http = http;
    this.messageService = messageService;
    for(let i = 0; i < this.months; i++){
      this.monthArr.push(DateTime.now().plus({month:i}).daysInMonth!);
    }
  }

  saveReservation(res: Reservation, pageSelectedDate: DateTime): Observable<ReservationViewWrapper> {
    let reservationParams = {} as ReservationParams;
    reservationParams.reservation = res;
    reservationParams.pageSelectedDate = pageSelectedDate;
    return this.http.post<ReservationViewWrapper>('https://localhost:7246/reservation/', reservationParams, this.httpOptions).pipe(
      tap((newRes: ReservationViewWrapper) => this.log(`added reservation `)),
      catchError(this.handleError<ReservationViewWrapper>('addReservation'))
    );
  }

  deleteReservation(roomId: number, date: DateTime, pageSelectedDate: DateTime): Observable<ReservationViewWrapper> {
    let deleteReservationParams = {} as DeleteReservationParams;
    deleteReservationParams.roomId = roomId;
    deleteReservationParams.date = date;
    deleteReservationParams.pageSelectedDate = pageSelectedDate
    return this.http.post<ReservationViewWrapper>('https://localhost:7246/reservation/delete', deleteReservationParams, this.httpOptions).pipe(
      tap((newRes: ReservationViewWrapper) => this.log(`deleted reservation `)),
      catchError(this.handleError<ReservationViewWrapper>('deleteReservation'))
    );
  }

  getReservationViewWrapper(currDate: DateTime): Observable<ReservationViewWrapper> {
    let param: any = {'currDate': currDate};
    return this.http.get<ReservationViewWrapper>('https://localhost:7246/reservation/',{params: param});
  }

  getReservationViewWrapperNextPrev(currDate: DateTime): Observable<ReservationViewWrapper> {
    let param: any = {'currDate': currDate};
    return this.http.get<ReservationViewWrapper>('https://localhost:7246/reservation/nextprev/',{params: param});
  }

  private handleError<T>(operation = 'operation', result?: T) {
      return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
  private log(message: string) {
    this.messageService.add(`HotelService: ${message}`);
  }
  public getDaysInMonth(pageSelectedDate: DateTime): any[] {
    this.monthArr = [];
    for(let i = 0; i < this.months; i++){
      this.monthArr.push(pageSelectedDate.plus({month:i}).daysInMonth!)
    }
    let daysInMonth = pageSelectedDate.daysInMonth!;
    let daysInMonthArr: any[] = [];
    for(let j= 0; j < this.monthArr.length; j++){
      for(let i = 1; i<= this.monthArr[j]; i++){
        daysInMonthArr.push({
            name: i,
            weekDay: pageSelectedDate.plus({month:j}).minus({days:pageSelectedDate.plus({month:j}).day}).plus({days: i}).minus({minutes: pageSelectedDate.minute}).minus({hours: pageSelectedDate.hour}).weekdayShort,
            dayDates: pageSelectedDate.plus({month:j}).minus({days:pageSelectedDate.plus({month:j}).day}).plus({days: i}).minus({minutes: pageSelectedDate.minute}).minus({hours: pageSelectedDate.hour})
          }
        );
    }
  }

    return daysInMonthArr;
  }

   getRoomCapacity(): RoomCapacity[]{
    let roomTypes = [
      {
        roomCapacityId: 2,
        capacity: 2
      },
      {
        roomCapacityId: 3,
        capacity: 3,
      },
      {
        roomCapacityId: 4,
        capacity: 4,
      }
    ];
    return roomTypes;
  }
}
