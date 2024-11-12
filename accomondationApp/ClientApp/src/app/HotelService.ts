import {Component, Inject, Injectable } from '@angular/core';
import { DateTime, Info, Interval} from 'luxon';
import { RoomCapacity } from './interfaces/RoomCapacity';
import { ReservationView } from './interfaces/ReservationView';
import { Reservation } from './interfaces/Reservation';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Room } from './interfaces/Room';


@Injectable({
  providedIn: 'root',
})

export class HotelService {
  months: number = 3;
  monthArr: number[] = [];
 reservationView: ReservationView[] = [];
 http: HttpClient;
 rooms!: Observable<Room>;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string){
    this.http = http;
    for(let i = 0; i < this.months; i++){
      this.monthArr.push(DateTime.now().plus({month:i}).daysInMonth!);
    }
  }

  getReservationView1(currDate: DateTime): Observable<ReservationView[]> {
    let param: any = {'currDate': currDate};
    return this.http.get<ReservationView[]>('https://localhost:7246/reservation/',{params: param});
  }

  getRoom(): Observable<Room> {
    return this.http.get<Room>(`https://localhost:7246/reservation`);
  }
/*
  getRooms(): Room[] {
    this.http.get<Room>('https://localhost:7246/reservation').subscribe(result => {
      this.rooms = result;
    }, error => console.error(error));
    return this.rooms;
  }
*/
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


  getReservationView(pageSelectedDate: DateTime): ReservationView[]{
    let daysInMonth: number = pageSelectedDate.daysInMonth!;
    let reservationView   = [] as ReservationView[];


    for(let index = 0; index < 5; index++){
      let reservationView1: ReservationView = <ReservationView>{};
      reservationView1.room ={
        roomNumber: "Room" + (index + 1),
        roomCapacity : {roomCapacityId: 2, capacity :2},
        status: "available"
      }
      reservationView1.dayDates = [];
      reservationView1.reservationSaved = [];
      reservationView1.reservationStartSaved = [];
      reservationView1.reservationName = [];
      for(let j= 0; j < this.monthArr.length; j++){
        for(let i = 1; i<= this.monthArr[j]; i++){
          reservationView1.dayDates.push(pageSelectedDate.plus({month:j}).minus({days:pageSelectedDate.plus({month:j}).day}).plus({days: i}).minus({minutes: pageSelectedDate.minute}).minus({hours: pageSelectedDate.hour}));
          reservationView1.reservationSaved.push(false);
          reservationView1.reservationStartSaved.push(false);
          reservationView1.reservationName.push('');
        }
      }
      reservationView.push(reservationView1);
    }

    for(let index = 5; index < 10; index++){
      let reservationView1: ReservationView = <ReservationView>{};
      reservationView1.room ={
        roomNumber: "Room" + (index + 1),
        roomCapacity : {roomCapacityId: 3, capacity :3},
        status: "available"
      }
      reservationView1.dayDates = [];
      reservationView1.reservationSaved = [];
      reservationView1.reservationStartSaved = [];
      reservationView1.reservationName = [];
      for(let j= 0; j < this.monthArr.length; j++){
        for(let i = 1; i<= this.monthArr[j]; i++){
          reservationView1.dayDates.push(pageSelectedDate.plus({month:j}).minus({days:pageSelectedDate.plus({month:j}).day}).plus({days: i}).minus({minutes: pageSelectedDate.minute}).minus({hours: pageSelectedDate.hour}));
          reservationView1.reservationSaved.push(false);
          reservationView1.reservationStartSaved.push(false);
          reservationView1.reservationName.push('');
        }
      }
      reservationView.push(reservationView1);
    }
    for(let index = 10; index < 15; index++){
      let reservationView1: ReservationView = <ReservationView>{};
      reservationView1.room ={
        roomNumber: "Room" + (index + 1),
        roomCapacity : {roomCapacityId: 4, capacity :4},
        status: "available"
      }
      reservationView1.dayDates = [];
      reservationView1.reservationSaved = [];
      reservationView1.reservationStartSaved = [];
      reservationView1.reservationName = [];
      for(let j= 0; j < this.monthArr.length; j++){
        for(let i = 1; i<= this.monthArr[j]; i++){
          reservationView1.dayDates.push(pageSelectedDate.plus({month:j}).minus({days:pageSelectedDate.plus({month:j}).day}).plus({days: i}).minus({minutes: pageSelectedDate.minute}).minus({hours: pageSelectedDate.hour}));
          reservationView1.reservationSaved.push(false);
          reservationView1.reservationStartSaved.push(false);
          reservationView1.reservationName.push('');
        }
      }
       reservationView.push(reservationView1);
    }
    /*
    let param: any = {'currDate': DateTime.now()};
    this.http.get<ReservationView[]>('https://localhost:7246/reservation',{params: param}).subscribe({
      next: (result: ReservationView[]) => this.reservationView = result,
      error: (err: HttpErrorResponse) => console.log(err)
  });
  */
    return reservationView;
  }

}
