using accomondationApp.Models;

namespace accomondationApp.ViewModel
{
    public class ReservationView
    {
        public Room room { get; set; }
        public DateTime[] dayDates { get; set; }
        public bool[]  reservationSaved { get; set; }
        public bool[] reservationStartSaved { get; set; }
        public string[]  reservationName { get; set; }
    }
}
