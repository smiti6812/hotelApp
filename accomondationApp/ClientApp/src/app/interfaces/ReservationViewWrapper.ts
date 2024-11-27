import { ReservationView } from "./ReservationView";
import { ReservationViewHeader } from "./ReservationViewHeader";
import { Reservation } from "./Reservation";

export interface ReservationViewWrapper{
  reservationView: ReservationView[],
  reservationViewHeader: ReservationViewHeader,
  monthArr: number[],
  reservations: Reservation[]
}
