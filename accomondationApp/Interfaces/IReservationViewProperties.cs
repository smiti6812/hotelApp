using accomondationApp.Models;

namespace accomondationApp.Interfaces
{
    public interface IReservationViewProperties
    {
        (bool checkReservation, Reservation reservation) CheckReservation(int roomId, DateTime date);
        (bool checkReservation, bool startSaved, bool endSaved, string reservationName, bool selectedDateArr) ReturnReservationViewProperties(int roomId, DateTime date);
    }
}
