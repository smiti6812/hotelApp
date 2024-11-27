import { Reservation } from "./Reservation";
import { DateTime } from "luxon";

export interface ReservationParams{
  reservation: Reservation,
  pageSelectedDate:  DateTime
}
