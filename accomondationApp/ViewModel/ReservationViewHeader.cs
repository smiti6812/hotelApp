using accomondationApp.Interfaces;

namespace accomondationApp.ViewModel
{
    public class ReservationViewHeader : IReservationViewHeader
    {
        public int[] Days { get; set; }
        public string[] WeekDays { get; set; }
        public DateTime[] DayDates { get; set; }
    }
}
