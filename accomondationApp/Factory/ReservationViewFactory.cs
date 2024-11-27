using accomondationApp.Interfaces;
using accomondationApp.Models;
using accomondationApp.ViewModel;

namespace accomondationApp.Factory
{
    public static class ReservationViewFactory
    {
        public static IReservationView GetReservationView(Room room, DateTime pageSelectedDate, IReservationViewProperties reservationViewProperties, int [] monthArr)
        {
            var reservationView = new ReservationView();
            reservationView.room = room;
            var dayDates = new List<DateTime>();
            var reservationName = new List<string>();
            var reservationStartSaved = new List<bool>();
            var reservationSaved = new List<bool>();
            var selectedDateArr = new List<bool>();
            for (int j = 0; j < monthArr.Length; j++)
            {
                DateTime firstDayOfTheMonth = pageSelectedDate.AddMonths(j).AddDays(-pageSelectedDate.AddMonths(j).Day).Date;                
                for (int m = 1; m <= monthArr[j]; m++)
                {
                    DateTime actualDate = firstDayOfTheMonth.AddDays(m);
                    dayDates.Add(actualDate);
                    var checkReservation = reservationViewProperties.ReturnReservationViewProperties(room.RoomId, actualDate);
                    reservationName.Add(checkReservation.reservationName);
                    reservationStartSaved.Add(checkReservation.startSaved);
                    reservationSaved.Add(checkReservation.endSaved);
                    selectedDateArr.Add(checkReservation.selectedDateArr);
                }
            }
            reservationView.dayDates = dayDates.ToArray<DateTime>();
            reservationView.reservationName = reservationName.ToArray<string>();
            reservationView.reservationStartSaved = reservationStartSaved.ToArray<bool>();
            reservationView.reservationSaved = reservationSaved.ToArray<bool>();
            reservationView.selectedDateArr = selectedDateArr.ToArray<bool>();
            return reservationView;
        }
    }
}
