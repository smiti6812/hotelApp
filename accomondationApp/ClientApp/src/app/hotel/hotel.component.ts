import { Component, Input, Inject, NgModule, ViewChild, ElementRef, OnInit } from '@angular/core';
import { CommonModule, AsyncPipe } from "@angular/common";
import { ReservationView } from '../interfaces/ReservationView';
import { Reservation } from '../interfaces/Reservation';
import { DateTime, Info, Interval} from 'luxon';
import { HotelService } from '../HotelService';
import { FormsModule } from '@angular/forms';
import { ReservationformComponent } from '../reservationform/reservationform.component';
import { ReservationForm } from '../interfaces/ReservationForm';
import { Observable } from 'rxjs';
import { Room } from '../interfaces/Room';
import { ReservationViewWrapper } from '../interfaces/ReservationViewWrapper';
import { Customer } from '../interfaces/Customer';
import { PaymentStatus } from '../interfaces/PaymentStatus';
import { bootstrapApplication } from '@angular/platform-browser';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthGuard } from '../guards/auth.guard';

@Component({
  selector: 'app-hotel',
  //standalone: true,
  //imports: [CommonModule, FilterPipe, FormsModule, SortPipe, SortParamsDirective, ReservationformComponent],
  templateUrl: './hotel.component.html',
  styleUrls: ['./hotel.component.css']
})

export class HotelComponent implements OnInit  {
  @ViewChild('search') searchElement: ElementRef;
  @Input() selected: any[] = [];
  @Input() selectedRow: number = -1;
  @Input() start: number = -1;
  @Input() end: number = -1;
  @Input() reservationViewWrapper!: ReservationViewWrapper;
  showModal: boolean = false;
  pageSelectedDate:DateTime = DateTime.now();
  hotelService: HotelService = Inject(HotelService);
  authService: AuthGuard = Inject(AuthGuard);
  isSorted: number = 0;
  searchText: string = '';
  column:string = 'Room';
  direction:string = 'asc';
  type:string = 'string';
  reservationForm: ReservationForm = {} as ReservationForm;


  constructor(private _hotelService: HotelService, private jwtHelper: JwtHelperService, private router:Router){
    this.hotelService = _hotelService;
  }

  isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    return false;
  }

  getDateString(date: DateTime){
    return date.toString().substring(0,10);
  }

  getDate(date: DateTime){
    return DateTime.fromISO(date.toString().substring(0,10));
  }

  checkWeekend(date: DateTime):boolean{
    return DateTime.fromISO(date.toString()).isWeekend;
  }

  getWeekDay(index: number){
    return this.reservationViewWrapper.reservationViewHeader.weekDays[index] + this.reservationViewWrapper.reservationViewHeader.days[index];
  }

  ngOnInit(): void {
    this.hotelService.getReservationViewWrapper(this.pageSelectedDate).subscribe(result =>
      this.reservationViewWrapper = result,
      error => console.error(error),
      () => console.log('ReservationViewwrapper loaded'),
    );

    this.clearSelectedDateArr();
  }

  focusSearch() {
    setTimeout(() => {
      this.searchElement.nativeElement.focus();
    }, 0);
  }
  cancelForm(e: any) {
    this.showModal = false;
    this.start = -1;
    this.end = -1;
    this.selectedRow = -1;
    this.selected = [];
    this.clearSelectedDateArr();
    this.focusSearch();
  }

  getReservationForm(form: ReservationForm) {
    let cust = {} as Customer;
    cust.name = form.name;
    cust.email = form.email;
    let res: Reservation = <Reservation>{
      rommId: form.view.room.roomId,
      customer: cust,
      paymentStatusId: 2,
      startDate: form.startDate,
      endDate: form.endDate,
      reservationDate: DateTime.now()
    };
    this.hotelService.saveReservation(res, this.pageSelectedDate).subscribe(
      result =>{ this.reservationViewWrapper = result }
    );

    this.showModal = false;
    this.start = -1;
    this.end = -1;
    this.selectedRow = -1;
  }

  setSortParams(param: any) {
    this.direction = param.dir;
    this.column = param.col;
    this.type = param.typ;
    this.clearSelectedDateArr();
  }

  onMouseArrDown(row: number, col: number, date: DateTime, roomId: number) {
    if (!this.checkReservation1(roomId, date)) {
      this.clearSelectedDateArr();
      this.selected.push(date);
      this.reservationViewWrapper.reservationView[row].selectedDateArr[col] = true;
      this.start = col + 1;
      this.selectedRow = row;
    }
  }

  onMouseArrOver(row: number, col: number, date: DateTime, roomId: number) {
    if (this.start > -1 && !this.checkReservation1(roomId, date)) {
      if (this.selectedRow === row) {
        for (let i = this.start; i <= col; i++) {
          this.reservationViewWrapper.reservationView[row].selectedDateArr[col] = true;
        }
      }
      else {
        this.clearSelectedDateArr();
        this.selectedRow = -1;
      }

      if (this.end > col && this.end < this.reservationViewWrapper.reservationViewHeader.days.length) {
        this.reservationViewWrapper.reservationView[row].selectedDateArr[this.end] = false;
      }
      this.end = col;
    }
    else {
      for (let j = col; j >= this.start - 1; j--) {
        this.reservationViewWrapper.reservationView[row].selectedDateArr[j] = false;
      }
    }
  }

  openModalAndSelectRangeBoxForRoomArr(view: ReservationView, item: any, row: number, col: number, date: DateTime) {
    if (this.start > -1 && this.selectedRow === row && !this.checkReservationInRange(row, this.start, col + 1, item)) {
      this.end = col;
      this.reservationForm.name = '';
      this.reservationForm = {} as ReservationForm;
      this.reservationForm.roomNumber = view.room.roomNumber;
      this.reservationForm.capacity = view.room.roomCapacity.capacity;
      this.reservationForm.startDate = item[this.start - 1];
      this.reservationForm.endDate = item[col];
      this.reservationForm.view = view;
      this.showModal = true;
      //const diff = DateTime.local(res.endDate.year, res.endDate.month, res.endDate.day).
      //diff(DateTime.local(res.startDate.year, res.startDate.month, res.startDate.day), 'days').days + 1;

    }
    else {
      this.focusSearch();
      this.clearSelectedDateArr();
    }
  }

  checkReservationInRange(row: number, start: number, end: number, dates: any[]): boolean {
    for (let j = start; j <= end; j++) {
      if (this.checkReservation1(this.reservationViewWrapper.reservationView[row].room.roomId, dates[j - 1])) {
        return true;
      }
    } date: DateTime
    return false;
  }

  deleteReservation(view: ReservationView, date: DateTime, row: number) {
    if(confirm("Do you really want to delete your reservation?")){
      this.hotelService.deleteReservation(view.room.roomId, date, this.pageSelectedDate).subscribe(
        result =>{ this.reservationViewWrapper = result }
      );
    }
  }

  closeModal = (): void => {
    this.showModal = false;
    this.clearSelectedDateArr();
  }

  checkReservationStart(roomId: number, date: DateTime): boolean {
    if (this.reservationViewWrapper.reservations.filter(f => f.romm.roomId === roomId && (
      f.startDate === date))) {
      return true;
    }
    return false;

  }

  checkReservation1(roomId: number, date: DateTime): boolean {
    if (this.reservationViewWrapper.reservations.find(f => f.romm.roomId === roomId &&
      this.getDate(f.startDate) <= this.getDate(date) && this.getDate(date) <= this.getDate(f.endDate))) {
      return true;
    }
    return false;
  }

  checkReservation(roomId: number, day: number): boolean {
    if (this.reservationViewWrapper.reservations.filter(f => f.romm.roomId === roomId &&
      f.startDate.monthShort === this.pageSelectedDate.monthShort &&
      f.startDate.year === this.pageSelectedDate.year).flatMap(fm => fm.days).find(d => d === day)) {
      return true;
    }
    return false;
  }

  goToPreviousMonth() {
    this.reservationViewWrapper = { } as ReservationViewWrapper;
    this.pageSelectedDate = this.pageSelectedDate.minus({ months: 1 });
    this.hotelService.getReservationViewWrapperNextPrev(this.pageSelectedDate).subscribe(result =>
      this.reservationViewWrapper = result,
      error => console.error(error),
      () => console.log('ReservationViewWrapper loaded')
    );
  }

  goToNextMonth() {
    this.reservationViewWrapper = { } as ReservationViewWrapper;
    this.pageSelectedDate = this.pageSelectedDate.plus({ months: 1 });
    this.hotelService.getReservationViewWrapperNextPrev(this.pageSelectedDate).subscribe(result =>
      this.reservationViewWrapper = result,
      error => console.error(error),
      () => console.log('ReservationViewWrapper loaded')
    );
  }

  public clearSelectedDateArr() {
    for (let i = 0; i < this.reservationViewWrapper?.monthArr?.length; i++) {
      for (let j = 0; j < this.reservationViewWrapper.reservationViewHeader?.dayDates.length; j++) {
        this.reservationViewWrapper.reservationView[i].selectedDateArr[j] = false;
      }
    }
    this.selected = [];
  }
}
