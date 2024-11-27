using accomondationApp.Models;

namespace accomondationApp.Repositories
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation?>> GetReservationsAsync(DateTime startDate, DateTime endDate);
        Task<bool?> DeleteReservationAsync(int reservationId);
        Task<Reservation?>? GetSingleReservationAsync(int reservationId);
        Task<Reservation?> AddReservationAsync(Reservation reservation);
        Task<Reservation> CheckReturnReservation(int roomId, DateTime date);
        IEnumerable<Reservation> GetReservations(DateTime date);
        int DisplayMonths();

    }
}
