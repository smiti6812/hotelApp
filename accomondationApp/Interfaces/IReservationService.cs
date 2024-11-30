using accomondationApp.Models;

namespace accomondationApp.Interfaces
{
    public interface IReservationService
    {
        Task<Reservation> SaveReservation(Reservation reservation);
        Task<bool?> DeleteReservation(int reservationId);
        Task<IEnumerable<Reservation>> GetReservations(DateTime date);       
        int DisplayedMonth();
        Task<IEnumerable<Room>> GetAllRooms();
        Task<Reservation> CheckReturnReservation(int roomId, DateTime date);
    }
}
