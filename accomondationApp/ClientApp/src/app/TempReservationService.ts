import {Component, Inject, Injectable } from '@angular/core';
import { TempReservation } from './interfaces/TempReservation';

@Injectable({
  providedIn: 'root',
})

export class TempReservationService{
  public tempReservation!: TempReservation[];
  constructor(){
    this.tempReservation = [];
  }
}
