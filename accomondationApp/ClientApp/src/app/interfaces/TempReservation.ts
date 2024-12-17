import { DateTime } from "luxon";

export interface TempReservation{
  id: number,
  name: string,
  roomNumber: string,
  endDate: DateTime
}

