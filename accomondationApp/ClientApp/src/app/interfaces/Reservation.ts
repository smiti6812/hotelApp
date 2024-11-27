import { DateTime, Info, Interval} from 'luxon';
import { Room } from "../interfaces/Room"
import { Customer } from './Customer';
import { PaymentStatus } from './PaymentStatus';

export interface Reservation{
  reservationId: number,
  rommId: number,
  customer: Customer,
  romm: Room,
  paymentStatusId: number,
  startDate: DateTime,
  endDate: DateTime,
  name: string,
  email: string,
  days: any [],
  reservationDate: DateTime
}
