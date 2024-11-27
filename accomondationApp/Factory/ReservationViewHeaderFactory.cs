using System.Globalization;

using accomondationApp.Interfaces;
using accomondationApp.ViewModel;

namespace accomondationApp.Factory
{
    public static class ReservationViewHeaderFactory
    {
        private static string getWeekDay(DateTime date)
        {
            var dayOfWeek = date.DayOfWeek;
            CultureInfo culture = CultureInfo.InvariantCulture;
            return culture.DateTimeFormat.DayNames[(int)dayOfWeek].Substring(0, 3);
        }

        public static IReservationViewHeader GetReservationViewHeader(int [] monthArr, DateTime pageSelectedDate)
        {
            var reservationViewHeader = new ReservationViewHeader();
            int allDays = monthArr.Sum(s => s);
            string[] weekDays = new string[allDays];
            int [] days = new int[allDays];
            DateTime[] headerDayDates = new DateTime[allDays];
           
            for (int j = 0; j < allDays; j++)
            {
                DateTime actualDate = pageSelectedDate.AddDays(j);
                weekDays[j] = getWeekDay(actualDate);
                days[j] =actualDate.Day;
                headerDayDates[j] = actualDate;
            }
            reservationViewHeader.DayDates = headerDayDates;
            reservationViewHeader.WeekDays = weekDays;
            reservationViewHeader.Days = days;
            return reservationViewHeader;
        }
    }
}
