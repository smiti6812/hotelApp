import { DateTime } from "luxon";

export interface ReservationViewHeader{
  days: number[],
  weekDays: string[],
  dayDates: DateTime[]
}
