import { DateTime } from "luxon";

export interface DeleteReservationParams{
  roomId: number,
  date: DateTime,
  pageSelectedDate: DateTime
}
