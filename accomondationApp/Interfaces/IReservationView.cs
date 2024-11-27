using accomondationApp.Models;

namespace accomondationApp.Interfaces
{
    public interface IReservationView
    {
        DateTime[] dayDates { get; set; }
        string[] reservationName { get; set; }
        bool[] reservationSaved { get; set; }
        bool[] reservationStartSaved { get; set; }
        Room room { get; set; }
        bool[] selectedDateArr { get; set; }
    }
}